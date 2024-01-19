using UnityEngine;

namespace Creature.CreatureClass.SquadFSM.States
{
    public class SquadSkillAttackState : SquadAttackState
    {
        public SquadSkillAttackState(SquadStateMachine squadStateMachine) : base(squadStateMachine) { }
        
        public override void Enter()
        {
            Debug.Log("가랏 워리어 몸통박치기!");
            StartAnimationWithFloat(AnimationData.AttackStateParameterHash, 1);
            
            base.Enter();
        }

        public override void Exit()
        {
            StartAnimationWithFloat(AnimationData.AttackStateParameterHash, -1);
            
            base.Exit();
        }
    }
}