using Creature.CreatureClass.MonsterClass;
using Data;
using Function;
using Managers;
using Module;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileBaseAttackWizardController : ProjectileController
    {
        protected TargetFinder TargetFinder;
        
        [SerializeField] protected float maxDuration;
        [SerializeField] protected float currentDuration;
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected bool readyToLaunch;
        [SerializeField] protected float splashRange;
        
        [SerializeField] protected Collider2D[] nearbyTargets;
        
        public void InitializeWizardBaseAttack(BigInteger damage, Vector3 direction)
        {
            Direction = direction;
            FlipSprite(Direction.x);

            Damage = damage;
            currentDuration = 0;
            transform.right = Direction;
            readyToLaunch = true;
        }
        
        protected virtual void Awake()
        {
            TargetFinder = GetComponent<TargetFinder>();
        }

        protected virtual void OnEnable()
        {
            nearbyTargets = null;
        }
        
        protected virtual void Update()
        {
            if (!readyToLaunch) return;
            
            currentDuration += Time.deltaTime;
            
            if (currentDuration > maxDuration)
            {
                DestroyProjectile(transform.position);
            }
        }
        
        protected void FixedUpdate()
        {
            if (!readyToLaunch) return;

            transform.position += Direction * (projectileSpeed * Time.deltaTime);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
            
            FindNearbyEnemy();
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
            EffectManager.Instance.CreateParticlesAtPosition(position, Enum.CharacterType.Wizard, true);
            gameObject.SetActive(false);
        }
    }
}