using Creature.CreatureFSM;
using Creature.MonsterScripts.MonsterClass;
using UnityEngine;

namespace Creature.MonsterScripts.MonsterFSM
{
    public class MonsterBaseState : CreatureBaseState
    {
        protected readonly MonsterNew MonsterNew;
        protected readonly MonsterStateMachine MonsterStateMachine;
        
        protected MonsterBaseState(MonsterStateMachine monsterStateMachine)
        {
            MonsterStateMachine = monsterStateMachine;
            MonsterNew = MonsterStateMachine.MonsterNew;
            Animator = MonsterNew.animator;
            AnimationEventReceiver = MonsterNew.animationEventReceiver;
            AnimationData = MonsterNew.animationData;
            Rigid = MonsterNew.rigid;

            MoveSpeed = MonsterNew.moveSpeed;
        }

        public override void Update() { }

        public override void Enter() { }

        public override void Exit() { }

        public override void LogicUpdate() { }

        public override void PhysicsUpdate() { }
        
        protected override void StartAnimationWithBool(int animationHash)
        {
            MonsterNew.animator.SetBool(animationHash, true);
        }

        protected override void StopAnimationWithBool(int animationHash)
        {
            MonsterNew.animator.SetBool(animationHash, false);
        }

        protected override void StartAnimationWithFloat(int animationHash, int parameterValue)
        {
            MonsterNew.animator.SetFloat(animationHash, parameterValue);
        }

        protected override void StartAnimationWithTrigger(int animationHash)
        {
            MonsterNew.animator.SetTrigger(animationHash);
        }

        protected override bool CheckAttackRange()
        {
            return MonsterNew.currentTarget != null && MonsterNew.attackRange >= Vector3.Distance(MonsterNew.currentTarget.transform.position, MonsterNew.transform.position);
        }

        protected override void FlipSprite(float directionX)
        {
            if (MonsterNew.currentTarget == null) return;
            
            var monsterNewTransform = MonsterNew.transform;
            var scale = monsterNewTransform.localScale;
            var localScale = new Vector3(Mathf.Abs(scale.x),Mathf.Abs(scale.y), Mathf.Abs(scale.z));
            
            switch (directionX)
            {
                case > 0.1f:
                    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                    MonsterNew.transform.localScale = localScale;
                    break;
                case < -0.1f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    MonsterNew.transform.localScale = localScale;
                    break;
            }
        }


        protected override void FlipSprite()
        {
            if (MonsterNew.currentTarget == null) return;

            var transform = MonsterNew.transform;
            var scale = transform.localScale;
            var direction = (MonsterNew.currentTarget.position - transform.position).normalized;
            var localScale = new Vector3(Mathf.Abs(scale.x),Mathf.Abs(scale.y), Mathf.Abs(scale.z));
            
            switch (direction.x)
            {
                case > 0.1f:
                    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                    MonsterNew.transform.localScale = localScale;
                    break;
                case < -0.1f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    MonsterNew.transform.localScale = localScale;
                    break;
            }
        }
    }
}