using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Wizard
{
    public class ThunderTornado : SkillAttackController
    {
        [Header("Projectile MoveSpeed")]
        [SerializeField] private float moveSpeed;
        
        [Header("Projectile 발사 여부")]
        [SerializeField] protected bool readyToLaunch;
        protected override void ActivateSkill()
        {
            particleCurrentTime = 0f;
            projectileTransform.position = startPosition;
            readyToLaunch = true;
        }

        private void Update()
        {
            if (!readyToLaunch) return;
            
            particleCurrentTime += Time.deltaTime;

            if (!(particleCurrentTime > particleMaxTime)) return;
            
            gameObject.GetComponent<ParticleSystem>().Stop(true);
            readyToLaunch = false;
        }

        private void FixedUpdate()
        {
            if (!readyToLaunch) return;

            transform.position += direction * (moveSpeed * Time.deltaTime);
        }
    }
}