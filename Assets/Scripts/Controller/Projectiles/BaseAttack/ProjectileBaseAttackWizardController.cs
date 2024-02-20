using Creature.CreatureClass.MonsterClass;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.GameManager;
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
        private TargetFinder targetFinder;
        
        public int criticalRate;
        public int criticalDamage;

        protected void Awake()
        {
            targetFinder = GetComponent<TargetFinder>();
        }
        
        private void OnDisable()
        {
            transform.position = Vector3.zero;
        }

        protected void Update()
        {
            if (!readyToLaunch) return;

            currentDuration += Time.deltaTime;

            if (currentDuration > maxDuration) DestroyProjectile(transform.position);
        }

        protected void FixedUpdate()
        {
            if (!readyToLaunch) return;

            transform.position += direction * (projectileSpeed * Time.deltaTime);
        }

        protected void OnEnable()
        {
            nearbyTargets = null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

            FindNearbyEnemy();
        }

        public void InitializeWizardBaseAttack(BigInteger damage, Vector3 inputDirection, int criticalRatePercent, int criticalDamagePercent)
        {
            direction = inputDirection;
            FlipLocalScaleY(direction.x);

            Damage = damage;
            
            criticalRate = criticalRatePercent;
            criticalDamage = criticalDamagePercent;
            
            currentDuration = 0;
            transform.right = direction;
            readyToLaunch = true;
        }

        private void FindNearbyEnemy()
        {
            if (nearbyTargets != null || splashRange == 0) return;

            nearbyTargets = targetFinder.ScanNearby(splashRange);

            if (nearbyTargets != null) AttackEnemy();
        }

        protected override void AttackEnemy()
        {
            base.AttackEnemy();

            foreach (var target in nearbyTargets)
            {
                if (target.gameObject.layer != LayerMask.NameToLayer("Enemy")) continue;

                target.GetComponent<Monster>().TakeDamage(Damage, criticalRate, criticalDamage);
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