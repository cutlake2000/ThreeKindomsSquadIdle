using System.Collections;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Warrior
{
    public class IceSlash : SkillAttackController
    {
        [Header("파티클 회전 관련")] [SerializeField] protected GameObject particle;

        protected override void ActivateSkill()
        {
            projectileTransform.position = startPosition;
            projectileTransform.right = direction * -1;

            particle.transform.localRotation = Quaternion.Euler(0, -90, 70.0f * Mathf.Abs(direction.y) + 20.0f);

            StartCoroutine(MoveCollider());
        }
        
        private IEnumerator MoveCollider()
        {
            attackColliders[0].SetActive(true);

            while (true)
            {
                particleCurrentTime += Time.deltaTime;
                yield return null;

                if (particleCurrentTime < particleMaxTime)
                {
                    var newPositionX = -4 * particleCurrentTime / particleMaxTime;
                    var newPosition = new Vector3(newPositionX, 0, 0);
                    attackColliders[0].transform.localPosition = newPosition;
                }
                else
                {
                    particleCurrentTime = 0;
                    attackColliders[0].SetActive(false);
                    yield break;
                }
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