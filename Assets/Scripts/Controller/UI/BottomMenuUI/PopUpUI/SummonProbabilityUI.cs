using System;
using System.Collections.Generic;
using Data;
using Managers.BottomMenuManager.SummonPanel;
using Resources.ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.PopUpUI
{
    public class SummonProbabilityUI : MonoBehaviour
    {
        [Header("Exit Button")]
        [SerializeField] private Button exitButton;
        [SerializeField] private Button backgroundButton;
        
        [Header("패널 정보")]
        [SerializeField] private TMP_Text summonTitle;
        [SerializeField] private TMP_Text summonLevel;
        [SerializeField] private List<GameObject> probabilityItems;
        
        [Header("전환 버튼")]
        [SerializeField] private Button previousLevelButton;
        [SerializeField] private Button nextLevelButton;

        [SerializeField] private int currentSummonLevel;
        [SerializeField] private Enums.SummonType currentSummonType;
        
        public void InitializeEventListener()
        {
            exitButton.onClick.AddListener(InactivatePanel);
            backgroundButton.onClick.AddListener(InactivatePanel); 
            previousLevelButton.onClick.AddListener(() => UpdateSummonProbabilityLevel(-1));
            nextLevelButton.onClick.AddListener(() => UpdateSummonProbabilityLevel(1));
        }

        private void UpdateSummonProbabilityLevel(int index)
        {
            UpdateProbabilityItemData(currentSummonType, index);
        }

        private void InactivatePanel()
        {
            gameObject.SetActive(false);
        }

        public void UpdateProbabilityItemData(Enums.SummonType summonType, int adjustIndexValue)
        {
            SummonProbability[] targetSummon = { };
            var startIndex = 0;
            
            currentSummonType = summonType;
            
            switch (currentSummonType)
            {
                case Enums.SummonType.Squad:
                    summonTitle.text = "영웅 소환 확률";
                    if (adjustIndexValue == 0) currentSummonLevel = SummonManager.Instance.SquadSummonLevel.CurrentSummonLevel;
                    targetSummon = SummonManager.Instance.summonSo.SummonSquads;
                    startIndex = 2;
                    
                    break;
                case Enums.SummonType.Weapon:
                    summonTitle.text = "무기 소환 확률";
                    if (adjustIndexValue == 0) currentSummonLevel = SummonManager.Instance.WeaponSummonLevel.CurrentSummonLevel;
                    targetSummon = SummonManager.Instance.summonSo.SummonWeapons;
                    startIndex = 0;
                    
                    break;
                case Enums.SummonType.Gear:
                    summonTitle.text = "방어구 소환 확률";
                    if (adjustIndexValue == 0) currentSummonLevel = SummonManager.Instance.GearSummonLevel.CurrentSummonLevel;
                    targetSummon = SummonManager.Instance.summonSo.SummonGears;
                    startIndex = 0;
                    
                    break;
            }
            
            currentSummonLevel += adjustIndexValue;
            summonLevel.text = $"소환 레벨 {currentSummonLevel}";
            
            previousLevelButton.gameObject.SetActive(currentSummonLevel != 1);
            nextLevelButton.gameObject.SetActive(currentSummonLevel != targetSummon.Length);
            
            var targetProbabilities = targetSummon[currentSummonLevel - 1].SummonProbabilities;
            
            for (var i = 0; i < probabilityItems.Count; i++)
            {
                if (i < startIndex)
                {
                    probabilityItems[i].SetActive(false);
                }
                else
                {
                    if (i > targetProbabilities.Length) break;
                
                    probabilityItems[i].SetActive(true);
                    probabilityItems[i].GetComponent<SummonProbabilityItemUI>().UpdateSummonProbabilityItemUI(targetProbabilities[i - startIndex]);
                }
            }
        }
    }
}