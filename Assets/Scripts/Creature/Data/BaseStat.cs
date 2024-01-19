using UnityEngine;
using UnityEngine.Serialization;

namespace Creature.Data
{
    public class BaseStat
    {
        [Tooltip("기본 공격력")]
        public int BaseAttack;
        [Tooltip("기본 체력")]
        public int BaseHealth;
        [Tooltip("기본 방어력")]
        public int BaseDefense;
        
        // TODO: 골드 획득량 등등 추가 예정
    }
}