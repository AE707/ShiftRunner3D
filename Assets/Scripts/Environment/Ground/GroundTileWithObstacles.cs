using UnityEngine;

namespace ShiftRunner3D.Environment
{
    /// <summary>
    /// Optional component to attach pre-placed obstacles to ground tiles.
    /// This allows obstacles to be "part of the road" and spawn naturally with the tile.
    /// </summary>
    public class GroundTileWithObstacles : MonoBehaviour
    {
        [Header("Tile Obstacles")]
        [Tooltip("Obstacles that are children of this tile (optional)")]
        public GameObject[] obstacles;
        
        [Header("Debug")]
        public bool showDebugInfo = false;
        
        void Start()
        {
            // Find all obstacles if not manually assigned
            if (obstacles == null || obstacles.Length == 0)
            {
                // Auto-detect obstacles by tag or by checking children
                FindObstaclesInChildren();
            }
            
            if (showDebugInfo)
            {
                Debug.Log($"GroundTileWithObstacles: Found {obstacles.Length} obstacles on {gameObject.name}");
            }
        }
        
        void FindObstaclesInChildren()
        {
            // Find all children with "Obstacle" tag or in Obstacles layer
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            System.Collections.Generic.List<GameObject> foundObstacles = new System.Collections.Generic.List<GameObject>();
            
            foreach (Transform child in allChildren)
            {
                if (child != transform && (child.CompareTag("Obstacle") || child.gameObject.layer == LayerMask.NameToLayer("Obstacles")))
                {
                    foundObstacles.Add(child.gameObject);
                }
            }
            
            obstacles = foundObstacles.ToArray();
        }
        
        /// <summary>
        /// Enable or disable all obstacles on this tile
        /// </summary>
        public void SetObstaclesActive(bool active)
        {
            if (obstacles != null)
            {
                foreach (GameObject obstacle in obstacles)
                {
                    if (obstacle != null)
                    {
                        obstacle.SetActive(active);
                    }
                }
            }
        }
    }
}
