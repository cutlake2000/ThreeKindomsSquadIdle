using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts
{
    [Serializable]
    public class SquadTalentSo
    {
        [Header("스탯 아이콘")]
        public Sprite squadTalentImage;
        [Header("스탯 이름")]
        public string squadTalentName;
        
        [Header("초기값")]
        public int initTalentValue = 0;
        [Header("레벨 업 초기 비용")]
        public int initialLevelUpCost = 1;
        [Header("레벨 업 비용")]
        public int levelUpCost = 1;

        [Header("스탯 증가 타입")]
        public Data.Enums.SquadStatType statTypeFromSquadTalentPanel;
        [Header("스탯 증가량 타입")]
        public Data.Enums.IncreaseStatValueType increaseTalentValueType;
        [Header("스탯 증가량")]
        public int increaseTalentValue = 1;
    }
}