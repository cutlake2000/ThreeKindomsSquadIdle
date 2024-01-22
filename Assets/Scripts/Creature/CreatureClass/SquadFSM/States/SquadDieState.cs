using UnityEngine;

namespace Creature.CreatureClass.SquadFSM.States
{
    public class SquadDieState : SquadBaseState
    {
        public SquadDieState(SquadStateMachine squadStateMachine) : base(squadStateMachine) { }
        
        public override void Enter()
        {
            base.Enter();
            
            StartAnimationWithBool(AnimationData.DieParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimationWithBool(AnimationData.DieParameterHash);
        }

        public override void LogicUpdate()
        {
            var normalizedTime = GetNormalizedTime(Animator, "Die");

            if (!(normalizedTime >= 1.0f)) return;

            Squad.StartCoroutine(Squad.Fade(1, 0));
        }
    }
}