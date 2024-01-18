namespace Creature.CreatureFSM
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void LogicUpdate();
        public void Update();
        public void PhysicsUpdate();
    }
}