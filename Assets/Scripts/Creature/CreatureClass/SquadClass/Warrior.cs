using Managers;
using UnityEngine;
using Enum = Data.Enum;

namespace Creature.CreatureClass.SquadClass
{
    public class Warrior : Squad
    {
        protected override void SetCreatureStats()
        {
            base.SetCreatureStats();

            // attack = SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.WarriorAtk);
            damage = SquadManager.Instance.totalWarriorAttack;
            attackRange = SquadManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.WarriorAttackRange);
            
            animator.SetFloat(animationData.ClassTypeParameterHash, 0);
        }

        protected override void OnNormalAttack()
        {
            base.OnNormalAttack();

            ProjectileManager.Instance.InstantiateBaseAttack(damage, ProjectileSpawnPosition, Direction,
                Enum.PoolType.ProjectileBaseAttackWarrior);
        }

        protected override void OnSkillAttack1()
        {
            base.OnSkillAttack1();
            
            for (var i = 0; i < SquadManager.Instance.warriorSkillCoolTimer.Length; i++)
            {
                if (!SquadManager.Instance.warriorSkillCoolTimer[i].isSkillReady) continue;
                SquadManager.Instance.RunSkillCoolTimer(Enum.SquadClassType.Warrior, i);
                ProjectileManager.Instance.InstantiateSkillAttack(SquadManager.Instance.warriorSkillCoolTimer[i].skill, damage, ProjectileSpawnPosition, currentTarget.transform.position);
            }
        }
    }
}