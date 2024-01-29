namespace Creature.CreatureClass.MonsterFSM.States
{
    public class MonsterDieState : MonsterBaseState
    {
        public MonsterDieState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
        {
        }

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

            Monster.StartCoroutine(Monster.Fade(1, 0));
        }
    }
}