using System.Collections;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Warrior
{
    public class SwordSlash : SkillAttackController
    {
        [Header("파티클 회전 관련")] [SerializeField] protected GameObject particle;

        public WaitForSeconds ParticleStandByTime;
        private int hitCount;

        protected override void ActivateSkill()
        {
            projectileTransform.position = startPosition;
            projectileTransform.right = direction * -1;

            particle.transform.localRotation = Quaternion.Euler(0, -90, 70.0f * Mathf.Abs(direction.y) + 20.0f);

            StartCoroutine(MultiHitCollider());
        }
        
        private IEnumerator MultiHitCollider()
        {
            var currentHitCount = 0;

            while (currentHitCount < 3)
            {
                attackCollider.SetActive(true);
                
                yield return ParticleStandByTime;
                
                attackCollider.SetActive(false);
                
                currentHitCount++;
            }
            
            attackCollider.SetActive(false);
        }
        
        protected override void FlipSprite(float directionX)
        {
            var scale = transform.localScale;
            var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));

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