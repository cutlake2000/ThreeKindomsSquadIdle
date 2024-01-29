using Creature.CreatureClass.SquadClass;
using Creature.CreatureFSM;
using UnityEngine;

namespace Creature.CreatureClass.SquadFSM
{
    public class SquadBaseState : CreatureBaseState
    {
        protected readonly Squad Squad;
        protected readonly SquadStateMachine SquadStateMachine;

        protected SquadBaseState(SquadStateMachine squadStateMachine)
        {
            SquadStateMachine = squadStateMachine;
            Squad = SquadStateMachine.Squad;
            Animator = Squad.animator;
            AnimationEventReceiver = Squad.animationEventReceiver;
            AnimationData = Squad.animationData;
            Rigid = Squad.rigid;

            MoveSpeed = Squad.moveSpeed;
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
            Squad.animator.SetBool(animationHash, true);
        }

        protected override void StopAnimationWithBool(int animationHash)
        {
            Squad.animator.SetBool(animationHash, false);
        }

        protected override void StartAnimationWithFloat(int animationHash, int parameterValue)
        {
            Squad.animator.SetFloat(animationHash, parameterValue);
        }

        protected override void StartAnimationWithTrigger(int animationHash)
        {
            Squad.animator.SetTrigger(animationHash);
        }

        protected override bool CheckAttackRange()
        {
            return Squad.currentTarget != null && Squad.attackRange >=
                Vector3.Distance(Squad.currentTarget.transform.position, Squad.transform.position);
        }

        protected override void FlipSprite(float directionX)
        {
            if (Squad.currentTarget == null) return;

            var transform = Squad.transform;
            var scale = transform.localScale;
            var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));

            switch (directionX)
            {
                case > 0f:
                    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                    Squad.transform.localScale = localScale;
                    break;
                case < 0f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    Squad.transform.localScale = localScale;
                    break;
            }
        }


        protected override void FlipSprite()
        {
            if (Squad.currentTarget == null) return;

            var squadTransform = Squad.transform;
            var scale = squadTransform.localScale;
            var direction = (Squad.currentTarget.position - squadTransform.position).normalized;
            var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));

            switch (direction.x)
            {
                case > 0.1f:
                    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                    Squad.transform.localScale = localScale;
                    break;
                case < -0.1f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    Squad.transform.localScale = localScale;
                    break;
            }
        }
    }
}