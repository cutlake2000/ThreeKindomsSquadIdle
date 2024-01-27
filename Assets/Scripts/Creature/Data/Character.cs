using System.Collections.Generic;
using Controller.UI.BottomMenuUI.SquadPanel.SquadConfigurePanel;
using Managers;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Enum = Data.Enum;
using Image = UnityEngine.UI.Image;

namespace Creature.Data
{
    public class Character : MonoBehaviour
    {
        [Header("ES3 ID")]
        public string characterId;
        [Header("이름")]
        public string characterName;
        [Header("스프라이트 인덱스")]
        public int characterIconIndex;
        [Header("스프라이트")]
        public Image characterIcon;
        [Header("레벨")]
        public int characterLevel;
        [Header("클래스 타입")]
        public Enum.CharacterType characterType;
        [Header("장착 여부")]
        public bool isEquippedCharacter;
        
        [Space(5)]
        [Header("장착 효과")]
        public SquadEffectSo characterEquippedEffect;
        [Header("보유 효과")]
        public SquadEffectSo characterOwnedEffect;
        
        [Space(5)]
        [Header("프리팹 모델")]
        public GameObject characterModel;

        public void SetCharacterInfo(string id, string name, Enum.CharacterType type, Sprite icon, GameObject model, SquadEffectSo equippedEffect, SquadEffectSo ownedEffect)
        {
            characterId = id;
            characterName = name;
            characterType = type;
            characterIcon.sprite = icon;

            characterModel = model;

            characterEquippedEffect = equippedEffect;
            characterOwnedEffect = ownedEffect;

            SetSquadConfigureBaseItemUI();
        }

        private void SetSquadConfigureBaseItemUI()
        {
            gameObject.GetComponent<SquadConfigureItemUI>().UpdateSquadConfigureItemUI(characterLevel, characterName, characterIcon.sprite);
        }

        public void SaveCharacterAllInfo(string characterId)
        {
            ES3.Save("characterId_" + characterId, characterId);
            ES3.Save("characterName_" + characterId, characterName);
            ES3.Save("characterLevel_" + characterId, characterLevel);
            ES3.Save("isEquippedCharacter" + characterId, isEquippedCharacter);
            ES3.Save("characterType_" + characterId, characterType);
            // ES3.Save("rarity_" + characterId, equipmentRarity);
            // ES3.Save("level_"+ characterId, level);
            // ES3.Save("basicEquippedEffect_" + characterId, basicEquippedEffect);
            // ES3.Save("basicOwnedEffect_" + characterId, basicOwnedEffect);
            // ES3.Save("equippedEffect_" + characterId, equippedEffect);
            // ES3.Save("ownedEffect_" + characterId, ownedEffect);
        }
    }
}