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

            damage = SquadBattleManager.Instance.GetTotalSquadStat(Enum.SquadStatType.WizardAtk);
            attackRange = SquadBattleManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.WizardAttackRange);

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
            
            for (var i = 0; i < SquadBattleManager.Instance.wizardSkillCoolTimer.Length; i++)
            {
                if (!SquadBattleManager.Instance.wizardSkillCoolTimer[i].isSkillReady) continue;
                if (!SquadBattleManager.Instance.autoSkill && !SquadBattleManager.Instance.wizardSkillCoolTimer[i].orderToInstantiate) continue;
                
                if (currentTarget == null) return;
                
                SquadBattleManager.Instance.RunSkillCoolTimer(Enum.CharacterType.Wizard, i);
                ProjectileManager.Instance.InstantiateSkillAttack(SquadBattleManager.Instance.wizardSkillCoolTimer[i].skill, damage, ProjectileSpawnPosition, currentTarget.transform.position);
                SquadBattleManager.Instance.wizardSkillCoolTimer[i].orderToInstantiate = false;
                
                break;
            }
        }
    }
}