using Function;
using Keiwando.BigInteger;
using UnityEngine;

namespace Creature.CreatureClass.MonsterClass
{
    public class Monster : Creature
    {
        public virtual void TakeDamage(BigInteger inputDamage, int criticalRate, int criticalDamage){}
    }
}