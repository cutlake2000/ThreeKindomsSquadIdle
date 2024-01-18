// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class AttackRange : MonoBehaviour
// {
//     [SerializeField] Character character;
//
//     [SerializeField] int enemyCount = 0;
//
//     private void Awake()
//     {
//         character = transform.parent.GetComponent<Character>();
//     }
//
//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         Debug.Log( "한번 보자잉 "+ collision.tag + " " + character.EnemyTag);
//         if (collision.CompareTag(character.EnemyTag))
//         {
//             enemyCount++;
//             character.isAttacking = enemyCount == 0 ? false : true;
//         }
//     }
//
//     private void OnTriggerExit2D(Collider2D collision)
//     {
//         if (collision.CompareTag(character.EnemyTag))
//         {
//             enemyCount--;
//             character.isAttacking = enemyCount == 0 ? false : true;
//         }
//     }
// }
