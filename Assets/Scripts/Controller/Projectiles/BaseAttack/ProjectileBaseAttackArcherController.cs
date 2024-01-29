using Creature.CreatureClass.MonsterClass;
using Data;
using Function;
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

        private void Update()
        {
            if (!readyToLaunch) return;

            currentDuration += Time.deltaTime;

            if (currentDuration > maxDuration) DestroyProjectile(transform.position);
        }

        private void FixedUpdate()
        {
            if (!readyToLaunch) return;

            transform.position += Direction * (projectileSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

            AttackEnemy(collision);
            DestroyProjectile(collision.transform.position);
        }

        public void InitializeArcherBaseAttack(BigInteger damage, Vector3 direction)
        {
            Direction = direction;
            FlipSprite(Direction.x);

            Damage = damage;
            currentDuration = 0;
            transform.right = Direction;
            readyToLaunch = true;
        }

        protected override void AttackEnemy(Collider2D collision)
        {
            base.AttackEnemy(collision);

            collision.GetComponent<Monster>().TakeDamage(Damage);
        }

        private void DestroyProjectile(Vector3 position)
        {
            gameObject.SetActive(false);
            EffectManager.Instance.CreateParticlesAtPosition(position, Enum.CharacterType.Archer, true);
            gameObject.SetActive(false);
        }
    }
}