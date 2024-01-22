using Creature.CreatureClass.SquadClass;
using UnityEngine;

namespace Creature.CreatureClass.MonsterFSM.States
{
    public class MonsterRunState : MonsterBaseState
    {
        public MonsterRunState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
        {
        }

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
                if (!OnAttack) OnAttack = true;

                MonsterStateMachine.ChangeState(MonsterStateMachine.MonsterAttackState);
            }
            else
            {
                if (OnAttack) OnAttack = false;
                if (!Monster.detector.activeSelf) Monster.detector.SetActive(true);

                if (Monster.currentTarget == null)
                    MonsterStateMachine.ChangeState(MonsterStateMachine.MonsterIdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!OnAttack) MoveCharacter();
            // if (MonsterNew.currentTarget != null && MonsterNew.currentTarget.GetComponent<Squad>().isDead == false)
            //     MoveCharacter();
        }

        private void MoveCharacter()
        {
            if (Monster.currentTarget == null) return;
            
            var monsterNewTransform = Monster.transform;
            var position = Rigid.transform.position;
            var direction = (Monster.currentTarget.position - position).normalized;

            position += direction * (Monster.moveSpeed * Time.deltaTime);

            monsterNewTransform.position = position;

            FlipSprite(direction.x);
        }
    }
}