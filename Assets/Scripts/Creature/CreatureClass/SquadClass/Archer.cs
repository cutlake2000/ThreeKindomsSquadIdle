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

            damage = SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.ArcherAtk);
            attackRange = SquadManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.ArcherAttackRange);

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
            EffectManager.Instance.CreateParticlesAtPosition(projectileSpawn.position, Enum.SquadClassType.Archer, false);
        }

        protected override void OnSkillAttack1()
        {
            base.OnSkillAttack1();

            for (var i = 0; i < SquadManager.Instance.archerSkillCoolTimer.Length; i++)
            {
                if (!SquadManager.Instance.archerSkillCoolTimer[i].isSkillReady) continue;
                SquadManager.Instance.RunSkillCoolTimer(Enum.SquadClassType.Archer, i);
                ProjectileManager.Instance.InstantiateSkillAttack(SquadManager.Instance.archerSkillCoolTimer[i].skill, damage, ProjectileSpawnPosition, currentTarget.transform.position);
            }
        }
    }
}