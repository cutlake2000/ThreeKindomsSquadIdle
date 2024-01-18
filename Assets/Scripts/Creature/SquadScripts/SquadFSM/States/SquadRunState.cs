using System.Linq;
using Creature.MonsterScripts.MonsterClass;
using Data;
using Managers;
using UnityEngine;

namespace Creature.SquadScripts.SquadFSM.States
{
    public class SquadRunState : SquadBaseState
    {
        public SquadRunState(SquadStateMachine squadStateMachine) : base(squadStateMachine) { }

        public override void Enter()
        {
            base.Enter();
            
            StartAnimationWithFloat(AnimationData.RunStateParameterHash, 1);
        }

        public override void Exit()
        {
            base.Exit();
            
            StartAnimationWithFloat(AnimationData.RunStateParameterHash, 0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (CheckAttackRange())
            {
                //TODO: 내일 로직 수정
                if (Squad.squadClassType == Enum.SquadClassType.Warrior)
                {
                    if (SquadManager.Instance.warriorSkillCoolTimer.Any(timer => timer.isSkillReady))
                    {
                        SquadStateMachine.ChangeState(SquadStateMachine.SquadSkillAttackState);
                    }
                    else
                    {
                        SquadStateMachine.ChangeState(SquadStateMachine.SquadNormalAttackState);
                    }
                }
                else
                {
                    SquadStateMachine.ChangeState(SquadStateMachine.SquadNormalAttackState);   
                }
            }
            else if (Squad.currentTarget == null)
            {
                SquadStateMachine.ChangeState(SquadStateMachine.SquadIdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            //TODO: GetComponent 넣으면 위험
            if (Squad.currentTarget != null && Squad.currentTarget.GetComponent<MonsterNew>().isDead == false)
            {
                MoveCharacter();
            }
        }

        private void MoveCharacter()
        {
            var squadTransform = Squad.transform;
            var position = Rigid.transform.position;
            var direction = (Squad.currentTarget.position - position).normalized;
            
            position += direction * (Squad.moveSpeed * Time.deltaTime);
            
            squadTransform.position = position;

            FlipSprite(direction.x);
        }
    }
}