namespace Creature.CreatureFSM
{
    public class CreatureStateMachine
    {
        private IState currentState;

        public void ChangeState(IState newState)
        {
            currentState?.Exit();

            currentState = newState;

            currentState?.Enter();
        }

        public void LogicUpdate()
        {
            currentState?.LogicUpdate();
        }

        public void Update()
        {
            currentState?.Update();
        }

        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate();
        }
    }
}