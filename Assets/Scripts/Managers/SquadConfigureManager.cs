using System;
using System.Collections.Generic;
using System.Linq;
using Controller.UI.BottomMenuUI.SquadPanel.SquadConfigurePanel;
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
        public List<Character> Warriors = new ();
        public List<Character> Archers = new ();
        public List<Character> Wizards = new ();
        private static readonly Dictionary<string, Character> AllCharacters = new();
        public Dictionary<string, Character> WarriorDictionary = new ();
        public Dictionary<string, Character> ArchersDictionary = new ();
        public Dictionary<string, Character> WizardsDictionary = new ();

        [Header("캐릭터 모델 컨테이너")]
        public List<GameObject> warriorModels = new();
        public List<GameObject> archerModels = new();
        public List<GameObject> wizardModels = new();

        [Header("캐릭터 모델 스폰 좌표")]
        public GameObject[] modelSpawnObjects;
        
        [Header("스쿼드 구성 패널 아이템 UI")]
        public List<GameObject> SquadConfigureScrollViewItemWarriors = new();
        public List<GameObject> SquadConfigureScrollViewItemArchers = new();
        public List<GameObject> SquadConfigureScrollViewItemWizards = new();

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
            foreach (var characterType in Enum.characterTypes)
            {
                foreach (var characterSo in squadConfigureSo)
                {
                    if (characterType != characterSo.characterType) continue;
                    
                    var character = new Character(characterSo.characterName);
                    AddCharacter(characterSo.characterName, character);
                    
                    InfiniteLoopDetector.Run();
                }
                
                UpdateSquadConfigureScrollViewItemUI(characterType);
            }
        }

        private void CreateAllCharacters()
        {
            foreach (var characterType in Enum.characterTypes)
            {
                var characterIndex = 0;
                
                foreach (var characterSo in squadConfigureSo)
                {
                    if (characterType != characterSo.characterType) continue;

                    var characterId = $"{characterSo.characterName}";
                    var characterName = characterSo.characterName;
                    const int characterLevel = 1;
                    var isEquipped = characterIndex == 1;
                    var isPossessed = characterIndex == 1;
                    var characterIconIndex = characterSo.characterIconIndex;
                    var characterIcon = SpriteManager.Instance.GetCharacterSprite(characterType, characterSo.characterIconIndex);
                    var characterRarity = characterSo.characterRarity;
                    var characterModelIndex = characterSo.characterModelIndex;
                    var characterModel = warriorModels[characterModelIndex];
                    var equippedEffect = squadEquippedEffectValueSoByRarity[(int)characterSo.characterRarity];
                    var ownedEffect = squadOwnedEffectValueSoByRarity[(int)characterSo.characterRarity];
                    
                    var character = new Character(characterId, characterName, characterLevel, isEquipped, isPossessed, characterType, characterIconIndex, characterIcon, characterRarity, characterModelIndex, characterModel, equippedEffect, ownedEffect);
                    AddCharacter(characterId, character);
                    characterIndex++;
                    
                    InfiniteLoopDetector.Run();
                }
                
                UpdateSquadConfigureScrollViewItemUI(characterType);
            }
        }
        
        private void UpdateSquadConfigureScrollViewItemUI(Enum.CharacterType characterType)
        {
            switch (characterType)
            {
                case Enum.CharacterType.Warrior:
                    Warriors = WarriorDictionary.Values.OrderByDescending(x => x.IsEquipped).ThenBy(x => x.CharacterRarity).ToList();
                    for (var i = 0 ; i < Warriors.Count ; i++)
                    {
                        SquadConfigureScrollViewItemWarriors[i].GetComponent<SquadConfigureItemUI>().UpdateSquadConfigureItemUI(Warriors[i].CharacterLevel, Warriors[i].IsEquipped, Warriors[i].IsPossessed, Warriors[i].CharacterName, SpriteManager.Instance.GetCharacterSprite(characterType, Warriors[i].CharacterIconIndex));
                    }
                    break;
                case Enum.CharacterType.Archer:
                    Archers = ArchersDictionary.Values.OrderByDescending(x => x.IsEquipped).ThenBy(x => x.CharacterRarity).ToList();
                    for (var i = 0 ; i < Archers.Count ; i++)
                    {
                        SquadConfigureScrollViewItemArchers[i].GetComponent<SquadConfigureItemUI>().UpdateSquadConfigureItemUI(Archers[i].CharacterLevel, Archers[i].IsEquipped, Archers[i].IsPossessed, Archers[i].CharacterName, SpriteManager.Instance.GetCharacterSprite(characterType, Archers[i].CharacterIconIndex));
                    }
                    break;
                case Enum.CharacterType.Wizard:
                    Wizards = WizardsDictionary.Values.OrderByDescending(x => x.IsEquipped).ThenBy(x => x.CharacterRarity).ToList();
                    for (var i = 0 ; i < Wizards.Count ; i++)
                    {
                        SquadConfigureScrollViewItemWizards[i].GetComponent<SquadConfigureItemUI>().UpdateSquadConfigureItemUI(Wizards[i].CharacterLevel, Wizards[i].IsEquipped, Wizards[i].IsPossessed, Wizards[i].CharacterName, SpriteManager.Instance.GetCharacterSprite(characterType, Wizards[i].CharacterIconIndex));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(characterType), characterType, null);
            }
        }

        
        private void AddCharacter(string characterId, Character character)
        {
            switch (character.CharacterType)
            {
                case Enum.CharacterType.Warrior:
                    if (!WarriorDictionary.TryAdd(character.CharacterId, character))
                    {
                        Debug.LogWarning($"Character already exists in the dictionary: {character}");
                    }

                    break;
                case Enum.CharacterType.Archer:
                    if (!ArchersDictionary.TryAdd(character.CharacterId, character))
                    {
                        Debug.LogWarning($"Character already exists in the dictionary: {character}");
                    }

                    break;
                case Enum.CharacterType.Wizard:
                    if (!WizardsDictionary.TryAdd(character.CharacterId, character))
                    {
                        Debug.LogWarning($"Character already exists in the dictionary: {character}");
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}