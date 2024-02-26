using System.Collections;
using Module;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Warrior
{
    public class SwordSlash : SkillAttackController
    {
        [Header("파티클 회전 관련")] [SerializeField] protected GameObject particle;

        private readonly WaitForSeconds particleStandByTime = new(0.2f);

        protected override void ActivateSkill()
        {
            foreach (var ac in attackColliders)
            {
                ac.GetComponent<AttackCollider>().Damage = skillDamage * 2;
            }
            
            projectileTransform.position = startPosition;
            projectileTransform.right = direction * -1;

            particle.transform.localRotation = Quaternion.Euler(20, 90, 70.0f * Mathf.Abs(direction.y));

            StartCoroutine(MultiHitCollider());
        }
        
        private IEnumerator MultiHitCollider()
        {
            foreach (var t in attackColliders)
            {
                t.SetActive(true);
                
                yield return particleStandByTime;
            
                t.SetActive(false);
            }
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