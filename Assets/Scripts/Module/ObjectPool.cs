using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ObjectPool : MonoBehaviour
    {
        [System.Serializable]
        public struct Pool
        {
            public Enum.PoolType tag;
            public GameObject prefab;
            public int size;
        }
        
        public List<Pool> pools;
        public Dictionary<Enum.PoolType, Queue<GameObject>> PoolDictionary;

        private void Awake()
        {
            PoolDictionary = new Dictionary<Enum.PoolType, Queue<GameObject>>();
            foreach (var pool in pools)
            {
                var objectPool = new Queue<GameObject>();
                
                for (var i = 0; i < pool.size; i++)
                {
                    var obj = Instantiate(pool.prefab, transform, true);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                
                PoolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(Enum.PoolType objectTag)
        {
            if (!PoolDictionary.ContainsKey(objectTag))
                return null;

            var obj = PoolDictionary[objectTag].Dequeue();
            PoolDictionary[objectTag].Enqueue(obj);

            return obj;
        }
    }
}