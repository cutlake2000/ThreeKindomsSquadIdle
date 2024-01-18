using Creature.MonsterScripts.MonsterClass;
using Function;
using Managers;
using UnityEngine;
using Enum = Data.Enum;

namespace Controller.CharacterProjectiles.BaseAttack
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
            
            FindNearbyEnemy();
        }
        
        protected override void AttackEnemy()
        {
            base.AttackEnemy();
            
            foreach (var target in NearbyTargets)
            {
                if (target.gameObject.layer != LayerMask.NameToLayer("Enemy")) continue;
                
                var position = target.transform.position; 
                
                target.GetComponent<MonsterNew>().TakeDamage(Damage);
                
                EffectManager.Instance.CreateParticlesAtPosition(position, Enum.SquadClassType.Warrior, true);
            }
        }
    }
}