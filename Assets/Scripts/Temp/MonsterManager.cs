// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class MonsterManager : MonoBehaviour
// {
//     [SerializeField] private Monster monsterPrefab;
//     [SerializeField] private int poolSize = 10;
//
//     private Queue<GameObject> monsterPool = new Queue<GameObject>();
//
//     [SerializeField] Transform target;
//
//     public List<GameObject> activeMonsters = new List<GameObject>();
//
//     private void Awake()
//     {
//         InitializeMonsterPool();
//     }
//
//     private void Start()
//     {
//         SpawnMonster();
//     }
//
//     void InitializeMonsterPool()
//     {
//         for (int i = 0; i < poolSize; i++)
//         {
//             GameObject monster = Instantiate(monsterPrefab.gameObject);
//
//             monster.GetComponent<MonsterControler>().SetClosestPlayer(target);
//
//             monster.SetActive(false);
//             monsterPool.Enqueue(monster);
//         }
//     }
//
//     public GameObject GetMonster()
//     {
//         if (monsterPool.Count > 0)
//         {
//             GameObject monster = monsterPool.Dequeue();
//             monster.SetActive(true);
//             activeMonsters.Add(monster);
//             return monster;
//         }
//         else
//         {
//             GameObject newMonster = Instantiate(monsterPrefab.gameObject);
//             activeMonsters.Add(newMonster);
//             return newMonster;
//         }
//     }
//
//     public void ReturnMonster(GameObject monster)
//     {
//         monster.SetActive(false);
//         activeMonsters.Remove(monster);
//         monsterPool.Enqueue(monster);
//     }
//
//     public void SpawnMonster()
//     {
//         GetMonster().transform.position = new Vector2(2, 4);
//         GetMonster().transform.position = new Vector2(-2, 4);
//         GetMonster().transform.position = new Vector2(2, -4);
//         GetMonster().transform.position = new Vector2(-2, -4);
//     }
// }
