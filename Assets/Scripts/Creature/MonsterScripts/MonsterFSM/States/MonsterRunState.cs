using Creature.SquadScripts.SquadClass;
using UnityEngine;

namespace Creature.MonsterScripts.MonsterFSM.States
{
    public class MonsterRunState : MonsterBaseState
    {
        public MonsterRunState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine) { }
        
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
                Debug.Log($"적팀의 몸통 박치기!!");
                MonsterStateMachine.ChangeState(MonsterStateMachine.MonsterAttackState);
            }
            else if (MonsterNew.currentTarget == null)
            {
                MonsterStateMachine.ChangeState(MonsterStateMachine.MonsterIdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            //TODO: GetComponent 넣으면 위험
            if (MonsterNew.currentTarget != null && MonsterNew.currentTarget.GetComponent<Squad>().isDead == false)
            {
                MoveCharacter();
            }
        }

        private void MoveCharacter()
        {
            var monsterNewTransform = MonsterNew.transform;
            var position = Rigid.transform.position;
            var direction = (MonsterNew.currentTarget.position - position).normalized;
            
            position += direction * (MonsterNew.moveSpeed * Time.deltaTime);
            
            monsterNewTransform.position = position;

            FlipSprite(direction.x);
        }
    }
}