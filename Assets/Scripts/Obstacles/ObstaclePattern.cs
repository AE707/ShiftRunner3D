using UnityEngine;

namespace ShiftRunner3D.Obstacles
{
    [System.Serializable]
    public class ObstaclePattern
    {
        [Header("Pattern Info")]
        public string patternName = "New Pattern";
        
        [Header("Lane Configuration")]
        public int[] lanes = new int[] { 0 };
        
        [Range(1, 5)]
        public int difficultyLevel = 1;
        
        [Header("Advanced Settings")]
        public float internalSpacing = 0f;
        
        [Range(0.1f, 10f)]
        public float spawnWeight = 1f;
        
        public bool IsValid()
        {
            if (lanes == null || lanes.Length == 0)
            {
                Debug.LogWarning($"ObstaclePattern '{patternName}': No lanes defined!");
                return false;
            }
            
            foreach (int lane in lanes)
            {
                if (lane < 0 || lane > 2)
                {
                    Debug.LogWarning($"ObstaclePattern '{patternName}': Invalid lane {lane}");
                    return false;
                }
            }
            
            return true;
        }
        
        public int GetObstacleCount()
        {
            return lanes != null ? lanes.Length : 0;
        }
    }
}
