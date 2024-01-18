// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
//
// public class Player : Character
// {
//     [SerializeField] private bool isMeleeAttack = false;
//     [SerializeField] GameObject meleeAttackRange;
//     [SerializeField] GameObject RangedAttackRange;
//
//     [SerializeField] GameObject meleeCollider;
//
//     public void ChangeAttack()
//     {
//         isMeleeAttack = !isMeleeAttack;
//
//         meleeAttackRange.SetActive(isMeleeAttack);
//         meleeCollider.SetActive(isMeleeAttack);
//
//         RangedAttackRange.SetActive(!isMeleeAttack);
//     }
//
//     public bool GetAttack()
//     {
//         return isMeleeAttack;
//     }
// }
