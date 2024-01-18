using Data;
using Managers;

namespace Creature.SquadScripts.SquadClass
{
    public class Archer : Squad
    {
        protected override void SetCreatureStats()
        {
            base.SetCreatureStats();

            attack = SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.ArcherAtk);
            attackRange = SquadManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.ArcherAttackRange);
            
            animator.SetFloat(animationData.ClassTypeParameterHash, 1);
        }

        protected override void OnNormalAttack()
        {
            base.OnNormalAttack();
            
            ProjectileManager.Instance.InstantiateBaseAttack(attack, ProjectileSpawnPosition, Direction, Enum.PoolType.ProjectileBaseAttackArcher);
        }

        protected override void OnNormalAttackEffect()
        {
            EffectManager.Instance.CreateParticlesAtPosition(projectileSpawn.position, Enum.SquadClassType.Archer, false);
        }
    }
}