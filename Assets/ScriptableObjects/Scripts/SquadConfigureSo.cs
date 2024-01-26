using System;
using UnityEngine;
using Enum = Data.Enum;

namespace ScriptableObjects.Scripts
{
    [Serializable]
    public class SquadConfigureSo
    {
        [Header("캐릭터 이름")]
        public string characterName;
        [Header("캐릭터 아이콘")]
        public Sprite characterIcon;
        [Header("캐릭터 타입")]
        public Enum.CharacterType characterType;
        [Header("캐릭터 등급")]
        public Enum.CharacterRarity characterRarity;

   

        [Space(5)]
        [Header("캐릭터 모델")]
        public GameObject model;
        
        [Space(5)]
        [Header("캐릭터 스킬")]
        public CharacterSkill[] characterSkills;
    }
    
    [Serializable]
    public struct CharacterSkill
    {
        public Sprite skillIcon;
        public GameObject skillObject;
        public string skillDescription;
        public int skillDamagePercent;
    }
}