using Creature.CreatureClass.SquadClass;
using Function;
using Keiwando.BigInteger;
using UnityEngine;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileBaseAttackMonsterController : ProjectileController
    {
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;

            if (collision.GetComponent<Squad>().isDead == false)
            {
                collision.GetComponent<Squad>().TakeDamage(Damage);
            }
            gameObject.SetActive(false);
        }

        public void InitializeMonsterBaseAttack(BigInteger damage, Vector3 direction)
        {
            base.direction = direction;
            FlipLocalScaleY(base.direction.x);

            Damage = damage;
            // Debug.Log($"몬스터 데미지 : {Damage}");

            transform.right = base.direction * -1;
        }
    }
}