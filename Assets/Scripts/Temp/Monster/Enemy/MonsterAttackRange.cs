// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace Enemy
// {
//     public class MonsterAttackRange : MonoBehaviour
//     {
//         [FormerlySerializedAs("myMonsterTemp")] [SerializeField] Monster myMonster;
//
//         private void OnTriggerEnter2D(Collider2D collision)
//         {
//             if (collision.CompareTag("Player"))
//             {
//                 myMonster.InAttackRangeEvent?.Invoke(collision);
//             }
//         }
//
//         private void OnTriggerExit2D(Collider2D collision)
//         {
//             if (collision.CompareTag("Player"))
//             {
//                 myMonster.FSM.ChangeState(MonsterState.Idle);
//             }
//         }
//     }
// }