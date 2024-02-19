using Data;
using Managers;
using Managers.BattleManager;
using Managers.GameManager;
using UnityEngine;

namespace Creature.CreatureClass.SquadClass
{
    public class Wizard : Squad
    {
        protected override void SetCreatureStats()
        {
            base.SetCreatureStats();
            
            attackRange = SquadBattleManager.Instance.GetTotalSubSquadStat(Enums.SquadStatType.WizardAttackRange);

            animator.SetFloat(animationData.ClassTypeParameterHash, 2);
        }

        protected override void OnNormalAttack()
        {
            base.OnNormalAttack();
            
            ProjectileManager.Instance.InstantiateBaseAttack(Attack, ProjectileSpawnPosition, Direction,
                Enums.PoolType.ProjectileBaseAttackWizard, isCriticalAttack);

            isCriticalAttack = false;
        }

        protected override void OnSkillAttack()
        {
            base.OnSkillAttack();

            for (var i = 0; i < SquadBattleManager.Instance.wizardSkillCoolTimer.Length; i++)
            {
                if (!SquadBattleManager.Instance.wizardSkillCoolTimer[i].isSkillReady) continue;
                if (!SquadBattleManager.Instance.autoSkill &&
                    !SquadBattleManager.Instance.wizardSkillCoolTimer[i].orderToInstantiate) continue;

                if (currentTarget == null) return;

                SquadBattleManager.Instance.RunSkillCoolTimer(Enums.CharacterType.Wizard, i);
                ProjectileManager.Instance.InstantiateSkillAttack(
                    SquadBattleManager.Instance.wizardSkillCoolTimer[i].skill, ((Creature)this).Attack, ProjectileSpawnPosition,
                    currentTarget.transform.position);
                SquadBattleManager.Instance.wizardSkillCoolTimer[i].orderToInstantiate = false;

                break;
            }
        }
    }
}