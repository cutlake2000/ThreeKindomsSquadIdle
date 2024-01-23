using Controller.CharacterProjectiles.BaseAttack;
using Creature.CreatureClass.MonsterClass;
using Function;
using Managers;
using UnityEngine;
using Enum = Data.Enum;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileBaseAttackWarriorController : ProjectileController
    {
        public void InitializeWarriorBaseAttack(BigInteger damage, Vector3 direction)
        {
            Direction = direction;
            FlipSprite(Direction.x);

            Damage = damage;
            CurrentDuration = 0;
            
            transform.right = Direction * -1;
        }
        
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
            
            collision.GetComponent<Monster>().TakeDamage(Damage);
        }
        
        // protected override void AttackEnemy()
        // {
        //     base.AttackEnemy();
        //     
        //     foreach (var target in NearbyTargets)
        //     {
        //         if (target.gameObject.layer != LayerMask.NameToLayer("Enemy")) continue;
        //         target.GetComponent<Monster>().TakeDamage(Damage);
        //         
        //         // var position = target.transform.position; 
        //         // EffectManager.Instance.CreateParticlesAtPosition(position, Enum.SquadClassType.Warrior, true);
        //     }
        // }
    }
}