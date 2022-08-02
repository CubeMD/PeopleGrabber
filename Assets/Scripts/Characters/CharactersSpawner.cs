using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Grabber;
using Characters.Walker;
using UnityEngine;
using UnityEngine.AI;
using Utilities;
using Utilities.Extensions;
using Random = UnityEngine.Random;

namespace Characters
{
    public class CharactersSpawner : MonoBehaviour
    {
        public static event Action<CharactersSpawner> OnMinAgentsRemaining;
    
        private const int MAX_RETRY_AMOUNT = 3;

        [SerializeField]
        private BoxCollider envPlane;

        [Header("Agents")]
        [SerializeField]
        private GrabberAgent agentTemplate;
        [SerializeField]
        private Transform agentsParent;
    
        [SerializeField]
        private List<Color> teamColors = new List<Color>();
    
        [SerializeField]
        private int amountFollowersForKill = 10;
        [SerializeField]
        private Vector2 followersSpawnRange = new Vector2(2, 10);

        [Header("Walkers")]
        [SerializeField]
        private NpcWalker npcWalkerTemplate;
        [SerializeField]
        private Transform walkersParent;
        [SerializeField]
        private int maxWalkersAmount = 200;
        [SerializeField]
        private int initialWalkersAmount = 100;
        [SerializeField]
        private float burstSpawnDelay = 100;
        [SerializeField]
        private int burstWalkersAmount = 20;
        
        private float GetRandomWidth => Random.Range(envSize.x, envSize.y);
        private float GetRandomHeight => Random.Range(envSize.z, envSize.w);

        private Vector4 envSize = Vector4.zero;
        private readonly List<Vector3> cachePositions = new List<Vector3>();
        private readonly List<NpcWalker> activeNpcWalkers = new List<NpcWalker>();
        private readonly List<GrabberAgent> activeAgents = new List<GrabberAgent>();
        private Coroutine characterSpawnRoutine;

        private void Awake()
        {
            NpcWalker.OnAnyWalkerBegunAgentFollow += HandleAnyWalkerBegunAgentFollow;
            GrabberAgent.OnAnyAgentDeath += HandleAnyAgentDeath;
            CalculateNavMeshSize();
        }

        private void Start()
        {
            StartSpawnRoutine();
        }

        private void OnDestroy()
        {
            DestroySpawnRoutine();
            NpcWalker.OnAnyWalkerBegunAgentFollow -= HandleAnyWalkerBegunAgentFollow;
            GrabberAgent.OnAnyAgentDeath -= HandleAnyAgentDeath;
        }

        private void CalculateNavMeshSize()
        {
            Vector3 size = envPlane.bounds.size;
            Vector3 center = envPlane.bounds.center;

            envSize.x = center.x - size.x / 2;
            envSize.y = center.x + size.x / 2;
            envSize.z = center.z - size.z / 2;
            envSize.w = center.z + size.z / 2;
        }

        private void StartSpawnRoutine()
        {
            DestroySpawnRoutine();
            characterSpawnRoutine = StartCoroutine(CharacterSpawnRoutine());
        }

        private IEnumerator CharacterSpawnRoutine()
        {
            for (int i = 0; i < initialWalkersAmount; i++)
            {
                SpawnNpcAtRandomPosition();
            }
        
            cachePositions.Clear();
            for (int i = 0; i < teamColors.Count; i++)
            {
                while (true)
                {
                    Vector3 randomPosition = new Vector3(GetRandomWidth, 0, GetRandomHeight);

                    if (NavMeshExtension.TryToGetNavMeshPosition(randomPosition, out Vector3 spawnPosition))
                    {
                        cachePositions.Add(spawnPosition);
                        break;
                    }
                }
            }

            yield return null;
            SpawnAgents();

            while (true)
            {
                int maxSpawn = Mathf.Min(burstWalkersAmount, maxWalkersAmount - activeNpcWalkers.Count);
                
                for (int i = 0; i < maxSpawn; i++)
                {
                    SpawnNpcAtRandomPosition();
                }
                
                yield return new WaitForSeconds(burstSpawnDelay);
            }
        } 
    
        private void SpawnAgents()
        {
            if (agentTemplate == null)
            {
                return;
            }

            for (int i = 0; i < cachePositions.Count; i++)
            {
                Vector3 position = cachePositions[i];
                Quaternion rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                GrabberAgent agent = ObjectPooler.Instantiate(agentTemplate, position, rotation, agentsParent);
                Team team = new Team(agent, teamColors[i]);
                agent.SetTeam(team);
                activeAgents.Add(agent);
            }
        }

        private void SpawnNpcAtRandomPosition()
        {
            for (int i = 0; i < MAX_RETRY_AMOUNT; i++)
            {
                Vector3 randomPosition = new Vector3(GetRandomWidth, 0, GetRandomHeight);
                if (TryToSpawnNpcNearPosition(randomPosition, out NpcWalker npc))
                {
                    activeNpcWalkers.Add(npc);
                    return;
                }
            }
            
            Debug.LogWarning($"Wasn't able to find any position on navmesh after {MAX_RETRY_AMOUNT} retries");
        }
    
        private void SpawnFollowersForAgent(Team team)
        {
            for (int i = 0; i < amountFollowersForKill; i++)
            {
                for (int j = 0; j < MAX_RETRY_AMOUNT; j++)
                {
                    Vector3 randomPosition = team.Agent.transform.position + Random.insideUnitSphere * Random.Range(followersSpawnRange.x, followersSpawnRange.y);
                    if (TryToSpawnNpcNearPosition(randomPosition, out NpcWalker npc))
                    {
                        npc.ForceTeam(team);
                        break;
                    }
                }
            }
        }
    
        private bool TryToSpawnNpcNearPosition(Vector3 position, out NpcWalker npc)
        {
            if (NavMeshExtension.TryToGetNavMeshPosition(position, out Vector3 spawnPosition))
            {
                npc = ObjectPooler.Instantiate(npcWalkerTemplate, spawnPosition, Quaternion.identity, walkersParent);
                return true;
            }

            npc = null;
            return false;
        }
    
        private void HandleAnyWalkerBegunAgentFollow(NpcWalker npcWalker)
        {
            if (activeNpcWalkers.Contains(npcWalker))
            {
                activeNpcWalkers.Remove(npcWalker);
            }
        }
        
        private void HandleAnyAgentDeath(GrabberAgent killedAgent, Team killerTeam)
        {
            if (activeAgents.Contains(killedAgent))
            {
                activeAgents.Remove(killedAgent);
                SpawnFollowersForAgent(killerTeam);
            
                if (activeAgents.Count < 2)
                {
                    OnMinAgentsRemaining?.Invoke(this);
                }
            }
        }

        private void DestroySpawnRoutine()
        {
            if (characterSpawnRoutine != null)
            {
                StopCoroutine(characterSpawnRoutine);
            }
        }

        private void OnDrawGizmos()
        {
            /*for (int i = 0; i < cachePositions.Count; i++)
            {
                Vector3 position = cachePositions[i];
                Gizmos.color = teamColors[i];
                Gizmos.DrawSphere(position, 1);
            }*/
        }
    }
}