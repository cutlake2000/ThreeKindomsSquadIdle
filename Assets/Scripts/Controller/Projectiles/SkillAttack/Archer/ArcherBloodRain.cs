using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Archer
{
    public class ArcherBloodRain : SkillAttackController
    {
        protected override void ActivateSkill()
        {
            projectileTransform.position = targetPosition;
            gameObject.GetComponent<SpawnCometsScript>().StartVFX();
        }
    }
}