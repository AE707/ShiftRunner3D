using UnityEngine;
using System.Collections.Generic;

namespace ShiftRunner3D.Environment
{
    public class GroundTileSpawner : MonoBehaviour
    {
        [Header("Ground Tile Settings")]
        public GameObject[] tilePrefabs; // Multiple tile variants (empty, with obstacles, etc.)
        public int visibleTiles = 80;
        public float tileLength = 20f;
        public float tileOverlap = 0.1f; // Small overlap to prevent gaps between tiles

        [Header("Spawn Settings")]
        public float startZ = -40f;

        [Header("Difficulty Settings")]
        [Tooltip("Indices of easy tiles (RoadEmpty, etc.) in tilePrefabs array")]
        public int[] easyTileIndices = new int[] { 0, 1 }; // Default: first 2 tiles are easy
        [Tooltip("Indices of hard tiles (RoadTurretB, RoadAllBlocked, etc.) in tilePrefabs array")]
        public int[] hardTileIndices = new int[] { 2, 3, 4, 5, 6, 7 }; // Default: rest are hard
        [Range(0f, 1f)]
        public float initialHardTileChance = 0.3f; // 30% hard tiles at start
        [Range(0f, 1f)]
        public float maxHardTileChance = 0.8f; // 80% hard tiles at max difficulty

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
            spawnZ = startZ; // Use configured start position

            // Spawn initial tiles
            for (int i = 0; i < visibleTiles; i++)
            {
                SpawnTile();
            }
        }

        void Update()
        {
            if (playerTransform == null) return;

            // Spawn new tiles as player moves forward
            if (playerTransform.position.z > spawnZ - (visibleTiles * tileLength))
            {
                SpawnTile();
                // DeleteOldTile(); // Commented out to prevent memory issues
            }
        }

        void SpawnTile()
        {
            if (tilePrefabs == null || tilePrefabs.Length == 0) return;

            // Get current difficulty from GameManager (0 = easy, 1 = hard)
            float difficulty = 0f;
            if (GameManager.Instance != null)
            {
                difficulty = GameManager.Instance.GetDifficulty();
            }

            // Calculate hard tile chance based on difficulty
            float hardTileChance = Mathf.Lerp(initialHardTileChance, maxHardTileChance, difficulty);

            // Select tile based on difficulty
            GameObject selectedPrefab = SelectTilePrefab(hardTileChance);

            // Spawn tile
            Vector3 spawnPosition = new Vector3(0, 0, spawnZ); // Ensure Y=0 for ground level
            GameObject tile = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity, transform);
            tile.name = $"GroundTile_{lastSpawnedIndex}";

            activeTiles.Add(tile);
            spawnZ += tileLength - tileOverlap; // Overlap to prevent gaps
            lastSpawnedIndex++;
        }

        GameObject SelectTilePrefab(float hardTileChance)
        {
            // Decide if we spawn easy or hard tile
            bool spawnHardTile = Random.value < hardTileChance;

            // Select from appropriate pool
            if (spawnHardTile && hardTileIndices.Length > 0)
            {
                int randomIndex = hardTileIndices[Random.Range(0, hardTileIndices.Length)];
                if (randomIndex >= 0 && randomIndex < tilePrefabs.Length)
                {
                    return tilePrefabs[randomIndex];
                }
            }

            // Default to easy tile
            if (easyTileIndices.Length > 0)
            {
                int randomIndex = easyTileIndices[Random.Range(0, easyTileIndices.Length)];
                if (randomIndex >= 0 && randomIndex < tilePrefabs.Length)
                {
                    return tilePrefabs[randomIndex];
                }
            }

            // Fallback: random tile
            return tilePrefabs[Random.Range(0, tilePrefabs.Length)];
        }

        /* Tile deletion commented out to prevent issues
        void DeleteOldTile()
        {
            if (activeTiles.Count > visibleTiles && activeTiles[0] != null)
            {
                GameObject oldTile = activeTiles[0];
                activeTiles.RemoveAt(0);
                Destroy(oldTile);
            }
        }
        */
    }
}
