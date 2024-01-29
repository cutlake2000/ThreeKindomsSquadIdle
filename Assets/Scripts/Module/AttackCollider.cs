using Creature.CreatureClass.MonsterClass;
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

            collision.GetComponent<Monster>().TakeDamage(damage);
            Debug.Log($"나 {gameObject.name} : 타겟 {collision.name} | damage : {damage}");
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // if (!collision.CompareTag(Strings.TAG_ENEMY)) return;
            //
            // playerControler.ResetClosestMonster();
        }
    }
}