using Creature.CreatureClass.MonsterClass;
using Creature.CreatureFSM;
using UnityEngine;

namespace Creature.CreatureClass.MonsterFSM
{
    public class MonsterBaseState : CreatureBaseState
    {
        protected readonly NormalMonster NormalMonster;
        protected readonly MonsterStateMachine MonsterStateMachine;

        protected MonsterBaseState(MonsterStateMachine monsterStateMachine)
        {
            MonsterStateMachine = monsterStateMachine;
            NormalMonster = MonsterStateMachine.NormalMonster;
            Animator = NormalMonster.animator;
            AnimationEventReceiver = NormalMonster.animationEventReceiver;
            AnimationData = NormalMonster.animationData;
            Rigid = NormalMonster.rigid;

            MoveSpeed = NormalMonster.moveSpeed;
            OnAttack = false;
        }

        public override void Update()
        {
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void LogicUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
        }

        protected override void StartAnimationWithBool(int animationHash)
        {
            NormalMonster.animator.SetBool(animationHash, true);
        }

        protected override void StopAnimationWithBool(int animationHash)
        {
            NormalMonster.animator.SetBool(animationHash, false);
        }

        protected override void StartAnimationWithFloat(int animationHash, int parameterValue)
        {
            NormalMonster.animator.SetFloat(animationHash, parameterValue);
        }

        protected override void StartAnimationWithTrigger(int animationHash)
        {
            NormalMonster.animator.SetTrigger(animationHash);
        }

        protected override bool CheckAttackRange()
        {
            return NormalMonster.currentTarget != null && NormalMonster.attackRange >=
                Vector3.Distance(NormalMonster.currentTarget.transform.position, NormalMonster.transform.position);
        }

        protected override void FlipSprite(float directionX)
        {
            if (NormalMonster.currentTarget == null) return;

            var monsterNewTransform = NormalMonster.transform;
            var scale = monsterNewTransform.localScale;
            var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));

            switch (directionX)
            {
                case > 0.1f:
                    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                    NormalMonster.transform.localScale = localScale;
                    break;
                case < -0.1f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    NormalMonster.transform.localScale = localScale;
                    break;
            }
        }


        protected override void FlipSprite()
        {
            if (NormalMonster.currentTarget == null) return;

            var transform = NormalMonster.transform;
            var scale = transform.localScale;
            var direction = (NormalMonster.currentTarget.position - transform.position).normalized;
            var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));

            switch (direction.x)
            {
                case > 0.1f:
                    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                    NormalMonster.transform.localScale = localScale;
                    break;
                case < -0.1f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    NormalMonster.transform.localScale = localScale;
                    break;
            }
        }
    }
}