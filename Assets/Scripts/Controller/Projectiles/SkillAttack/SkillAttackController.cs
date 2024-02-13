using System.Collections;
using Data;
using Function;
using Module;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack
{
    public class SkillAttackController : MonoBehaviour
    {
        [Header("스킬 타입")] public Enums.SkillType skillType;

        [Header("파티클 Direction")] [SerializeField]
        protected Vector3 direction;

        [Header("파티클 Transform")] [SerializeField]
        protected Transform projectileTransform;

        [Header("AttackCollider")] [SerializeField]
        protected GameObject attackCollider;

        [Header("AttackCollider 이동 관련 파리미터")] [SerializeField]
        protected float particleCurrentTime;

        [SerializeField] protected float particleMaxTime = 0.5f;

        [Header("InitializeSkillAttackData")] [SerializeField]
        protected Vector3 startPosition;

        [SerializeField] protected Vector3 targetPosition;
        protected BigInteger skillDamage;

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

        protected virtual void ActivateSkill()
        {
        }

        protected virtual void FlipSprite(float directionX)
        {
        }
    }
}