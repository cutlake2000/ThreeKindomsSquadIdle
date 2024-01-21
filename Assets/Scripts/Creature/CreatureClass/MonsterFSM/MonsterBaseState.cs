using Creature.CreatureClass.MonsterClass;
using Creature.CreatureFSM;
using UnityEngine;

namespace Creature.CreatureClass.MonsterFSM
{
    public class MonsterBaseState : CreatureBaseState
    {
        protected readonly Monster Monster;
        protected readonly MonsterStateMachine MonsterStateMachine;

        protected MonsterBaseState(MonsterStateMachine monsterStateMachine)
        {
            MonsterStateMachine = monsterStateMachine;
            Monster = MonsterStateMachine.Monster;
            Animator = Monster.animator;
            AnimationEventReceiver = Monster.animationEventReceiver;
            AnimationData = Monster.animationData;
            Rigid = Monster.rigid;

            MoveSpeed = Monster.moveSpeed;
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
            Monster.animator.SetBool(animationHash, true);
        }

        protected override void StopAnimationWithBool(int animationHash)
        {
            Monster.animator.SetBool(animationHash, false);
        }

        protected override void StartAnimationWithFloat(int animationHash, int parameterValue)
        {
            Monster.animator.SetFloat(animationHash, parameterValue);
        }

        protected override void StartAnimationWithTrigger(int animationHash)
        {
            Monster.animator.SetTrigger(animationHash);
        }

        protected override bool CheckAttackRange()
        {
            return Monster.currentTarget != null && Monster.attackRange >=
                Vector3.Distance(Monster.currentTarget.transform.position, Monster.transform.position);
        }

        protected override void FlipSprite(float directionX)
        {
            if (Monster.currentTarget == null) return;

            var monsterNewTransform = Monster.transform;
            var scale = monsterNewTransform.localScale;
            var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));

            switch (directionX)
            {
                case > 0.1f:
                    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                    Monster.transform.localScale = localScale;
                    break;
                case < -0.1f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    Monster.transform.localScale = localScale;
                    break;
            }
        }


        protected override void FlipSprite()
        {
            if (Monster.currentTarget == null) return;

            var transform = Monster.transform;
            var scale = transform.localScale;
            var direction = (Monster.currentTarget.position - transform.position).normalized;
            var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));

            switch (direction.x)
            {
                case > 0.1f:
                    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                    Monster.transform.localScale = localScale;
                    break;
                case < -0.1f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    Monster.transform.localScale = localScale;
                    break;
            }
        }
    }
}