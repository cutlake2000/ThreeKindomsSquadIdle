using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Wizard
{
    public class WizardBlueMeteor : SkillAttackController
    {
        protected override void ActivateSkill()
        {
            projectileTransform.position = targetPosition;
            gameObject.GetComponent<SpawnCometsScript>().StartVFX();
        }
        
        protected override void FlipSprite(float directionX)
        {
            var scale = transform.localScale;
            var localScale = new Vector3(Mathf.Abs(scale.x),Mathf.Abs(scale.y), Mathf.Abs(scale.z));
            
            switch (directionX)
            {
                case > 0.1f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
                case < -0.1f:
                    localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
            }
        }
    }
}