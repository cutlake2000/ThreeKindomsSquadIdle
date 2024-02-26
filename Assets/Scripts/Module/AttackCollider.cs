using Creature.CreatureClass.MonsterClass;
using Function;
using Keiwando.BigInteger;
using UnityEngine;

namespace Module
{
    public class AttackCollider : MonoBehaviour
    {
        public BigInteger Damage;
        public int criticalRate;
        public int criticalDamage;

        public void InitializeAttackColliderData(BigInteger damage, int criticalRate, int criticalDamage)
        {
            Damage = damage;
            this.criticalRate = criticalRate;
            this.criticalDamage = criticalDamage;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(Strings.TAG_ENEMY)) return;

            collision.GetComponent<Monster>().TakeDamage(Damage, criticalRate, criticalDamage);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // if (!collision.CompareTag(Strings.TAG_ENEMY)) return;
            //
            // playerControler.ResetClosestMonster();
        }
    }
}