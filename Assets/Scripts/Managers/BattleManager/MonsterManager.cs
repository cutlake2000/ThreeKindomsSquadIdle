using System;
using System.Collections.Generic;
using System.Linq;
using Creature.CreatureClass.MonsterClass;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.BattleManager
{
    public class MonsterManager : MonoBehaviour
    {
        public static MonsterManager Instance;

        public GameObject[] monsterNewTypePrefabs;
        public BoxCollider2D[] spawnPosition;
        [SerializeField] private List<NormalMonster> activeMonsters = new();

        public int monsterNewCount = 20;

        private readonly Dictionary<string, float> animationLengths = new();
        private readonly int[] spawnXs = { -10, 10, 0 };
        private Dictionary<Enums.MonsterClassType, Queue<NormalMonster>> monsterNewPools;
        private int spawnCount;

        private void Awake()
        {
            Instance = this;

            InitializeMonsterPools();
        }

        private void InitializeMonsterPools()
        {
            monsterNewPools = new Dictionary<Enums.MonsterClassType, Queue<NormalMonster>>();
            foreach (Enums.MonsterClassType type in System.Enum.GetValues(typeof(Enums.MonsterClassType)))
            {
                var pool = new Queue<NormalMonster>();
                var prefab = monsterNewTypePrefabs[(int)type];
                FillMonsterPool(prefab, pool);
                monsterNewPools[type] = pool;
            }
        }

        private void FillMonsterPool(GameObject prefab, Queue<NormalMonster> pool)
        {
            for (var i = 0; i < monsterNewCount; i++)
            {
                var newMonsterObj = Instantiate(prefab);
                var monsterNew = newMonsterObj.GetComponent<NormalMonster>();
                monsterNew.gameObject.SetActive(false);
                pool.Enqueue(monsterNew);
            }
        }

        public void SpawnMonsters(Enums.MonsterClassType[] monsterNewTypes, int totalCount)
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

                SpawnMonsters(monsterNewTypes[i], countForType);
            }

            spawnCount = (spawnCount + 1) % spawnXs.Length;
        }

        public void SpawnMonsters(Enums.MonsterClassType monsterNewClassType, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var monsterNew = GetMonster(monsterNewClassType);
                PositionMonster(monsterNew);
            }
        }

        private NormalMonster GetMonster(Enums.MonsterClassType classType)
        {
            NormalMonster normalMonster;
            if (monsterNewPools[classType].Count > 0)
            {
                normalMonster = monsterNewPools[classType].Dequeue();
            }
            else
            {
                var newMonsterObj = Instantiate(monsterNewTypePrefabs[(int)classType]);
                normalMonster = newMonsterObj.GetComponent<NormalMonster>();
            }

            activeMonsters.Add(normalMonster);
            return normalMonster;
        }

        public void DespawnAllMonster()
        {
            if (activeMonsters.Count == 0) return;
            foreach (var monster in activeMonsters.ToList()) ReturnMonster(monster.monsterClassType, monster);
        }

        public void ReturnMonster(Enums.MonsterClassType classType, NormalMonster normalMonster)
        {
            normalMonster.gameObject.SetActive(false);
            monsterNewPools[classType].Enqueue(normalMonster);
            activeMonsters.Remove(normalMonster);
        }

        private void PositionMonster(Component monsterNew)
        {
            // var t = count / (float)numberOfMonster;
            // var position = CalculateBezierPoint(t, startPoint[spawnCount].position, controlPoint[spawnCount].position,
            //     endPoint[spawnCount].position);

            var position = CalculateRandomPosition();

            monsterNew.transform.position = position;
            monsterNew.gameObject.SetActive(true);
        }

        /// <summary>
        ///     베지어 곡선 공식으로 사용한 곡선 스포닝
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

        private Vector3 CalculateRandomPosition()
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