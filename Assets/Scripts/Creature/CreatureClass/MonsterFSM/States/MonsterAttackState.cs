using UnityEngine;

namespace Creature.CreatureClass.MonsterFSM.States
{
    public class MonsterAttackState : MonsterBaseState
    {
        public MonsterAttackState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine) { }
        
        public override void Enter()
        {
            base.Enter();
            
            Debug.Log("공격!");
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
        
        private void ChangeNextState()
        {
            var normalizedTime = GetNormalizedTime(Animator, "Attack");

            if (!(normalizedTime >= 1.0f)) return;
            
            MonsterStateMachine.ChangeState(MonsterStateMachine.MonsterRunState);
        }
    }
}