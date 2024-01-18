using Creature.Data;
using Module;
using UnityEngine;

namespace Creature.CreatureFSM
{
    public class CreatureBaseState : MonoBehaviour, IState
    {
        protected Animator Animator;
        protected AnimationEventReceiver AnimationEventReceiver;
        protected Rigidbody2D Rigid;
        protected AnimationData AnimationData;
        
        protected float MoveSpeed;

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void LogicUpdate() { }

        public virtual void Update() { }

        public virtual void PhysicsUpdate() { }

        protected virtual void StartAnimationWithBool(int animationHash){}

        protected virtual void StopAnimationWithBool(int animationHash){}

        protected virtual void StartAnimationWithFloat(int animationHash, int parameterValue){}

        protected virtual void StartAnimationWithTrigger(int animationHash){}

        protected virtual bool CheckAttackRange()
        {
            return false;}

        protected static float GetNormalizedTime(Animator animator, string tag)
        {
            var currentInfo = animator.GetCurrentAnimatorStateInfo(0);
            var nextInfo = animator.GetNextAnimatorStateInfo(0);

            if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
            {
                return nextInfo.normalizedTime;
            }

            if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
            {
                return currentInfo.normalizedTime;
            }

            return 0f;
        }

        protected virtual void FlipSprite(float directionX){ }

        protected virtual void FlipSprite(){}
    }
}