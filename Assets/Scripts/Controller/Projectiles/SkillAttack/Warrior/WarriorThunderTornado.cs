using System;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Warrior
{
    public class WarriorThunderTornado : SkillAttackController
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
            
            if (particleCurrentTime > particleMaxTime)
            {
                readyToLaunch = false;
                gameObject.GetComponent<ParticleSystem>().Stop(true);
            }
        }

        private void FixedUpdate()
        {
            if (!readyToLaunch) return;

            transform.position += direction * (moveSpeed * Time.deltaTime);
        }
    }
}