using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ObjectPool : MonoBehaviour
    {
        public List<Pool> pools;
        public Dictionary<Enums.PoolType, Queue<GameObject>> PoolDictionary;

        private void Awake()
        {
            PoolDictionary = new Dictionary<Enums.PoolType, Queue<GameObject>>();
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

        public GameObject SpawnFromPool(Enums.PoolType objectTag)
        {
            if (!PoolDictionary.ContainsKey(objectTag))
                return null;

            var obj = PoolDictionary[objectTag].Dequeue();
            PoolDictionary[objectTag].Enqueue(obj);

            return obj;
        }

        [Serializable]
        public struct Pool
        {
            public Enums.PoolType tag;
            public GameObject prefab;
            public int size;
        }
    }
}