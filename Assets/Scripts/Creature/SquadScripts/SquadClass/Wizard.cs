using Data;
using Managers;

namespace Creature.SquadScripts.SquadClass
{
    public class Wizard : Squad
    {
        protected override void SetCreatureStats()
        {
            base.SetCreatureStats();

            attack = SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.WizardAtk);
            attackRange = SquadManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.WizardAttackRange);

            animator.SetFloat(animationData.ClassTypeParameterHash, 2);
        }

        protected override void OnNormalAttack()
        {
            base.OnNormalAttack();
            
            ProjectileManager.Instance.InstantiateBaseAttack(attack, ProjectileSpawnPosition, Direction, Enum.PoolType.ProjectileBaseAttackWizard);
        }
    }
}