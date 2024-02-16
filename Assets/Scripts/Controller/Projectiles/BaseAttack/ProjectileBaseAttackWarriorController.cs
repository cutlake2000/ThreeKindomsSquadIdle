using Creature.CreatureClass.MonsterClass;
using Function;
using Keiwando.BigInteger;
using UnityEngine;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileBaseAttackWarriorController : ProjectileController
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

            collision.GetComponent<Monster>().TakeDamage(Damage);
        }

        public void InitializeWarriorBaseAttack(BigInteger damage, Vector3 direction)
        {
            this.direction = direction;
            FlipLocalScaleXY(this.direction.x);

            Damage = damage;

            transform.right = this.direction * -1;
        }
    }
}