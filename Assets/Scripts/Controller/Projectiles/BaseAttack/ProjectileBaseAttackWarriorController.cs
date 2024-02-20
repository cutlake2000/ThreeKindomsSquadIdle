using Creature.CreatureClass.MonsterClass;
using Function;
using Keiwando.BigInteger;
using UnityEngine;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileBaseAttackWarriorController : ProjectileController
    {
        public int criticalRate;
        public int criticalDamage;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

            collision.GetComponent<Monster>().TakeDamage(Damage, criticalRate, criticalDamage);
        }

        public void InitializeWarriorBaseAttack(BigInteger damage, Vector3 inputDirection, int criticalRatePercent, int criticalDamagePercent)
        {
            direction = inputDirection;
            FlipLocalScaleXY(direction.x);

            Damage = damage;

            criticalRate = criticalRatePercent;
            criticalDamage = criticalDamagePercent;
            
            transform.right = direction * -1;
        }
    }
}