using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Warrior
{
    public class PillarOfFire : SkillAttackController
    {
        protected override void ActivateSkill()
        {
            projectileTransform.position = targetPosition;
            gameObject.GetComponent<SpawnCometsScript>().StartVFX();
        }
    }
}