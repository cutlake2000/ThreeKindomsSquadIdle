using Data;
using Keiwando.BigInteger;
using Managers;
using Managers.BattleManager;
using Managers.GameManager;
using UnityEngine;

namespace Creature.CreatureClass.SquadClass
{
    public class Archer : Squad
    {
        protected override void SetCreatureStats()
        {
            base.SetCreatureStats();

            attackRange = SquadBattleManager.Instance.GetTotalSubSquadStat(Enums.SquadStatType.ArcherAttackRange);

            animator.SetFloat(animationData.ClassTypeParameterHash, 1);
        }

        protected override void OnNormalAttack()
        {
            base.OnNormalAttack();
            
            ProjectileManager.Instance.InstantiateBaseAttack(Attack, ProjectileSpawnPosition, Direction, Enums.PoolType.ProjectileBaseAttackArcher, BigInteger.ToInt32(CriticalRate) / 10000, BigInteger.ToInt32(CriticalDamage) / 10000);
        }

        protected override void OnNormalAttackEffect()
        {
            EffectManager.Instance.CreateParticlesAtPosition(projectileSpawn.position, Enums.CharacterType.Archer,
                false);
        }

        protected override void OnSkillAttack()
        {
            base.OnSkillAttack();

            for (var i = 0; i < SquadBattleManager.Instance.archerSkillCoolTimer.Length; i++)
            {
                if (!SquadBattleManager.Instance.archerSkillCoolTimer[i].isSkillReady) continue;
                if (!SquadBattleManager.Instance.autoSkill &&
                    !SquadBattleManager.Instance.archerSkillCoolTimer[i].orderToInstantiate) continue;

                if (currentTarget == null) return;

                SquadBattleManager.Instance.RunSkillCoolTimer(Enums.CharacterType.Archer, i);
                ProjectileManager.Instance.InstantiateSkillAttack(
                    SquadBattleManager.Instance.archerSkillCoolTimer[i].skill, Attack, ProjectileSpawnPosition,
                    currentTarget.transform.position);
                SquadBattleManager.Instance.archerSkillCoolTimer[i].orderToInstantiate = false;

                break;
            }
        }
    }
}