using System;
using System.Collections.Generic;
using Creature.Data;
using Module;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Managers
{
    public class SquadConfigureManager : MonoBehaviour
    {
        public static SquadConfigureManager Instance;
        
        //TODO: 임시 So 대체 클래스 -> 추후 csv, json으로 대체
        [SerializeField] private SquadConfigureSo[] squadConfigureSo;
        [SerializeField] private SquadEffectSo[] squadOwnedEffectValueSoByRarity;
        [SerializeField] private SquadEffectSo[] squadEquippedEffectValueSoByRarity;
        
        [Header("캐릭터 정보 컨테이너")]
        private static readonly Dictionary<string, Character> AllCharacters = new();
        public List<Character> warriors = new();
        public List<Character> archers = new();
        public List<Character> wizards = new();
        
        [Header("캐릭터 스프라이트")]
        public List<Sprite> warriorSprites = new();
        public List<Sprite> archerSprites = new();
        public List<Sprite> wizardSprites = new();

        private void Awake()
        {
            Instance = this;
        }

        public void InitSquadConfigureManager()
        {
            SetAllCharacters();
        }

        private void SetAllCharacters()
        {
            if (ES3.KeyExists("Init_Game"))
            {
                LoadAllCharacters();
            }
            else
            {
                CreateAllCharacters();
            }
        }
        
        private void LoadAllCharacters()
        {
            throw new NotImplementedException();
        }

        private void CreateAllCharacters()
        {
            foreach (var characterType in Enum.characterTypes)
            {
                var characterIndex = 0;

                foreach (var characterSo in squadConfigureSo)
                {
                    if (characterType != characterSo.characterType) continue;
                    
                    var character = characterType switch
                    {
                        Enum.CharacterType.Warrior => warriors[characterIndex],
                        Enum.CharacterType.Archer => archers[characterIndex],
                        Enum.CharacterType.Wizard => wizards[characterIndex],
                        _ => null
                    };
                    
                    if (character == null) continue;

                    var characterId = $"{characterSo.characterName}";
                    var characterName = characterSo.characterName;
                    var characterIcon = characterType switch
                    {
                        Enum.CharacterType.Warrior => SpriteManager.Instance.warriorSprite[characterSo.characterIconIndex],
                        Enum.CharacterType.Archer => SpriteManager.Instance.archerSprite[characterSo.characterIconIndex],
                        Enum.CharacterType.Wizard => SpriteManager.Instance.wizardSprite[characterSo.characterIconIndex],
                        _ => null
                    };
                    
                    var characterModel = characterSo.characterModel;
                    var equippedEffect = squadEquippedEffectValueSoByRarity[(int)characterSo.characterRarity];
                    var ownedEffect = squadOwnedEffectValueSoByRarity[(int)characterSo.characterRarity];

                    character.SetCharacterInfo(characterId, characterName, characterType, characterIcon, characterModel, equippedEffect, ownedEffect);
                    // character.GetComponent<Button>().onClick.AddListener(); //TODO: 캐릭터 클릭 시 뚜돵돵 되는 거

                    AddCharacter(characterId, character);
                    character.SaveCharacterAllInfo(characterId);

                    // if (isEquipped)
                    // {
                    //     
                    // }

                    characterIndex++;
                    
                    InfiniteLoopDetector.Run();
                }
            }
        }

        private static void AddCharacter(string characterId, Character character)
        {
            if (!AllCharacters.TryAdd(characterId, character))
            {
                Debug.LogWarning($"Character already exists in the dictionary: {character}");
            }
        }
    }
}