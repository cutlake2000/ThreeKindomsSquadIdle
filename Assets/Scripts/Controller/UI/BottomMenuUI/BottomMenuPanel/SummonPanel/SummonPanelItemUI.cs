using System;
using Data;
using Managers.BattleManager;
using Managers.BottomMenuManager.SummonPanel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SummonPanel
{
    public class SummonPanelItemUI : MonoBehaviour
    {
        public Enums.SummonType summonType;
        public Slider summonSquadExpSlider;
        public TMP_Text summonLevelText;
        public Button summonX10Button;
        public Button summonX30Button;
        public Button summonX100Button;

        public void InitializeEventListener()
        {
            summonX10Button.onClick.AddListener(() => SummonManager.Instance.SummonRandomTarget(summonType, 10));
            summonX30Button.onClick.AddListener(() => SummonManager.Instance.SummonRandomTarget(summonType, 30));
            summonX100Button.onClick.AddListener(() => SummonManager.Instance.SummonRandomTarget(summonType, 100));
        }
        
        public void UpdateSummonPanelItemUI(int level, float currentExp, float targetExp)
        {
            summonLevelText.text = $"소환 레벨 : {level} ({currentExp} / {targetExp})";
            summonSquadExpSlider.value = currentExp / targetExp;
        }
    }
}