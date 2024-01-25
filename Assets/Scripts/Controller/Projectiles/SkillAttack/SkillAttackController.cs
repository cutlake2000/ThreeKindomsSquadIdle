using System.Collections;
using Data;
using Function;
using Module;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controller.Projectiles.SkillAttack
{
    public class SkillAttackController : MonoBehaviour
    {
        private BigInteger skillDamage;
        
        [Header("스킬 타입")]
        public Enum.SkillType skillType;
        
        [Header("파티클 Direction")]
        [SerializeField] protected Vector3 direction;

        [Header("파티클 Transform")]
        [SerializeField] protected Transform projectileTransform;
        
        [Header("AttackCollider")]
        [SerializeField] protected GameObject attackCollider;
        
        [Header("AttackCollider 이동 관련 파리미터")]
        [SerializeField] protected float particleCurrentTime;
        [SerializeField] protected float particleMaxTime = 0.5f;
        
        [Header("InitializeSkillAttackData")]
        [SerializeField] protected Vector3 startPosition;
        [SerializeField] protected Vector3 targetPosition;

        public void InitializeSkillAttack(BigInteger damage, Vector3 start, Vector3 target)
        {
            direction = (target - start).normalized;
            skillDamage = damage;
            
            projectileTransform = transform;

            startPosition = start;
            targetPosition = target;
            
            attackCollider.GetComponent<AttackCollider>().damage = skillDamage * 2;
            gameObject.GetComponent<ParticleSystem>().Play(true);

            FlipSprite(direction.x);

            ActivateSkill();
        }
        
        protected IEnumerator MoveCollider()
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
        
        protected virtual void ActivateSkill() { }
        
        protected virtual void FlipSprite(float directionX) { }
    }
}