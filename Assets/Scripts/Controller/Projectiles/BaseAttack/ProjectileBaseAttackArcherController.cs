using System;
using Creature.CreatureClass.MonsterClass;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.GameManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileBaseAttackArcherController : ProjectileController
    {
        [SerializeField] protected float maxDuration;
        [SerializeField] protected float currentDuration;
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected bool readyToLaunch;
        [SerializeField] protected bool isHitSomeone;
        
        public int criticalRate;
        public int criticalDamage;

        private void OnDisable()
        {
            transform.position = Vector3.zero;
        }

        private void Update()
        {
            if (!readyToLaunch) return;

            currentDuration += Time.deltaTime;

            if (currentDuration > maxDuration) DestroyProjectile(transform.position);
        }

        private void FixedUpdate()
        {
            if (!readyToLaunch) return;

            transform.position += direction * (projectileSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy") || isHitSomeone) return;
            isHitSomeone = true;
            
            collision.GetComponent<Monster>().TakeDamage(Damage, criticalRate, criticalDamage);
            DestroyProjectile(collision.transform.position);
        }

        public void InitializeArcherBaseAttack(BigInteger damage, Vector3 inputDirection, int criticalRatePercent, int criticalDamagePercent)
        {
            direction = inputDirection;
            FlipLocalScaleY(direction.x);

            Damage = damage;
            
            criticalRate = criticalRatePercent;
            criticalDamage = criticalDamagePercent;
            
            currentDuration = 0;
            transform.right = direction;
            readyToLaunch = true;
            isHitSomeone = false;
        }

        private void DestroyProjectile(Vector3 position)
        {
            gameObject.SetActive(false);
            EffectManager.Instance.CreateParticlesAtPosition(position, Enums.CharacterType.Archer, true);
            gameObject.SetActive(false);
        }
    }
}