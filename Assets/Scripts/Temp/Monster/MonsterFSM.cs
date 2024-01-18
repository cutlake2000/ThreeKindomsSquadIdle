// using System.Collections;
// using System.Collections.Generic;
// using MonsterLove.StateMachine;
// using UnityEngine;
//
// public class MonsterFSM : MonoBehaviour
// {
//     Monster monster;
//     MonsterControler monsterControler;
//
//     StateMachine<Enums.StateEnum> FSM;
//
//     WaitForSeconds deathDelay;
//
//     private void Awake()
//     {
//         FSM = new StateMachine<Enums.StateEnum>(this);
//         monster = GetComponent<Monster>();
//         monsterControler = GetComponent<MonsterControler>();
//         FSM.ChangeState(Enums.StateEnum.Spawn);
//     }
//
//     private void Start()
//     {
//         deathDelay = new WaitForSeconds(monster.GetAnimationLength(Strings.ANIMATION_DEATH));
//     }
//
//     private void Update()
//     {
//         FSM.Driver.Update?.Invoke();
//     }
//     private void FixedUpdate()
//     {
//         FSM.Driver.FixedUpdate.Invoke();
//     }
//
//     void Spawn_Enter()
//     {
//         Debug.Log("Spawn");
//         FSM.ChangeState(Enums.StateEnum.Idle);
//     }
//
//     void Idle_Enter()
//     {
//         monster.StartAnimation(Strings.ANIMATION_IDLE);
//         Debug.Log(Strings.ANIMATION_IDLE);
//         FSM.ChangeState(Enums.StateEnum.Run);
//     }
//
//     void Idle_Exit()
//     {
//         monster.StopAnimation(Strings.ANIMATION_IDLE);
//     }
//
//     void Run_Enter()
//     {
//         monster.StartAnimation(Strings.ANIMATION_RUN);
//         Debug.Log(Strings.ANIMATION_RUN);
//     }
//
//     void Run_Update()
//     {
//         if (monster.isAttacking)
//         {
//             Debug.Log("몬스터 공격 한다!");
//             FSM.ChangeState(Enums.StateEnum.MeleeAttack);
//         }
//     }
//
//     void Run_FixedUpdate()
//     {
//         if (!monsterControler.Move())
//         {
//             FSM.ChangeState(Enums.StateEnum.Idle);
//         }
//     }
//
//     void Run_Exit()
//     {
//         monster.StopAnimation(Strings.ANIMATION_RUN);
//     }
//
//     void MeleeAttack_Enter()
//     {
//         Debug.Log("MeleeAttack");
//     }
//
//     void MeleeAttack_Update()
//     {
//         if (monster.CheckHealth()) FSM.ChangeState(Enums.StateEnum.Death);
//     }
//
//     void RangedAttack_Enter()
//     {
//         Debug.Log("RangedAttack");
//     }
//
//     void RangedAttack_Update()
//     {
//         if (monster.CheckHealth()) FSM.ChangeState(Enums.StateEnum.Death);
//     }
//
//     void Death_Enter()
//     {
//         StartCoroutine(Death());
//
//         Debug.Log(Strings.ANIMATION_DEATH);
//     }
//
//     IEnumerator Death()
//     {
//         Debug.Log("불리고 있나니? ");
//         monster.StartAnimation(Strings.ANIMATION_DEATH);
//
//         yield return deathDelay;
//
//         monster.gameObject.SetActive(false);
//
//     }
// }
