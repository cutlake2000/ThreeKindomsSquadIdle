using Creature.CreatureClass.MonsterClass;
using Data;
using Function;
using Managers.BattleManager;
using Module;
using UnityEngine;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileBaseAttackWizardController : ProjectileController
    {
        [SerializeField] protected float maxDuration;
        [SerializeField] protected float currentDuration;
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected bool readyToLaunch;
        [SerializeField] protected float splashRange;

        [SerializeField] protected Collider2D[] nearbyTargets;
        protected TargetFinder TargetFinder;

        protected virtual void Awake()
        {
            TargetFinder = GetComponent<TargetFinder>();
        }

        protected virtual void Update()
        {
            if (!readyToLaunch) return;

            currentDuration += Time.deltaTime;

            if (currentDuration > maxDuration) DestroyProjectile(transform.position);
        }

        protected void FixedUpdate()
        {
            if (!readyToLaunch) return;

            transform.position += Direction * (projectileSpeed * Time.deltaTime);
        }

        protected virtual void OnEnable()
        {
            nearbyTargets = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

            FindNearbyEnemy();
        }

        public void InitializeWizardBaseAttack(BigInteger damage, Vector3 direction)
        {
            Direction = direction;
            FlipSprite(Direction.x);

            Damage = damage;
            currentDuration = 0;
            transform.right = Direction;
            readyToLaunch = true;
        }

        protected void FindNearbyEnemy()
        {
            if (nearbyTargets != null || splashRange == 0) return;

            nearbyTargets = TargetFinder.ScanNearby(splashRange);

            if (nearbyTargets != null) AttackEnemy();
        }

        protected override void AttackEnemy()
        {
            base.AttackEnemy();

            foreach (var target in nearbyTargets)
            {
                if (target.gameObject.layer != LayerMask.NameToLayer("Enemy")) continue;

                target.GetComponent<Monster>().TakeDamage(Damage);
            }

            DestroyProjectile(transform.position);
        }

        private void DestroyProjectile(Vector3 position)
        {
            EffectManager.Instance.CreateParticlesAtPosition(position, Enums.CharacterType.Wizard, true);
            gameObject.SetActive(false);
        }
    }
}