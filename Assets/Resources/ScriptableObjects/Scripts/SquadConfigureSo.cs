using System;
using Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        public Enums.CharacterType characterType;
        [Header("캐릭터 등급")]
        public Enums.CharacterRarity characterRarity;

        [Space(5)]
        [Header("캐릭터 모델")]
        public int characterModelIndex;
        
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
        public string skillName;
        public string skillDescription;
        public int skillDamagePercent;
        public int maxSkillCoolTime;
    }
}