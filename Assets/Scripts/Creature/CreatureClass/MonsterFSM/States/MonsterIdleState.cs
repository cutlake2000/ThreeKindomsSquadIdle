namespace Creature.CreatureClass.MonsterFSM.States
{
    public class MonsterIdleState : MonsterBaseState
    {
        public MonsterIdleState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
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

            if (Monster.currentTarget == null) return;

            MonsterStateMachine.ChangeState(MonsterStateMachine.MonsterRunState);
        }
    }
}