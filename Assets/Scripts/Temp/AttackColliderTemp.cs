// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class AttackCollider : MonoBehaviour
// {
//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (!collision.CompareTag(Strings.TAG_MONSTER)) return;
//
//         if (collision.GetComponent<Monster>().TakeDamage(100))
//         {
//             Debug.Log("ReSet!");
//             PlayerControler.isKilled?.Invoke();
//         }
//
//         if (gameObject.tag == "RangedAttack") gameObject.SetActive(false);
//     }
//
//     //private void OnTriggerExit2D(Collider2D collision)
//     //{
//     //    if (!collision.CompareTag(Strings.TAG_MONSTER)) return;
//
//     //    playerControler.ResetClosestMonster();
//
//     //}
// }
