using Data;
using Keiwando.BigInteger;
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
            
            ProjectileManager.Instance.InstantiateBaseAttack(Attack, ProjectileSpawnPosition, Direction, Enums.PoolType.ProjectileBaseAttackWizard, BigInteger.ToInt32(CriticalRate), BigInteger.ToInt32(CriticalDamage));
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
                    SquadBattleManager.Instance.wizardSkillCoolTimer[i].skill, Attack, ProjectileSpawnPosition,
                    currentTarget.transform.position);
                SquadBattleManager.Instance.wizardSkillCoolTimer[i].orderToInstantiate = false;

                break;
            }
        }
    }
}