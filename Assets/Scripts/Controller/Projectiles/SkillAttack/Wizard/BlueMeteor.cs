using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Wizard
{
    public class BlueMeteor : SkillAttackController
    {
        protected override void ActivateSkill()
        {
            projectileTransform.position = targetPosition;
            gameObject.GetComponent<SpawnCometsScript>().StartVFX();
        }
    }
}