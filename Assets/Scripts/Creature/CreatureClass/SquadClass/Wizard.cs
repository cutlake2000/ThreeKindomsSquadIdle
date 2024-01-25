using Data;
using Managers;
using UnityEngine;

namespace Creature.CreatureClass.SquadClass
{
    public class Wizard : Squad
    {
        protected override void SetCreatureStats()
        {
            base.SetCreatureStats();

            damage = SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.WizardAtk);
            attackRange = SquadManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.WizardAttackRange);

            animator.SetFloat(animationData.ClassTypeParameterHash, 2);
        }

        protected override void OnNormalAttack()
        {
            base.OnNormalAttack();
            
            ProjectileManager.Instance.InstantiateBaseAttack(damage, ProjectileSpawnPosition, Direction, Enum.PoolType.ProjectileBaseAttackWizard);
        }
        
        protected override void OnSkillAttack()
        {
            base.OnSkillAttack();
            
            for (var i = 0; i < SquadManager.Instance.wizardSkillCoolTimer.Length; i++)
            {
                if (!SquadManager.Instance.wizardSkillCoolTimer[i].isSkillReady) continue;
                if (!SquadManager.Instance.autoSkill && !SquadManager.Instance.wizardSkillCoolTimer[i].orderToInstantiate) continue;
                
                if (currentTarget == null) return;
                
                SquadManager.Instance.RunSkillCoolTimer(Enum.SquadClassType.Wizard, i);
                ProjectileManager.Instance.InstantiateSkillAttack(SquadManager.Instance.wizardSkillCoolTimer[i].skill, damage, ProjectileSpawnPosition, currentTarget.transform.position);
                SquadManager.Instance.wizardSkillCoolTimer[i].orderToInstantiate = false;
                
                break;
            }
        }
    }
}