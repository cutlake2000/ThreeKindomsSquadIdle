using Creature.CreatureClass.SquadClass;
using Function;
using Module;
using UnityEngine;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileBaseAttackMonsterController : ProjectileController
    {
        public void InitializeMonsterBaseAttack(BigInteger damage, Vector3 direction)
        {
            Direction = direction;
            FlipSprite(Direction.x);

            Damage = damage;
            Debug.Log($"몬스터 데미지 : {Damage}");
            CurrentDuration = 0;
            
            transform.right = Direction * -1;
        }
        
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            
            collision.GetComponent<Squad>().TakeDamage(Damage);
            gameObject.SetActive(false);
        }
    }
}