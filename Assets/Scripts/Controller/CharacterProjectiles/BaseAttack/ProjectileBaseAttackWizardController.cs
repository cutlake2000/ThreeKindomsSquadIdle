using Creature.MonsterScripts.MonsterClass;
using Data;
using Function;
using Managers;
using UnityEngine;

namespace Controller.CharacterProjectiles.BaseAttack
{
    public class ProjectileBaseAttackWizardController : ProjectileBaseRangedAttackController
    {
        public void InitializeWizardBaseAttack(BigInteger damage, Vector3 direction)
        {
            Direction = direction;
            FlipSprite(Direction.x);

            Damage = damage;
            CurrentDuration = 0;
            transform.right = Direction;
            ReadyToLaunch = true;
        }
        
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
            
            FindNearbyEnemy();
        }
        
        protected override void AttackEnemy()
        {
            base.AttackEnemy();
            
            foreach (var target in NearbyTargets)
            {
                if (target.gameObject.layer != LayerMask.NameToLayer("Enemy")) continue;
                
                target.GetComponent<MonsterNew>().TakeDamage(Damage);
            }
            
            DestroyProjectile(transform.position);
        }
        
        protected override void DestroyProjectile(Vector3 position)
        {
            EffectManager.Instance.CreateParticlesAtPosition(position, Enum.SquadClassType.Wizard, true);
            gameObject.SetActive(false);
        }
    }
}