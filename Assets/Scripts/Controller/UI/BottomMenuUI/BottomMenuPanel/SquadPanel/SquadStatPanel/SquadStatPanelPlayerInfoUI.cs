using System;
using Function;
using Managers.BattleManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadStatPanel
{
    public class SquadStatPanelPlayerInfoUI : MonoBehaviour
    {
        [Header("계정 이름")] [SerializeField] private TMP_Text nickName;
        [Header("레벨")] [SerializeField] private TMP_Text accountLevel;
        [Header("현재 경험치")] [SerializeField] private TMP_Text accountExp;
        [Header("스탯 포인트")] [SerializeField] private TMP_Text accountStatPoint;
        [Header("경험치 슬라이더")] [SerializeField] private Slider accountExpSlider;
        [Header("레벨 업 버튼")] [SerializeField] private Button levelUpButton;
        
        public void InitializeEventListeners()
        {
            levelUpButton.onClick.AddListener(() => AccountManager.LevelUpAction?.Invoke());
        }
        
        public void UpdateSquadStatPanelSquadInfoAllUI(string level, BigInteger currentExp, BigInteger maxExp, string statPoint)
        {
            //TODO : 닉네임 기능은 나중에
            accountLevel.text = $"Lv. {level}";
            accountExp.text = $"{currentExp.ChangeMoney()} / {maxExp.ChangeMoney()}";
            accountStatPoint.text = $"스탯 포인트 : {statPoint}";
            accountExpSlider.value = currentExp == 0 ? 0 : int.Parse((currentExp / maxExp).ToString());
            accountExpSlider.maxValue = 100;
        }

        public void UpdateSquadStatPanelSquadInfoLevelUI(string level)
        {
            accountLevel.text = $"Lv.{level}";
        }
        
        public void UpdateSquadStatPanelSquadInfoExpUI(string currentExp, string maxExp, float value)
        {
            accountExp.text = $"{currentExp} / {maxExp}";
            accountExpSlider.value = value;
        }
        
        public void UpdateSquadStatPanelSquadInfoStatPointUI(string statPoint)
        {
            accountStatPoint.text = $"스탯 포인트 : {statPoint}";
        }
        
        public void UpdateSquadStatPanelSquadInfoLevelUpButton(bool active)
        {
            levelUpButton.interactable = active;
        }
    }
}