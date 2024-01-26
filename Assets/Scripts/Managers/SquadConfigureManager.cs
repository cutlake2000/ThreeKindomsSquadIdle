using System;
using System.Collections.Generic;
using Creature.Data;
using ScriptableObjects.Scripts;
using UnityEngine;
using Enum = Data.Enum;

namespace Managers
{
    public class SquadConfigureManager : MonoBehaviour
    {
        public static SquadConfigureManager Instance;
        
        //TODO: 임시 So 대체 클래스 -> 추후 csv, json으로 대체
        [SerializeField] private SquadConfigureSo[] squadConfigureSo;
        
        [Header("캐릭터 정보 컨테이너")]
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
                    
                    var character = characterSo.characterType switch
                    {
                        Enum.CharacterType.Warrior => warriors[characterIndex],
                        Enum.CharacterType.Archer => archers[characterIndex],
                        Enum.CharacterType.Wizard => wizards[characterIndex],
                        _ => null
                    };
                    
                    
                }
            }
        }
    }
}