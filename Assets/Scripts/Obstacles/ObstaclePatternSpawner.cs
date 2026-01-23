using UnityEngine;
using ShiftRunner3D.Utils;

namespace ShiftRunner3D.Obstacles
{
    public class ObstaclePatternSpawner : MonoBehaviour
    {
        [Header("References")]
        public ObjectPool obstaclePool;
        public Transform playerTransform; // ADD THIS - assign in inspector
        
        [Header("Spawn Settings")]
        public float spawnDistance = 120f; // Distance ahead of player to spawn (increased to hide spawning)        public float spawnInterval = 2.5f;
                public float spawnInterval = 3.0f; // Time between pattern spawns
        public float laneDistance = 2.5f;
        public float spawnHeight = 0.5f;
        public float despawnDistance = 20f; // Distance behind player to despawn
        
        [Header("Patterns")]
        public ObstaclePattern[] patterns;
        
        [Header("Difficulty Progression")]
        public bool enableDifficultyProgression = true;
        public float intervalReduction = 0.1f;
        public float minimumInterval = 1.0f;
        
        private float timer;
        private float currentInterval;
        private float gameTime;
        private int lastPatternIndex = -1;
        
        void Start()
        {
            currentInterval = spawnInterval;
            
            // Find player if not assigned
            if (playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerTransform = player.transform;
                }
                else
                {
                    Debug.LogError("ObstaclePatternSpawner: Player not found!");
                    enabled = false;
                    return;
                }
            }
            
            if (patterns == null || patterns.Length == 0)
            {
                Debug.LogError("ObstaclePatternSpawner: No patterns defined!");
                enabled = false;
                return;
            }
            
            if (obstaclePool == null)
            {
                Debug.LogError("ObstaclePatternSpawner: Object pool not assigned!");
                enabled = false;
                return;
            }
        }
        
        void Update()
        {
            if (playerTransform == null) return;
            
            timer += Time.deltaTime;
            
            // Difficulty progression
            if (enableDifficultyProgression)
            {
                gameTime += Time.deltaTime;
                
                if (gameTime >= 10f)
                {
                    currentInterval = Mathf.Max(minimumInterval, currentInterval - intervalReduction);
                    gameTime = 0f;
                }
            }
            
            // Spawn patterns at regular intervals
            if (timer >= currentInterval)
            {
                SpawnPattern();
                timer = 0f;
            }
        }
        
        void SpawnPattern()
        {
            if (patterns.Length == 0) return;
            
            int patternIndex = SelectRandomPattern();
            ObstaclePattern pattern = patterns[patternIndex];
            
            if (!pattern.IsValid()) return;
            
            // Spawn ahead of player
            float patternStartZ = playerTransform.position.z + spawnDistance;
            
            foreach (int lane in pattern.lanes)
            {
                SpawnObstacleInLane(lane, patternStartZ);
                patternStartZ += pattern.internalSpacing;
            }
            
            lastPatternIndex = patternIndex;
        }
        
        int SelectRandomPattern()
        {
            float totalWeight = 0f;
            foreach (var pattern in patterns)
            {
                if (pattern.IsValid())
                {
                    totalWeight += pattern.spawnWeight;
                }
            }
            
            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;
            
            for (int i = 0; i < patterns.Length; i++)
            {
                if (!patterns[i].IsValid()) continue;
                
                currentWeight += patterns[i].spawnWeight;
                if (randomValue <= currentWeight)
                {
                    // Avoid repeating same pattern
                    if (i == lastPatternIndex && patterns.Length > 1)
                    {
                        return (i + 1) % patterns.Length;
                    }
                    return i;
                }
            }
            
            return 0;
        }
        
        void SpawnObstacleInLane(int lane, float zPosition)
        {
            // Lane mapping: 0=left, 1=center, 2=right
            float xPos = (lane - 1) * laneDistance;
            Vector3 spawnPos = new Vector3(xPos, spawnHeight, zPosition);
            
            GameObject obstacle = obstaclePool.GetPooledObject();
            
            if (obstacle != null)
            {
                obstacle.transform.position = spawnPos;
                obstacle.transform.rotation = Quaternion.identity;
                obstacle.SetActive(true);
            }
        }
    }
}
