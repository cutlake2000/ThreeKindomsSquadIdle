using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Archer
{
    public class BloodRain : SkillAttackController
    {
        protected override void ActivateSkill()
        {
            projectileTransform.position = targetPosition;
            gameObject.GetComponent<SpawnCometsScript>().StartVFX();
        }
    }
}