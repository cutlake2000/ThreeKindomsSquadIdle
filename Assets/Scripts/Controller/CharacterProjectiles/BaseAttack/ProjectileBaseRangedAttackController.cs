using UnityEngine;

namespace Controller.CharacterProjectiles.BaseAttack
{
    public class ProjectileBaseRangedAttackController : ProjectileController
    {
        [SerializeField] protected float projectileSpeed;
        
        protected virtual void Update()
        {
            if (!ReadyToLaunch) return;
            
            CurrentDuration += Time.deltaTime;
            
            if (CurrentDuration > duration)
            {
                DestroyProjectile(transform.position);
            }
        }
        
        protected void FixedUpdate()
        {
            if (!ReadyToLaunch) return;

            transform.position += Direction * (projectileSpeed * Time.deltaTime);
        }
        
        protected virtual void DestroyProjectile(Vector3 position)
        {
            gameObject.SetActive(false);
        }
    }
}