using Data;
using Managers;

namespace Creature.CreatureClass.SquadClass
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
        
        protected override void OnSkillAttack1()
        {
            base.OnSkillAttack1();
            
            for (var i = 0; i < SquadManager.Instance.wizardSkillCoolTimer.Length; i++)
            {
                if (!SquadManager.Instance.wizardSkillCoolTimer[i].isSkillReady) continue;
                SquadManager.Instance.RunSkillCoolTimer(Enum.SquadClassType.Wizard, i);
                ProjectileManager.Instance.InstantiateSkillAttack(attack, ProjectileSpawnPosition, Direction,
                    Enum.PoolType.ProjectileSkillAttackWizard, i);
            }
        }
    }
}