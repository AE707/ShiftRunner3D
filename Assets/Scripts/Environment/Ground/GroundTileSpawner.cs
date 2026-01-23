using UnityEngine;
using System.Collections.Generic;

namespace ShiftRunner3D.Environment
{
    public class GroundTileSpawner : MonoBehaviour
    {
        [Header("Ground Tile Settings")]
        public GameObject[] tilePrefabs; // Multiple tile variants (empty, with obstacles, etc.)        pu
                [HideInInspector] public GameObject groundTilePrefab; // Legacy field for backward compatibility
                public int visibleTiles = 8;
        public float tileLength = 20f;
                public float tileOverlap = 0.1f; // Small overlap to prevent gaps between tiles
        
        [Header("Spawn Settings")]
        public float startZ = -20f;
        
        private List<GameObject> activeTiles = new List<GameObject>();
        private Transform playerTransform;
        private float spawnZ;
        private int lastSpawnedIndex = 0;
        
        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("GroundTileSpawner: Player not found!");
                enabled = false;
                return;
            }
            
            playerTransform = player.transform;
                    spawnZ = playerTransform.position.z - tileLength; // Start one tile behind player
            
            for (int i = 0; i < visibleTiles; i++)
            {
                SpawnTile();
            }
        }
        
        void Update()
        {
            if (playerTransform == null) return;
            
            if (playerTransform.position.z > spawnZ - (visibleTiles * tileLength))
            {
                SpawnTile();
                DeleteOldTile();
            }
        }
        
        void SpawnTile()
        {
        if (tilePrefabs == null || tilePrefabs.Length == 0) return;            
                
        // Randomly select a tile variant
        GameObject selectedPrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
            Vector3 spawnPosition = new Vector3(0, 0, spawnZ); // Ensure Y=0 for ground level
        GameObject tile = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity, transform);            tile.name = $"GroundTile_{lastSpawnedIndex}";
            
            activeTiles.Add(tile);
                    spawnZ += (tileLength - tileOverlap); // Overlap to prevent gaps    lastSpawnedIndex++;
                            lastSpawnedIndex++;
        }
        
        void DeleteOldTile()
        {
            if (activeTiles.Count > visibleTiles && activeTiles[0] != null)
            {
                GameObject oldTile = activeTiles[0];
                activeTiles.RemoveAt(0);
                Destroy(oldTile);
            }
        }
    }
}
