using Data;
using Managers;
using Managers.BattleManager;

namespace Creature.CreatureClass.SquadClass
{
    public class Warrior : Squad
    {
        protected override void SetCreatureStats()
        {
            base.SetCreatureStats();

            damage = SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.WarriorAtk);
            attackRange = SquadBattleManager.Instance.GetTotalSubSquadStat(Enums.SquadStatType.WarriorAttackRange);

            animator.SetFloat(animationData.ClassTypeParameterHash, 0);
        }

        protected override void OnNormalAttack()
        {
            base.OnNormalAttack();

            ProjectileManager.Instance.InstantiateBaseAttack(damage, ProjectileSpawnPosition, Direction,
                Enums.PoolType.ProjectileBaseAttackWarrior);
        }

        protected override void OnSkillAttack()
        {
            base.OnSkillAttack();

            for (var i = 0; i < SquadBattleManager.Instance.warriorSkillCoolTimer.Length; i++)
            {
                if (!SquadBattleManager.Instance.warriorSkillCoolTimer[i].isSkillReady) continue;
                if (!SquadBattleManager.Instance.autoSkill &&
                    !SquadBattleManager.Instance.warriorSkillCoolTimer[i].orderToInstantiate) continue;

                if (currentTarget == null) return;

                SquadBattleManager.Instance.RunSkillCoolTimer(Enums.CharacterType.Warrior, i);
                ProjectileManager.Instance.InstantiateSkillAttack(
                    SquadBattleManager.Instance.warriorSkillCoolTimer[i].skill, damage, ProjectileSpawnPosition,
                    currentTarget.transform.position);
                SquadBattleManager.Instance.warriorSkillCoolTimer[i].orderToInstantiate = false;

                break;
            }
        }
    }
}