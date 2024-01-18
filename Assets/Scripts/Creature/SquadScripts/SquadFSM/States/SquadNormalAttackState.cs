namespace Creature.SquadScripts.SquadFSM.States
{
    public class SquadNormalAttackState : SquadAttackState
    {
        public SquadNormalAttackState(SquadStateMachine squadStateMachine) : base(squadStateMachine) { }

        public override void Enter()
        {
            StartAnimationWithFloat(AnimationData.AttackStateParameterHash, 0);
            
            base.Enter();
        }

        public override void Exit()
        {
            StartAnimationWithFloat(AnimationData.AttackStateParameterHash, -1);
            
            base.Exit();
        }
    }
}