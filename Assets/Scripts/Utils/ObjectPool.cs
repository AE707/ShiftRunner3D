using UnityEngine;
using System.Collections.Generic;

namespace ShiftRunner3D.Utils
{
    public class ObjectPool : MonoBehaviour
    {
        [Header("Pool Settings")]
        public GameObject prefab;
        public int initialPoolSize = 20;
        public bool canGrow = true;
        public int maxPoolSize = 0;
        
        private List<GameObject> pool = new List<GameObject>();
        private Transform poolParent;
        public GameObject[] prefabVariations;
        
        void Awake()
        {
            InitializePool();
        }
        
        void InitializePool()
        {

            if (prefabVariations == null || prefabVariations.Length == 0)
            {
                // Fallback to single prefab
                if (prefab == null) {
                Debug.LogError($"ObjectPool: Prefab not assigned!");
                return;
            }
                 prefabVariations = new GameObject[] { prefab };
            }

            //poolParent = new GameObject($"{prefab.name}_Pool").transform;
            //poolParent.SetParent(transform);
            
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject randomPrefab = prefabVariations[Random.Range(0, prefabVariations.Length)];
                GameObject obj = Instantiate(randomPrefab);
                obj.SetActive(false);
                obj.transform.parent = poolParent;
                pool.Add(obj);
            }
        }
        
        GameObject CreatePooledObject()
        {
            GameObject obj = Instantiate(prefab, poolParent);
            obj.SetActive(false);
            obj.name = $"{prefab.name}_{pool.Count}";
            pool.Add(obj);
            return obj;
        }
        
        public GameObject GetPooledObject()
        {
            foreach (GameObject obj in pool)
            {
                if (obj != null && !obj.activeInHierarchy)
                {
                    return obj;
                }
            }
            
            if (canGrow && (maxPoolSize == 0 || pool.Count < maxPoolSize))
            {
                return CreatePooledObject();
            }
            
            return null;
        }
        
        public void ReturnToPool(GameObject obj)
        {
            if (obj == null) return;
            
            obj.SetActive(false);
            obj.transform.SetParent(poolParent);
        }
        
        public void ReturnAllToPool()
        {
            foreach (GameObject obj in pool)
            {
                if (obj != null && obj.activeInHierarchy)
                {
                    ReturnToPool(obj);
                }
            }
        }
    }
}
