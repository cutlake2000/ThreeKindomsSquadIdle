namespace Creature.CreatureClass.SquadFSM.States
{
    public class SquadAttackState : SquadBaseState
    {
        public SquadAttackState(SquadStateMachine squadStateMachine) : base(squadStateMachine) {}

        public override void Enter()
        {
            base.Enter();
            
            FlipSprite();
            StartAnimationWithBool(AnimationData.AttackParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimationWithBool(AnimationData.AttackParameterHash);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            ChangeNextState();
        }
        
        /// <summary>
        /// 애니메이션이 끝났다면, 공격 범위를 재확인하여 Run / Attack State로 전환시키는 메서드
        /// </summary>
        private void ChangeNextState()
        {
            var normalizedTime = GetNormalizedTime(Animator, "Attack");

            if (!(normalizedTime >= 1.0f)) return;
            
            SquadStateMachine.ChangeState(SquadStateMachine.SquadRunState);
        }
    }
}