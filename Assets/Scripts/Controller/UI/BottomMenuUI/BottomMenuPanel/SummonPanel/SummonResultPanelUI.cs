using System;
using System.Collections.Generic;
using Creature.Data;
using Data;
using Function;
using Managers.BattleManager;
using Managers.BottomMenuManager.SummonPanel;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SummonPanel
{
    public class SummonResultPanelUI : MonoBehaviour
    {
        [Header("루비")] [SerializeField] private TMP_Text currentDia;
        [Header("소환 레벨")] [SerializeField] private TMP_Text currentSummonLevel;
        [Header("소환 경험치")] [SerializeField] private TMP_Text currentSummonExp;
        [Header("소환 경험치 슬라이더")] [SerializeField] private Slider currentSummonExpSlider;

        [Header("소환 아이템")] public List<SummonResultPanelItemUI> summonResultPanelItems;
        [Header("추가 30회 뽑기")] [SerializeField] private Button extra30SummonBtn;
        [Header("추가 100회 뽑기")] [SerializeField] private Button extra100SummonBtn;
        [Header("추가 250회 뽑기")] [SerializeField] private Button extra250SummonBtn;
        
        [Header("나가기 버튼")] [SerializeField] private Button summonResultExitButton;
        
        public void InitializeEventListener()
        {
            extra30SummonBtn.onClick.AddListener(() => SummonManager.Instance.SummonRandomTarget(SummonManager.Instance.currentSummonType, 30));
            extra100SummonBtn.onClick.AddListener(() => SummonManager.Instance.SummonRandomTarget(SummonManager.Instance.currentSummonType, 100));
            extra250SummonBtn.onClick.AddListener(() => SummonManager.Instance.SummonRandomTarget(SummonManager.Instance.currentSummonType, 250));
            
            summonResultExitButton.onClick.AddListener(() => ActiveSummonResultPanel(false));
        }

        public void UpdateSummonResultPanelUI()
        {
            var targetSummonLevel = SummonManager.Instance.currentSummonType switch
            {
                Enums.SummonType.Squad => SummonManager.Instance.SquadSummonLevel,
                Enums.SummonType.Weapon => SummonManager.Instance.WeaponSummonLevel,
                Enums.SummonType.Gear => SummonManager.Instance.GearSummonLevel,
                _ => throw new ArgumentOutOfRangeException()
            };

            currentDia.text = $"<sprite={(int)Enums.IconType.Dia}> {BigInteger.ChangeMoney(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.Dia))}";
            currentSummonLevel.text = $"소환 레벨 {targetSummonLevel.CurrentSummonLevel}";
            currentSummonExp.text = $"{targetSummonLevel.CurrentSummonExp} / {targetSummonLevel.TargetSummonExp}";
            currentSummonExpSlider.value = targetSummonLevel.CurrentSummonExp / targetSummonLevel.TargetSummonExp;
        }

        public void ActiveSummonResultPanel(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}