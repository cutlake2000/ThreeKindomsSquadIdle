using Function;
using Module;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack
{
    public class ProjectileSkillSpawnAttackController : ProjectileSkillAttackController
    {
        [SerializeField] private GameObject attackCollider;
        
        public void InitializeSkillAttack(BigInteger damage, Vector3 startPosition, Vector3 targetPosition)
        {
            Direction = (targetPosition - startPosition).normalized;

            var projectileTransform = transform;
            projectileTransform.position = targetPosition;
            
            attackCollider.GetComponentInChildren<AttackCollider>().damage = damage * 2;
            
            gameObject.GetComponent<ParticleSystem>().Play(true);
            gameObject.GetComponent<SpawnCometsScript>().StartVFX();

            FlipSprite(Direction.x);
        }
    }
}