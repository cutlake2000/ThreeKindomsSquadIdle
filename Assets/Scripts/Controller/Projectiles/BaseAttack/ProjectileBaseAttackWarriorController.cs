using Creature.CreatureClass.MonsterClass;
using Function;
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
            Direction = direction;
            FlipSprite(Direction.x);

            Damage = damage;

            transform.right = Direction * -1;
        }
    }
}