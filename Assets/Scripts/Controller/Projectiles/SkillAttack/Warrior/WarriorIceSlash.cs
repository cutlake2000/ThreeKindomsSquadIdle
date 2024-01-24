using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Warrior
{
    public class WarriorIceSlash : SkillAttackController
    {
        protected override void ActivateSkill()
        {
            projectileTransform.position = startPosition;
            projectileTransform.right = direction * -1;
            
            particle.transform.localRotation = Quaternion.Euler(0, -90, 70.0f * Mathf.Abs(direction.y) + 20.0f);
            
            StartCoroutine(MoveCollider());
        }
        
        protected override void FlipSprite(float directionX)
        {
            var scale = transform.localScale;
            var localScale = new Vector3(Mathf.Abs(scale.x),Mathf.Abs(scale.y), Mathf.Abs(scale.z));
            
            switch (directionX)
            {
                case > 0.1f:
                    localScale = new Vector3(localScale.x, -localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
                case < -0.1f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
            }
        }
    }
}