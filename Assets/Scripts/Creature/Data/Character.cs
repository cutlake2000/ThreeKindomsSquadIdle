using System;
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
    [Serializable]
    public class Character
    {
        [Header("ES3 ID")]
        public string CharacterId;
        [Header("이름")]
        public string CharacterName;
        [Header("스프라이트 인덱스")]
        public int CharacterIconIndex;
        [Header("레벨")]
        public int CharacterLevel;
        [Header("클래스 타입")]
        public Enum.CharacterType CharacterType;
        [Header("클래스 등급")]
        public Enum.CharacterRarity CharacterRarity;
        [Header("장착 여부")]
        public bool IsEquipped;
        [Header("보유 여부")]
        public bool IsPossessed;

        [field: Space(5)]
        [field: Header("장착 효과")]
        public SquadEffectSo CharacterEquippedEffect;
        [Header("보유 효과")]
        public SquadEffectSo CharacterOwnedEffect;

        [Space(5)]
        [Header("프리팹 모델")]
        public int CharacterModelIndex;
        public GameObject CharacterModel;

        public Character (string id, string name, int level, bool isEquipped, bool isPossessed, Enum.CharacterType type, int iconIndex, Sprite icon, Enum.CharacterRarity rarity, int modelIndex, GameObject model, SquadEffectSo equippedEffect, SquadEffectSo ownedEffect)
        {
            CharacterId = id;
            CharacterName = name;
            CharacterLevel = level;
            IsEquipped = isEquipped;
            IsPossessed = isPossessed;
            CharacterType = type;
            CharacterIconIndex = iconIndex;
            CharacterRarity = rarity;

            CharacterModelIndex = modelIndex;
            CharacterModel = model;

            CharacterEquippedEffect = equippedEffect;
            CharacterOwnedEffect = ownedEffect;

            SaveCharacterAllInfo();
        }

        public Character (string characterId)
        {
            LoadCharacterAllInfo(characterId);
        }

        public void LoadCharacterAllInfo(string characterId)
        {
            if (!ES3.KeyExists("id_" + characterId)) return;
            
            CharacterId = ES3.Load<string>("characterId_" + characterId);
            CharacterName = ES3.Load<string>("characterName_" + characterId);
            CharacterLevel = ES3.Load<int>("characterLevel_" + characterId);
            IsEquipped = ES3.Load<bool>("isEquipped_" + characterId);
            IsPossessed = ES3.Load<bool>("isPossessed_" + characterId);
            CharacterType = ES3.Load<Enum.CharacterType>("characterType_" + characterId);
            CharacterIconIndex = ES3.Load<int>("characterIconIndex_" + characterId);
            CharacterRarity = ES3.Load<Enum.CharacterRarity>("characterRarity_" + characterId);

            CharacterModelIndex = ES3.Load<int>("characterName_" + characterId);
            CharacterModel = ES3.Load<GameObject>("characterName_" + characterId);

            CharacterEquippedEffect = ES3.Load<SquadEffectSo>("characterName_" + characterId);
            CharacterOwnedEffect = ES3.Load<SquadEffectSo>("characterName_" + characterId);
        }

        public void SaveCharacterAllInfo()
        {
            ES3.Save("characterId_" + CharacterId, CharacterId);
            ES3.Save("characterName_" + CharacterId, CharacterName);
            ES3.Save("characterLevel_" + CharacterId, CharacterLevel);
            ES3.Save("isEquipped_" + CharacterId, IsEquipped);
            ES3.Save("isPossessed_" + CharacterId, IsPossessed);
            ES3.Save("characterType_" + CharacterId, CharacterType);
            ES3.Save("characterIconIndex_" + CharacterId, CharacterIconIndex);
            ES3.Save("characterRarity_" + CharacterId, CharacterRarity);
            
            ES3.Save("characterModelIndex_" + CharacterId, CharacterModelIndex);
            ES3.Save("characterModel_" + CharacterId, CharacterModel);
            
            ES3.Save("characterEquippedEffect_" + CharacterId, CharacterEquippedEffect);
            ES3.Save("characterOwnedEffect_" + CharacterId, CharacterOwnedEffect);
        }
    }
}