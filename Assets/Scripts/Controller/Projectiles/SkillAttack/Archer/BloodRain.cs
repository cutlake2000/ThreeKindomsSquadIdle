using Externals.SkillEffect_Set.GabrielAguiarProductions.Unique_MagicAbilities_Volume_1.Scripts.UniqueMagicAbilities;
using Function;

namespace Controller.Projectiles.SkillAttack.Archer
{
    public class BloodRain : SkillAttackController
    {
        protected override void ActivateSkill()
        {
            projectileTransform.position = targetPosition;
            gameObject.GetComponent<SpawnCometsScript>().StartVFX(skillDamage);
        }
    }
}