using System.Collections;
using Controller.Projectiles.BaseAttack;
using Function;
using Module;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controller.Projectiles.SkillAttack
{
    public class ProjectileSkillMoveAttackController : ProjectileSkillAttackController
    {
        [SerializeField] private GameObject particle;
        [SerializeField] private GameObject attackCollider;
        [SerializeField] private float particleCurrentTime = 0.0f;
        [SerializeField] private float particleMaxTime = 0.5f;

        public void InitializeSkillAttack(BigInteger damage, Vector3 startPosition, Vector3 targetPosition)
        {
            Direction = (targetPosition - startPosition).normalized;

            var projectileTransform = transform;
            projectileTransform.position = startPosition;
            projectileTransform.right = Direction * -1;
            
            particle.transform.localRotation = Quaternion.Euler(0, -90, 70.0f * Mathf.Abs(Direction.y) + 20.0f);
            
            attackCollider.GetComponent<AttackCollider>().damage = damage * 2;
            gameObject.GetComponent<ParticleSystem>().Play(true);

            FlipSprite(Direction.x);
            StartCoroutine(MoveCollider());
        }

        public IEnumerator MoveCollider()
         {
             attackCollider.SetActive(true);
             
             while(true)
             {
                 particleCurrentTime += Time.deltaTime;
                 yield return null;

                 if (particleCurrentTime < particleMaxTime)
                 {
                     var newPositionX = -4 * particleCurrentTime / particleMaxTime;
                     var newPosition = new Vector3(newPositionX, 0, 0);
                     attackCollider.transform.localPosition = newPosition;
                 }
                 else
                 {
                     particleCurrentTime = 0;
                     attackCollider.SetActive(false);
                     yield break;
                 }
             }
         }
    }
}