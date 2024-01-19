using System.Collections.Generic;
using Creature.MonsterScripts.MonsterClass;
using UnityEngine;
using Enum = Data.Enum;
using Random = UnityEngine.Random;

namespace Managers
{
    public class MonsterManager : MonoBehaviour
    {
        public static MonsterManager Instance;

        public GameObject[] monsterNewTypePrefabs;
        public BoxCollider2D[] spawnPosition;
        public Transform[] startPoint;
        public Transform[] controlPoint;
        public Transform[] endPoint;
        [SerializeField] private List<MonsterNew> activeMonsters = new();

        private readonly Dictionary<string, float> animationLengths = new();
        private Dictionary<Enum.MonsterClassType, Queue<MonsterNew>> monsterNewPools;

        public int monsterNewCount = 20;
        private readonly int[] spawnXs = { -10, 10, 0 };
        private int spawnCount;

        private void Awake()
        {
            Instance = this;
            
            InitializeMonsterPools();
        }
        private void InitializeMonsterPools()
        {
            monsterNewPools = new Dictionary<Enum.MonsterClassType, Queue<MonsterNew>>();
            foreach (Enum.MonsterClassType type in System.Enum.GetValues(typeof(Enum.MonsterClassType)))
            {
                var pool = new Queue<MonsterNew>();
                var prefab = monsterNewTypePrefabs[(int)type];
                FillMonsterPool(prefab, pool);
                monsterNewPools[type] = pool;
            }
        }

        private void FillMonsterPool(GameObject prefab, Queue<MonsterNew> pool)
        {
            for (var i = 0; i < monsterNewCount; i++)
            {
                var newMonsterObj = Instantiate(prefab);
                var monsterNew = newMonsterObj.GetComponent<MonsterNew>();
                monsterNew.gameObject.SetActive(false);
                pool.Enqueue(monsterNew);
            }
        }

        public void GenerateMonsters(Enum.MonsterClassType[] monsterNewTypes, int totalCount)
        {
            var remainingCount = totalCount;
            var typesCount = monsterNewTypes.Length;

            for (var i = 0; i < typesCount; i++)
            {
                int countForType;

                if (i == typesCount - 1)
                {
                    // 마지막 타입에 대해서는 남은 수를 모두 할당
                    countForType = remainingCount;
                }
                else
                {
                    // 남은 수 중에서 랜덤하게 할당 (최소 1개 이상)
                    countForType = Random.Range(1, remainingCount - (typesCount - i - 1));
                    remainingCount -= countForType;
                }

                GenerateMonsters(monsterNewTypes[i], countForType);
            }

            spawnCount = (spawnCount + 1) % spawnXs.Length;
        }


        public void GenerateMonsters(Enum.MonsterClassType monsterNewClassType, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var monsterNew = GetMonster(monsterNewClassType);
                PositionMonster(monsterNew, i, count);
            }
        }

        private MonsterNew GetMonster(Enum.MonsterClassType classType)
        {
            MonsterNew monsterNew;
            if (monsterNewPools[classType].Count > 0)
            {
                monsterNew = monsterNewPools[classType].Dequeue();
            }
            else
            {
                var newMonsterObj = Instantiate(monsterNewTypePrefabs[(int)classType]);
                monsterNew = newMonsterObj.GetComponent<MonsterNew>();
            }

            activeMonsters.Add(monsterNew);
            return monsterNew;
        }

        public void ReturnMonster(Enum.MonsterClassType classType, MonsterNew monsterNew)
        {
            monsterNew.gameObject.SetActive(false);
            monsterNewPools[classType].Enqueue(monsterNew);
            activeMonsters.Remove(monsterNew);
        }

        private void PositionMonster(Component monsterNew, int count, int numberOfMonster)
        {
            // var t = count / (float)numberOfMonster;
            // var position = CalculateBezierPoint(t, startPoint[spawnCount].position, controlPoint[spawnCount].position,
            //     endPoint[spawnCount].position);

            var position = CalculateRandomPosition(spawnCount);

            monsterNew.transform.position = position;
            monsterNew.gameObject.SetActive(true);
        }

        /// <summary>
        /// 베지어 곡선 공식으로 사용한 곡선 스포닝
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            // 베지어 곡선 공식
            return (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
        }

        private Vector3 CalculateRandomPosition(int spawnCount)
        {
            var rangeX = spawnPosition[spawnCount].bounds.size.x;
            var rangeY = spawnPosition[spawnCount].bounds.size.y;
            var rangeZ = spawnPosition[spawnCount].bounds.size.z;
            
            rangeX = spawnPosition[spawnCount].transform.position.x + Random.Range(-rangeX / 2f, rangeX / 2f);
            rangeY = spawnPosition[spawnCount].transform.position.y + Random.Range(-rangeY / 2f, rangeY / 2f);
            rangeZ = spawnPosition[spawnCount].transform.position.z + Random.Range(-rangeZ / 2f, rangeZ / 2f);

            return spawnPosition[spawnCount].transform.position + new Vector3(rangeX, rangeY, rangeZ);
        }
    }
}