using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        public GameObject player;

        void Update()
        {
            transform.position = new Vector3(player.transform.position.x , 30, player.transform.position.z -20);    
        }
    }
}
