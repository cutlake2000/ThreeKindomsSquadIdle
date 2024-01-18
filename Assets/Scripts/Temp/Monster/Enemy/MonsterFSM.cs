// using UnityEngine;
//
// namespace Enemy
// {
//     public class MonsterFSM : MonoBehaviour
//     {
//         public MonsterState currentState = MonsterState.Idle;
//         public float detectionRange = 50.0f; // 플레이어 감지 범위
//         public Transform playerTransform; // 플레이어의 Transform
//
//         [SerializeField] Animator myAnimator;
//
//         private float attackRange = 0.2f; // 공격 범위
//         private float timer = 0.0f; // 상태 지속 시간을 추적하는 타이머
//
//         void Update()
//         {
//             if (playerTransform == null) return;
//             switch (currentState)
//             {
//                 case MonsterState.Idle:
//                     Idle();
//                     break;
//                 case MonsterState.Walk:
//                     Walk();
//                     break;
//                 case MonsterState.Run:
//                     Run();
//                     break;
//                 case MonsterState.ChopAttack:
//                     ChopAttack();
//                     break;
//                 case MonsterState.ThrustAttack:
//                     ThrustAttack();
//                     break;
//             }
//
//             // 상태 전환 로직
//             TransitionStates();
//         }
//
//         void Idle()
//         {
//             // Idle 상태일 때의 로직
//             myAnimator.SetTrigger("Idle");
//             // 예: 몬스터가 주변을 둘러보거나 잠시 쉬는 애니메이션 재생
//         }
//
//         void Walk()
//         {
//             // Walk 상태일 때의 로직
//             myAnimator.SetTrigger("Walk");
//             // 예: 느린 속도로 이동
//         }
//
//         void Run()
//         {
//             // Run 상태일 때의 로직
//             myAnimator.SetTrigger("Run");
//             // 예: 빠른 속도로 플레이어를 향해 이동
//         }
//
//         void ChopAttack()
//         {
//             // ChopAttack 상태일 때의 로직
//             myAnimator.SetTrigger("ChopAttack");
//             // 예: 공격 애니메이션 재생
//         }
//
//         void ThrustAttack()
//         {
//             // ThrustAttack 상태일 때의 로직
//             myAnimator.SetTrigger("ThrustAttack");
//
//             // 예: 다른 유형의 공격 애니메이션 재생
//         }
//
//         void TransitionStates()
//         {
//
//             float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
//             Debug.Log("넌 뭐냐 한번 보자 : " + distanceToPlayer);  
//
//             // 상태 전환 예시 로직
//             if (currentState == MonsterState.Idle && distanceToPlayer <= detectionRange)
//             {
//                 currentState = MonsterState.Run;
//             }
//             else if (currentState == MonsterState.Run && distanceToPlayer <= attackRange)
//             {
//                 currentState = MonsterState.ChopAttack;
//             }
//             else if (currentState == MonsterState.ChopAttack)
//             {
//                 timer += Time.deltaTime;
//                 if (timer > 3.0f) // 공격 상태 유지 시간
//                 {
//                     timer = 0.0f;
//                     currentState = MonsterState.Idle;
//                 }
//             }
//             // 추가적인 상태 전환 조건들...
//         }
//     }
// }
