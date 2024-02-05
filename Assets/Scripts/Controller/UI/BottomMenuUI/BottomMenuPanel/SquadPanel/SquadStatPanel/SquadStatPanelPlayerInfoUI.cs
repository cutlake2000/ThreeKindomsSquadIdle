using System;
using Function;
using Managers.BattleManager;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadStatPanel
{
    public class SquadStatPanelPlayerInfoUI : MonoBehaviour
    {
        [Header("계정 이름")] [SerializeField] private TMP_Text accountName;
        [Header("레벨")] [SerializeField] private TMP_Text accountLevel;
        [Header("현재 경험치")] [SerializeField] private TMP_Text accountExp;
        [Header("스탯 포인트")] [SerializeField] private TMP_Text accountStatPoint;
        [Header("경험치 슬라이더")] [SerializeField] private Slider accountExpSlider;
        [Header("레벨 업 버튼")] [SerializeField] private Button levelUpButton;
        [Header("레벨 업 잠금 버튼")] [SerializeField] private Button levelUpLockButton;
        
        public void InitializeEventListeners()
        {
            levelUpButton.onClick.AddListener(() => AccountManager.LevelUpAction?.Invoke());
        }
        
        public void UpdateSquadStatPanelSquadInfoAllUI(string nickName, int level, BigInteger currentExp, BigInteger maxExp, int statPoint)
        {
            UpdateSquadStatPanelSquadInfoAccountNameUI(nickName);
            UpdateSquadStatPanelSquadInfoLevelUI(level);
            UpdateSquadStatPanelSquadInfoStatPointUI(statPoint);
            UpdateSquadStatPanelSquadInfoExpUI(currentExp, maxExp);
        }
        
        public void UpdateSquadStatPanelSquadInfoAccountNameUI(string nickName)
        {
            accountName.text = $"{nickName}";
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
            levelUpButton.gameObject.SetActive(active);
            levelUpLockButton.gameObject.SetActive(!active);
        }
    }
}