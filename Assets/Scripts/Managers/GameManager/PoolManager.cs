using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PoolManager : MonoBehaviour
    {
        public GameObject[] prefabs; // 인스펙터에서 초기화
        private List<GameObject>[] pools;

        private void Awake()
        {
            pools = new List<GameObject>[prefabs.Length];

            for (var index = 0; index < pools.Length; index++)
                pools[index] = new List<GameObject>();
        }

        public GameObject Get(int index)
        {
            GameObject select = null;

            foreach (var item in pools[index])
                if (!item.activeSelf)
                {
                    select = item;
                    select.SetActive(true);
                    break;
                }

            if (!select)
            {
                select = Instantiate(prefabs[index], transform);
                pools[index].Add(select);
            }

            return select;
        }

        public void Clear(int index)
        {
            foreach (var item in pools[index])
                item.SetActive(false);
        }

        public void ClearAll()
        {
            for (var index = 0; index < pools.Length; index++)
                foreach (var item in pools[index])
                    item.SetActive(false);
        }
    }
}