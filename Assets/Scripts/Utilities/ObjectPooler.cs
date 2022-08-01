using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    /// ObjectPooler class that efficiently reuses prefab instances to save on Instantiation and Destruction time
    /// </summary>
    public class ObjectPooler : Singleton<ObjectPooler>
    {
        private readonly Dictionary<Object, List<Object>> pooledGameObjects = new Dictionary<Object, List<Object>>();
        private readonly Dictionary<int, Object> prefabByInstanceID = new Dictionary<int, Object>();
        
        /// <summary>
        /// Instantiates a prefab.  If possible, utilizes a previously pooled object
        /// </summary>
        /// <param name="prefab">Prefab to instantiate</param>
        /// <param name="parent">Transform the instantiated object will be a child of</param>
        /// <param name="isLODObject">If true, this is a LOD object and its active instances will be tracked (for unloading upon complete pooling).</param>
        /// <returns>Returns the instantiated prefab</returns>
        public new static T Instantiate<T>(T prefab, Transform parent) where T : Object
        {
            GameObject gameObj = prefab is Component component ? component.gameObject : (prefab as GameObject);
            
            T g = InstantiatePrefab(prefab, () =>
            {
                return Object.Instantiate(prefab, parent) as T;
            });
            
            GameObject instanceObject = g is Component instanceComp ? instanceComp.gameObject : (g as GameObject);

            if (instanceObject != null && instanceObject.transform.parent != parent)
            {
                instanceObject.transform.SetParent(parent);
            }
            
            InitializeGameObject(g);
            return g;
        }
        
        /// <summary>
        /// Instantiates a prefab.  If possible, utilizes a previously pooled object
        /// </summary>
        /// <param name="prefab">Prefab to instantiate</param>
        /// <param name="position">World position of the instantiated object</param>
        /// <param name="rotation">World rotation of the instantiated object</param>
        /// <returns>Returns the instantiated prefab</returns>
        public new static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            return Instantiate(prefab, position, rotation, null);
        }
        
        public static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, 
            Transform parent) where T : Object
        {
            GameObject prefabGameObj = prefab is Component component ? component.gameObject : (prefab as GameObject);
            
            T g = InstantiatePrefab(prefab, () =>
            {
                return Object.Instantiate(prefab, position, rotation, parent) as T;
            });

            GameObject instanceGameObj = g is Component comp ? comp.gameObject : (g as GameObject);

            if (instanceGameObj != null)
            {
                if (instanceGameObj.transform.parent != parent)
                {
                    instanceGameObj.transform.SetParent(parent);
                }

                instanceGameObj.transform.position = position;
                instanceGameObj.transform.rotation = rotation;
            }
            
            
            InitializeGameObject(g);
            return g;
        }
        
        private static T InstantiatePrefab<T>(T prefab, System.Func<T> instantiationFunction) where T : Object
        {
            if (prefab == null || instantiationFunction == null)
            {
                Debug.LogError("Attempting to instantiate a null prefab!");
                return null;
            }
            if (Instance.pooledGameObjects.ContainsKey(prefab))
            {
                List<Object> pooledPrefabInstances = Instance.pooledGameObjects[prefab];

                while (pooledPrefabInstances.Count > 0 && pooledPrefabInstances[0] == null)
                {
                    pooledPrefabInstances.RemoveAt(0);
                }

                if (pooledPrefabInstances.Count > 0)
                {
                    T pooledPrefab = pooledPrefabInstances[0] as T;
                    pooledPrefabInstances.RemoveAt(0);
                    return pooledPrefab;
                }
                else
                {
                    return InstantiateNewPrefab(prefab, instantiationFunction);
                }
            }
            else
            {
                return InstantiateNewPrefab(prefab, instantiationFunction);
            }
        }
        
        private static T InstantiateNewPrefab<T>(T prefab, System.Func<T> instantiationFunction) where T : Object
        {
            GameObject gameObj = prefab is Component component ? component.gameObject : (prefab as GameObject);

            if (instantiationFunction == null)
            {
                return null;
            }

            T g = instantiationFunction();

            if (g != null)
            {
                Instance.prefabByInstanceID.Add(g.GetInstanceID(), prefab);
            }

            return g;
        }
        
        private static void InitializeGameObject<T>(T obj) where T : Object
        {
            GameObject gameObj = obj is Component component ? component.gameObject : (obj as GameObject);
            Object prefabObj = ((T)Instance.prefabByInstanceID[obj.GetInstanceID()]);
            GameObject prefabGameObj =
                prefabObj is Component prefabComp ? prefabComp.gameObject : (prefabObj as GameObject);
            gameObj.transform.localScale = prefabGameObj.transform.localScale;
            gameObj.gameObject.SetActive(true);
        }
        
                /// <summary>
        /// Pools a game object
        /// </summary>
        /// <param name="obj">GameObject to pool</param>
        /// <param name="reset">If true, resets all child IResettables.  If false, IResettables will be unaffected.</param>
        public static void PoolGameObject<T>(T obj, bool reset = false) where T : Object
        {
            if (Instance == null)
            {
                return;
            }
            
            GameObject gameObj = obj is Component component ? component.gameObject : (obj as GameObject);
            int objectInstanceID = obj.GetInstanceID();

            if (!Instance.prefabByInstanceID.ContainsKey(objectInstanceID))
            {
                Destroy(gameObj);
                return;
            }

            T originalPrefab = Instance.prefabByInstanceID[objectInstanceID] as T;

            if (!Instance.pooledGameObjects.ContainsKey(originalPrefab))
            {
                Instance.pooledGameObjects.Add(originalPrefab, new List<Object>());
            }

            if (!Instance.pooledGameObjects[originalPrefab].Contains(obj))
            {
                Instance.pooledGameObjects[originalPrefab].Add(obj);
            }

            if (!gameObj.activeSelf)
            {
                return;
            }
            
            gameObj.SetActive(false);
        }
    }
}