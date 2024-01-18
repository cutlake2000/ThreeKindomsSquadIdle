using Creature.MonsterScripts.MonsterClass;
using Function;
using UnityEngine;

namespace Module
{
    public class AttackCollider : MonoBehaviour
    {
        public BigInteger damage;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(Strings.TAG_ENEMY)) return;

            if (collision.GetComponent<MonsterNew>().TakeDamage(damage)) { }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // if (!collision.CompareTag(Strings.TAG_ENEMY)) return;
            //
            // playerControler.ResetClosestMonster();
        }
    }
}