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
        
        public void UpdateSquadStatPanelSquadInfoAllUI(int level, BigInteger currentExp, BigInteger maxExp, int statPoint)
        {
            //TODO : 닉네임 기능은 나중에
            UpdateSquadStatPanelSquadInfoLevelUI(level);
            UpdateSquadStatPanelSquadInfoStatPointUI(statPoint);
            UpdateSquadStatPanelSquadInfoExpUI(currentExp, maxExp);
        }

        public void UpdateSquadStatPanelSquadInfoLevelUI(int level)
        {
            accountLevel.text = $"Lv.{level}";
        }
        
        public void UpdateSquadStatPanelSquadInfoExpUI(BigInteger currentExp, BigInteger maxExp)
        {
            accountExp.text = $"{currentExp.ChangeMoney()} / {maxExp.ChangeMoney()}";
            var sliderValue = currentExp == 0 ? 0 : int.Parse((currentExp * 100/ maxExp).ToString());
            accountExpSlider.value = sliderValue;
        }
        
        public void UpdateSquadStatPanelSquadInfoStatPointUI(int statPoint)
        {
            accountStatPoint.text = $"스탯 포인트 : {statPoint}";
        }
        
        public void UpdateSquadStatPanelSquadInfoLevelUpButton(bool active)
        {
            levelUpButton.interactable = active;
        }
    }
}