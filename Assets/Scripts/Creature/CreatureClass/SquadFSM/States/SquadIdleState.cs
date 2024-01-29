namespace Creature.CreatureClass.SquadFSM.States
{
    public class SquadIdleState : SquadBaseState
    {
        public SquadIdleState(SquadStateMachine squadStateMachine) : base(squadStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimationWithFloat(AnimationData.RunStateParameterHash, 0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Squad.currentTarget == null) return;

            SquadStateMachine.ChangeState(SquadStateMachine.SquadRunState);
        }
    }
}