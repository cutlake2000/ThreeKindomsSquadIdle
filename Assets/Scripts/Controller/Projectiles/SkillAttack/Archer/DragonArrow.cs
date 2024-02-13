using System;
using System.Collections;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Archer
{
    public class DragonArrow : SkillAttackController
    {
        [SerializeField] private float maxDuration;
        [SerializeField] private float currentDuration;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private bool readyToLaunch;

        private void Update()
        {
            if (!readyToLaunch) return;

            currentDuration += Time.deltaTime;

            if (currentDuration > maxDuration) DestroyProjectile();
        }
        
        protected void FixedUpdate()
        {
            if (!readyToLaunch) return;

            transform.position += direction * (projectileSpeed * Time.deltaTime);
        }

        protected override void ActivateSkill()
        {
            projectileTransform.position = startPosition;
            projectileTransform.right = direction * -1;

            readyToLaunch = true;
            gameObject.SetActive(true);
        }
        
        private void DestroyProjectile()
        {
            readyToLaunch = false;
            gameObject.SetActive(false);
        }
    }
}