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
        [Tooltip("기본 관통력")]
        public int BasePenetration;
        [Tooltip("기본 명중률")]
        public int BaseAccuracy;
        [Tooltip("기본 치명타 확률")]
        public int BaseCriticalRate;
        [Tooltip("기본 치명타 데미지")]
        public int BaseCriticalDamage;
        
        // TODO: 골드 획득량 등등 추가 예정
    }
}