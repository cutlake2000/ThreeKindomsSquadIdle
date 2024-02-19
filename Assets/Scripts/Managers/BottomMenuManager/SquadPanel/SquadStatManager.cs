using System;
using Controller.Effects;
using Controller.UI;
using Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadStatPanel;
using Creature.Data;
using Data;
using Function;
using Managers.BattleManager;
using Managers.GameManager;
using Module;
using ScriptableObjects.Scripts;
using UnityEngine;

namespace Managers.BottomMenuManager.SquadPanel
{
    public class SquadStatManager : MonoBehaviour
    {
        public static SquadStatManager Instance;
        public event Action<Enums.SquadStatType, int, bool> OnUpgradeTotalSquadStatFromSquadStatPanel;

        public ObjectPool objectPool;
        public Transform effectTarget;
        [SerializeField] private SquadStatSo[] squadStatSo;
        public SquadStatPanelItemUI[] squadStatItem;
        public int levelUpMagnification;

        private void Awake()
        {
            Instance = this;
        }

        // 이벤트 설정하는 메서드
        public void InitSquadStatManager()
        {
            InitializeEventListeners();
            UpdateAllSquadStatData();
            UpdateAllSquadStatUI();
        }

        // 버튼 초기화 메서드
        private void InitializeEventListeners()
        {
            for (var i = 0; i < squadStatItem.Length; i++)
            {
                var index = i;
                squadStatItem[i].GetComponent<SquadStatPanelItemUI>().upgradeButton.onClick.AddListener(() =>
                    UpgradeSquadStatPanelStat(index));
                
                squadStatItem[i].GetComponent<SquadStatPanelItemUI>().upgradeButton.GetComponent<HoldButton>().onHold
                    .AddListener(() => UpgradeSquadStatPanelStat(index));
                
                squadStatItem[i].GetComponent<SquadStatPanelItemUI>().upgradeBlockButton.GetComponent<LockButtonUI>().InitializeEventListener();
            }
        }

        // UpdateData 초기화 메서드 - 여기서 스텟퍼센트 조정 가능
        private void UpdateAllSquadStatData()
        {
            levelUpMagnification = 1;
            
            for (var i = 0; i < squadStatSo.Length; i++)
            {
                squadStatItem[i].squadStatName = squadStatSo[i].squadStatName;
                squadStatItem[i].statTypeFromSquadStatPanel = squadStatSo[i].statTypeFromSquadStatPanel;
                squadStatItem[i].increaseStatValueType = squadStatSo[i].increaseStatValueType;
                squadStatItem[i].increaseStatValue = squadStatSo[i].increaseStatValue;
                squadStatItem[i].currentLevel = ES3.Load($"{nameof(SquadStatSo)}/{squadStatItem[i].statTypeFromSquadStatPanel}/currentLevel : ", 0);
                squadStatItem[i].currentLevelUpCost = squadStatSo[i].levelUpCost;
                squadStatItem[i].currentIncreasedStat = squadStatItem[i].currentLevel * squadStatItem[i].increaseStatValue;
                squadStatItem[i].squadStatSprite = squadStatSo[i].squadStatImage;
                squadStatItem[i].UpgradeTotalSquadStatBySquadStatItem = OnUpgradeTotalSquadStatFromSquadStatPanel;

                squadStatItem[i].InitSquadStatUI();
                
                SquadBattleManager.Instance.squadEntireStat.UpdateStat(squadStatItem[i].statTypeFromSquadStatPanel, squadStatItem[i].currentIncreasedStat, squadStatItem[i].increaseStatValueType == Enums.IncreaseStatValueType.BaseStat);
            }
        }

        // 모든 스텟 UI 업데이트
        private void UpdateAllSquadStatUI()
        {
            foreach (var squadStat in squadStatItem) squadStat.UpdateSquadStatUI();
        }

        public void UpgradeSquadStatPanelStat(int index)
        {
            if (AccountManager.Instance.statPoint < squadStatItem[index].levelUpCost * levelUpMagnification) return;
            if (squadStatItem[index].upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;

            AccountManager.Instance.statPoint -= squadStatItem[index].levelUpCost * levelUpMagnification;
            
            var effect = objectPool.SpawnFromPool(Enums.PoolType.EffectEnhance);
            effect.transform.position = squadStatItem[index].effectTarget.position;
            effect.SetActive(true);
            effect.GetComponent<ParticleSystem>().Play();

            // Debug.Log($"levelUpCost {squadStatItem[index].levelUpCost}");
            // Debug.Log($"levelUpMagnification {levelUpMagnification}");
            // Debug.Log($"Magnification {squadStatItem[index].levelUpCost * levelUpMagnification}");

            squadStatItem[index].UpdateSquadStat(levelUpMagnification);
            SetUpgradeUI(squadStatItem[index]);
            UIManager.Instance.squadPanelUI.squadStatPanelUI.CheckRequiredCurrencyOfMagnificationAllButton();
            UIManager.Instance.squadPanelUI.squadStatPanelUI.squadStatPanelPlayerInfoUI.UpdateSquadStatPanelSquadInfoStatPointUI(AccountManager.Instance.statPoint);
            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        // 스텟 UI 업데이트
        private static void SetUpgradeUI(SquadStatPanelItemUI squadStatPanelItemUI)
        {
            squadStatPanelItemUI.UpdateSquadStatUI();
        }
    }
}