using UnityEngine;
using System.Collections.Generic;

namespace ShiftRunner3D.Environment
{
    public class GroundTileSpawner : MonoBehaviour
    {
        [Header("Ground Tile Settings")]
        public GameObject groundTilePrefab;
        public int visibleTiles = 8;
        public float tileLength = 20f;
        
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
            spawnZ = startZ;
            
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
            if (groundTilePrefab == null) return;
            
            Vector3 spawnPosition = new Vector3(0, 0, spawnZ);
            GameObject tile = Instantiate(groundTilePrefab, spawnPosition, Quaternion.identity, transform);
            tile.name = $"GroundTile_{lastSpawnedIndex}";
            
            activeTiles.Add(tile);
            spawnZ += tileLength;
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
