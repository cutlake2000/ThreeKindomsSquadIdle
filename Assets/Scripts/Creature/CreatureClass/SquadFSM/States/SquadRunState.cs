using System;
using Data;
using Managers;
using Managers.BattleManager;
using Managers.GameManager;
using UnityEngine;

namespace Creature.CreatureClass.SquadFSM.States
{
    public class SquadRunState : SquadBaseState
    {
        public SquadRunState(SquadStateMachine squadStateMachine) : base(squadStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimationWithFloat(AnimationData.RunStateParameterHash, 1);
        }

        public override void Exit()
        {
            base.Exit();

            StartAnimationWithFloat(AnimationData.RunStateParameterHash, 0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (CheckAttackRange())
            {
                if (!OnAttack) OnAttack = true;

                switch (Squad.characterType)
                {
                    case Enums.CharacterType.Warrior:

                        if (!CheckWarriorSkill())
                            SquadStateMachine.ChangeState(SquadStateMachine.SquadNormalAttackState);

                        break;

                    case Enums.CharacterType.Archer:

                        if (!CheckArcherSkill())
                            SquadStateMachine.ChangeState(SquadStateMachine.SquadNormalAttackState);

                        break;

                    case Enums.CharacterType.Wizard:

                        if (!CheckWizardSkill())
                            SquadStateMachine.ChangeState(SquadStateMachine.SquadNormalAttackState);

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                if (OnAttack) OnAttack = false;

                if (Squad.currentTarget == null) SquadStateMachine.ChangeState(SquadStateMachine.SquadIdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!OnAttack) MoveCharacter();
            // if (Squad.currentTarget != null && Squad.currentTarget.GetComponent<MonsterNew>().isDead == false)
            //     MoveCharacter();
        }

        private void MoveCharacter()
        {
            var squadTransform = Squad.transform;
            var position = Rigid.transform.position;

            if (Squad.currentTarget == null) return;
            var direction = (Squad.currentTarget.position - position).normalized;

            position += direction * (Squad.moveSpeed * Time.deltaTime);

            squadTransform.position = position;

            FlipSprite(direction.x);
        }

        private bool CheckWarriorSkill()
        {
            for (var i = 0; i < SquadBattleManager.Instance.warriorSkillCoolTimer.Length; i++)
            {
                if (!SquadBattleManager.Instance.warriorSkillCoolTimer[i].isSkillReady ||
                    (!SquadBattleManager.Instance.autoSkill &&
                     !SquadBattleManager.Instance.warriorSkillCoolTimer[i].orderToInstantiate)) continue;

                SquadStateMachine.ChangeState(SquadStateMachine.SquadSkillAttackState);

                return true;
            }

            return false;
        }

        private bool CheckArcherSkill()
        {
            for (var i = 0; i < SquadBattleManager.Instance.archerSkillCoolTimer.Length; i++)
            {
                if (!SquadBattleManager.Instance.archerSkillCoolTimer[i].isSkillReady ||
                    (!SquadBattleManager.Instance.autoSkill &&
                     !SquadBattleManager.Instance.archerSkillCoolTimer[i].orderToInstantiate)) continue;

                SquadStateMachine.ChangeState(SquadStateMachine.SquadSkillAttackState);

                return true;
            }

            return false;
        }

        private bool CheckWizardSkill()
        {
            for (var i = 0; i < SquadBattleManager.Instance.wizardSkillCoolTimer.Length; i++)
            {
                if (!SquadBattleManager.Instance.wizardSkillCoolTimer[i].isSkillReady ||
                    (!SquadBattleManager.Instance.autoSkill &&
                     !SquadBattleManager.Instance.wizardSkillCoolTimer[i].orderToInstantiate)) continue;

                SquadStateMachine.ChangeState(SquadStateMachine.SquadSkillAttackState);

                return true;
            }

            return false;
        }
    }
}