using UnityEngine;

namespace ShiftRunner3D.Environment
{
    public class GroundTile : MonoBehaviour
    {
        [Header("Tile Info")]
        public string tileType = "Standard";
        
        [Header("Optional Features")]
        public bool canSpawnCollectibles = true;
        public bool canSpawnDecorations = true;
        
        private float tileLength;
        
        void Awake()
        {
            if (TryGetComponent<Renderer>(out Renderer renderer))
            {
                tileLength = renderer.bounds.size.z;
            }
            else
            {
                tileLength = 20f;
            }
        }
        
        public float GetTileLength()
        {
            return tileLength;
        }
    }
}
