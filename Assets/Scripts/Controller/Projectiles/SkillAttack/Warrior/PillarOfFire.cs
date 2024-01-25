using System;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Warrior
{
    public class PillarOfFire : SkillAttackController
    {
        [Header("Projectile 발사 여부")]
        [SerializeField] protected bool readyToLaunch;
        
        protected override void ActivateSkill()
        {
            particleCurrentTime = 0f;
            projectileTransform.position = targetPosition;
            readyToLaunch = true;
            gameObject.GetComponent<ParticleSystem>().Play(true);
        }

        private void Update()
        {
            if (!readyToLaunch) return;

            particleCurrentTime += Time.deltaTime;

            switch (particleCurrentTime)
            {
                case >= 1.5f when attackCollider.activeInHierarchy:
                    attackCollider.SetActive(false);
                    break;
                case >= 0.8f when attackCollider.activeInHierarchy == false:
                    attackCollider.SetActive(true);
                    break;
            }
        }
    }
}