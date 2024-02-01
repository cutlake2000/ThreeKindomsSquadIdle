using Externals.SkillEffect_Set.GabrielAguiarProductions.Unique_MagicAbilities_Volume_1.Scripts.UniqueMagicAbilities;

namespace Controller.Projectiles.SkillAttack.Wizard
{
    public class BlueMeteor : SkillAttackController
    {
        protected override void ActivateSkill()
        {
            projectileTransform.position = targetPosition;
            gameObject.GetComponent<SpawnCometsScript>().StartVFX(skillDamage);
        }
    }
}