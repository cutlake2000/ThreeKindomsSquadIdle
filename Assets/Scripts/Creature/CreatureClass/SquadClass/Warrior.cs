using Data;
using Managers;
using Managers.BattleManager;
using Managers.GameManager;
using UnityEngine;

namespace Creature.CreatureClass.SquadClass
{
    public class Warrior : Squad
    {
        protected override void SetCreatureStats()
        {
            base.SetCreatureStats();
            
            attackRange = SquadBattleManager.Instance.GetTotalSubSquadStat(Enums.SquadStatType.WarriorAttackRange);

            animator.SetFloat(animationData.ClassTypeParameterHash, 0);
        }

        protected override void OnNormalAttack()
        {
            base.OnNormalAttack();
            
            // Debug.Log("워리어 공격");

            ProjectileManager.Instance.InstantiateBaseAttack(Attack, Vector2.zero, Direction,
                Enums.PoolType.ProjectileBaseAttackWarrior, isCriticalAttack);

            isCriticalAttack = false;
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
                    SquadBattleManager.Instance.warriorSkillCoolTimer[i].skill, ((Creature)this).Attack, ProjectileSpawnPosition,
                    currentTarget.transform.position);
                SquadBattleManager.Instance.warriorSkillCoolTimer[i].orderToInstantiate = false;

                break;
            }
        }
    }
}