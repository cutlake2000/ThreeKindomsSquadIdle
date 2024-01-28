using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts
{
    [Serializable]
    public class SquadStatSo
    {
        [Header("스탯 아이콘")]
        public Sprite squadStatImage;
        [Header("스탯 이름")]
        public string squadStatName;
        
        [Header("초기값")]
        public int initStatValue = 0;
        [Header("레벨 업 비용")]
        public int levelUpCost = 1;

        [FormerlySerializedAs("squadStatTypeBySquadPanel")] [Header("스탯 증가 타입")]
        public Data.Enum.SquadStatTypeBySquadStatPanel squadStatTypeBySquadStatPanel;
        [Header("스탯 증가량 타입")]
        public Data.Enum.IncreaseStatValueType increaseStatValueType;
        [Header("스탯 증가량")]
        public int increaseStatValue;
    }
}