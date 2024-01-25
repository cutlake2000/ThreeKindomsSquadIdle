using System;
using System.Linq;
using Creature.CreatureClass.MonsterClass;
using Managers;
using UnityEngine;
using Enum = Data.Enum;

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

                switch (Squad.squadClassType)
                {
                    case Enum.SquadClassType.Warrior:
                        
                        if (!CheckWarriorSkill())
                        {
                            SquadStateMachine.ChangeState(SquadStateMachine.SquadNormalAttackState);
                        }
                        
                        break;
                    
                    case Enum.SquadClassType.Archer:
                        
                        if (!CheckArcherSkill())
                        {
                            SquadStateMachine.ChangeState(SquadStateMachine.SquadNormalAttackState);
                        }
                        
                        break;
                    
                    case Enum.SquadClassType.Wizard:
                        
                        if (!CheckWizardSkill ())
                        {
                            SquadStateMachine.ChangeState(SquadStateMachine.SquadNormalAttackState);
                        }

                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                if (OnAttack) OnAttack = false;

                if (Squad.currentTarget == null)
                {
                    SquadStateMachine.ChangeState(SquadStateMachine.SquadIdleState);
                }
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
            for (var i = 0; i < SquadManager.Instance.warriorSkillCoolTimer.Length; i++)
            {
                if (!SquadManager.Instance.warriorSkillCoolTimer[i].isSkillReady || (!SquadManager.Instance.autoSkill && !SquadManager.Instance.warriorSkillCoolTimer[i].orderToInstantiate)) continue;
                
                SquadStateMachine.ChangeState(SquadStateMachine.SquadSkillAttackState);

                return true;
            }

            return false;
        }
        
        private bool CheckArcherSkill()
        {
            for (var i = 0; i < SquadManager.Instance.warriorSkillCoolTimer.Length; i++)
            {
                if (!SquadManager.Instance.warriorSkillCoolTimer[i].isSkillReady || (!SquadManager.Instance.autoSkill && !SquadManager.Instance.warriorSkillCoolTimer[i].orderToInstantiate)) continue;
                
                SquadStateMachine.ChangeState(SquadStateMachine.SquadSkillAttackState);

                return true;
            }

            return false;
        }
        
        private bool CheckWizardSkill()
        {
            for (var i = 0; i < SquadManager.Instance.warriorSkillCoolTimer.Length; i++)
            {
                if (!SquadManager.Instance.warriorSkillCoolTimer[i].isSkillReady || (!SquadManager.Instance.autoSkill && !SquadManager.Instance.warriorSkillCoolTimer[i].orderToInstantiate)) continue;
                
                SquadStateMachine.ChangeState(SquadStateMachine.SquadSkillAttackState);

                return true;
            }

            return false;
        }
    }
}