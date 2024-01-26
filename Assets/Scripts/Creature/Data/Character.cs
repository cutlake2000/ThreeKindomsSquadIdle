using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Serialization;
using Enum = Data.Enum;

namespace Creature.Data
{
    public class Character : MonoBehaviour
    {
        [Header("ES3 ID")]
        public string characterId;
        [Header("이름")]
        public string characterName;
        [FormerlySerializedAs("characterClassType")] [Header("클래스 타입")]
        public Enum.CharacterType characterType;
        [Header("스프라이트")]
        public Image characterIcon;
        
        [Space(5)]
        [Header("프리팹 모델")]
        public GameObject model;
    }
}