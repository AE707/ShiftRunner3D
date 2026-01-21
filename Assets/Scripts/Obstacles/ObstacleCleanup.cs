using UnityEngine;
using ShiftRunner3D.Utils;

namespace ShiftRunner3D.Obstacles
{
    public class ObstacleCleanup : MonoBehaviour
    {
        private Transform player;
        private ObjectPool obstaclePool;
        
        void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("ObstacleCleanup: Player not found!");
            }
            
            obstaclePool = Object.FindFirstObjectByType<ObjectPool>();
            if (obstaclePool == null)
            {
                Debug.LogWarning("ObstacleCleanup: ObjectPool not found! Will use Destroy instead.");
            }
        }
        
        void Update()
        {
            if (player == null) return;
            
            if (transform.position.z < player.position.z - 10f)
            {
                if (obstaclePool != null)
                {
                    obstaclePool.ReturnToPool(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
