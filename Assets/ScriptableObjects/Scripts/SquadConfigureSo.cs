using System;
using UnityEngine;
using UnityEngine.Serialization;
using Enum = Data.Enum;

namespace ScriptableObjects.Scripts
{
    [Serializable]
    public class SquadConfigureSo
    {
        [Header("캐릭터 이름")]
        public string characterName;
        [Header("캐릭터 아이콘 인덱스")]
        public int characterIconIndex;
        [Header("캐릭터 타입")]
        public Enum.CharacterType characterType;
        [Header("캐릭터 등급")]
        public Enum.CharacterRarity characterRarity;
        
        [Space(5)]
        [Header("캐릭터 모델")]
        public GameObject characterModel;
        
        [Space(5)]
        [Header("캐릭터 스킬")]
        public CharacterSkill[] characterSkills;
    }
    
    [Serializable]
    public struct CharacterSkill
    {
        [Header("스킬 아이콘 인덱스")]
        public int skillIconIndex;
        public GameObject skillObject;
        public string skillDescription;
        public int skillDamagePercent;
    }
}