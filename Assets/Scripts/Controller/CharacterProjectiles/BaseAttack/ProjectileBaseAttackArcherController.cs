using Creature.CreatureClass;
using Creature.CreatureClass.MonsterClass;
using Data;
using Function;
using Managers;
using UnityEngine;

namespace Controller.CharacterProjectiles.BaseAttack
{
    public class ProjectileBaseAttackArcherController : ProjectileBaseRangedAttackController
    {
        public void InitializeArcherBaseAttack(BigInteger damage, Vector3 direction)
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
                
            AttackEnemy(collision);
            DestroyProjectile(collision.transform.position);
        }
        
        protected override void AttackEnemy(Collider2D collision)
        {
            base.AttackEnemy(collision);
            
            collision.GetComponent<MonsterNew>().TakeDamage(Damage);
        }
        
        protected override void DestroyProjectile(Vector3 position)
        {
            EffectManager.Instance.CreateParticlesAtPosition(position, Enum.SquadClassType.Archer, true);
            gameObject.SetActive(false);
        }
    }
}