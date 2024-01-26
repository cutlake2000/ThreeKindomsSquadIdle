using Data;
using Managers;
using UnityEngine;

namespace Creature.CreatureClass.SquadClass
{
    public class Archer : Squad
    {
        protected override void SetCreatureStats()
        {
            base.SetCreatureStats();

            damage = SquadBattleManager.Instance.GetTotalSquadStat(Enum.SquadStatType.ArcherAtk);
            attackRange = SquadBattleManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.ArcherAttackRange);

            animator.SetFloat(animationData.ClassTypeParameterHash, 1);
        }

        protected override void OnNormalAttack()
        {
            base.OnNormalAttack();

            ProjectileManager.Instance.InstantiateBaseAttack(damage, ProjectileSpawnPosition, Direction,
                Enum.PoolType.ProjectileBaseAttackArcher);
        }

        protected override void OnNormalAttackEffect()
        {
            EffectManager.Instance.CreateParticlesAtPosition(projectileSpawn.position, Enum.CharacterType.Archer, false);
        }

        protected override void OnSkillAttack()
        {
            base.OnSkillAttack();

            for (var i = 0; i < SquadBattleManager.Instance.archerSkillCoolTimer.Length; i++)
            {
                if (!SquadBattleManager.Instance.archerSkillCoolTimer[i].isSkillReady) continue;
                if (!SquadBattleManager.Instance.autoSkill && !SquadBattleManager.Instance.archerSkillCoolTimer[i].orderToInstantiate) continue;
                
                if (currentTarget == null) return;
                
                SquadBattleManager.Instance.RunSkillCoolTimer(Enum.CharacterType.Archer, i);
                ProjectileManager.Instance.InstantiateSkillAttack(SquadBattleManager.Instance.archerSkillCoolTimer[i].skill, damage, ProjectileSpawnPosition, currentTarget.transform.position);
                SquadBattleManager.Instance.archerSkillCoolTimer[i].orderToInstantiate = false;
                
                break;
            }
        }
    }
}