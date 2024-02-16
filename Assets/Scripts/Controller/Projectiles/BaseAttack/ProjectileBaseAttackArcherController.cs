using System;
using Creature.CreatureClass.MonsterClass;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers.BattleManager;
using UnityEngine;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileBaseAttackArcherController : ProjectileController
    {
        [SerializeField] protected float maxDuration;
        [SerializeField] protected float currentDuration;
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected bool readyToLaunch;

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
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

            collision.GetComponent<Monster>().TakeDamage(Damage);
            DestroyProjectile(collision.transform.position);
        }

        public void InitializeArcherBaseAttack(BigInteger damage, Vector3 direction)
        {
            base.direction = direction;
            FlipLocalScaleY(base.direction.x);

            Damage = damage;
            currentDuration = 0;
            transform.right = base.direction;
            readyToLaunch = true;
        }

        private void DestroyProjectile(Vector3 position)
        {
            gameObject.SetActive(false);
            EffectManager.Instance.CreateParticlesAtPosition(position, Enums.CharacterType.Archer, true);
            gameObject.SetActive(false);
        }
    }
}