using System;
using Data;
using Managers.BattleManager;
using Managers.BottomMenuManager.SummonPanel;
using Managers.BottomMenuManager.TalentPanel;
using Managers.GameManager;
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
        public Button summonX10LockButton;
        public Button summonX30LockButton;
        public Button summonX100LockButton;

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

        public void UpdateSummonPanelSummonButtonUI()
        {
            var requiredDia = Convert.ToInt32(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.Dia));

            if (requiredDia >= 100)
            {
                summonX10Button.gameObject.SetActive(true);
                summonX10LockButton.gameObject.SetActive(false);
            }
            else
            {
                summonX10Button.gameObject.SetActive(false);
                summonX10LockButton.gameObject.SetActive(true);
            }

            if (requiredDia >= 300)
            {
                summonX30Button.gameObject.SetActive(true);
                summonX30LockButton.gameObject.SetActive(false);
            }
            else
            {
                summonX30Button.gameObject.SetActive(false);
                summonX30LockButton.gameObject.SetActive(true);
            }

            if (requiredDia >= 1000)
            {
                summonX100Button.gameObject.SetActive(true);
                summonX100LockButton.gameObject.SetActive(false);
            }
            else
            {
                summonX100Button.gameObject.SetActive(false);
                summonX100LockButton.gameObject.SetActive(true);
            }
        }
    }
}