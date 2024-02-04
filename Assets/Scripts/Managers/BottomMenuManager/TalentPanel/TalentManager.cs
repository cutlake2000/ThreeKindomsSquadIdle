using System;
using Controller.UI.BottomMenuUI.BottomMenuPanel.TalentPanel;
using Creature.Data;
using Data;
using Function;
using Managers.BattleManager;
using ScriptableObjects.Scripts;
using UnityEngine;

namespace Managers.BottomMenuManager.TalentPanel
{
    public class TalentManager : MonoBehaviour
    {
        public static TalentManager Instance;

        [SerializeField] private SquadTalentSo[] squadTalentSo;
        public TalentItemUI[] talentItem;
        public int levelUpMagnification;

        private void Awake()
        {
            Instance = this;
        }

        public event Action<Enums.SquadStatType, int, bool> OnUpgradeTotalSquadStatFromSquadTalentPanel;

        public void InitSquadTalentManager()
        {
            InitializeEventListeners();
            SetSquadTalentData();
            UpdateAllSquadTalentUI();
        }

        // 버튼 초기화 메서드
        private void InitializeEventListeners()
        {
            for (var i = 0; i < talentItem.Length; i++)
            {
                var index = i;
                talentItem[i].GetComponent<TalentItemUI>().upgradeButton.onClick.AddListener(() =>
                    UpgradeSquadTalentPanelStat((Enums.StatTypeFromSquadTalentPanel)index));
                talentItem[i].GetComponent<TalentItemUI>().upgradeButton.GetComponent<HoldButton>().onHold
                    .AddListener(() => UpgradeSquadTalentPanelStat((Enums.StatTypeFromSquadTalentPanel)index));
            }
        }

        // UpdateData 초기화 메서드 - 여기서 스텟퍼센트 조정 가능
        private void SetSquadTalentData()
        {
            for (var i = 0; i < squadTalentSo.Length; i++)
            {
                talentItem[i].squadTalentName = squadTalentSo[i].squadTalentName;
                talentItem[i].statTypeFromSquadTalentPanel = squadTalentSo[i].statTypeFromSquadTalentPanel;
                talentItem[i].increaseTalentValueType = squadTalentSo[i].increaseTalentValueType;
                talentItem[i].increaseTalentValue = squadTalentSo[i].increaseTalentValue;
                talentItem[i].currentLevel =
                    ES3.Load($"{nameof(SquadEntireStat)}/{(Enums.StatTypeFromSquadTalentPanel)i}/currentLevel : ",
                        0);
                talentItem[i].currentLevelUpCost = squadTalentSo[i].levelUpCost;
                talentItem[i].currentIncreasedStat =
                    talentItem[i].currentLevel * talentItem[i].increaseTalentValue;
                talentItem[i].squadTalentSprite = squadTalentSo[i].squadTalentImage;
                talentItem[i].UpgradeTotalSquadStatBySquadTalentItem = OnUpgradeTotalSquadStatFromSquadTalentPanel;

                talentItem[i].InitSquadTalentUI();
            }
        }

        // 모든 스텟 UI 업데이트
        private void UpdateAllSquadTalentUI()
        {
            foreach (var squadTalent in talentItem) squadTalent.UpdateSquadTalentUI();
        }

        public void UpgradeSquadTalentPanelStat(Enums.StatTypeFromSquadTalentPanel type)
        {
            if (!AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.Gold,
                    talentItem[(int)type].levelUpCost * levelUpMagnification)) return;
            if (talentItem[(int)type].upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;

            talentItem[(int)type].UpdateSquadTalent(levelUpMagnification);
            SetUpgradeUI(talentItem[(int)type]);
            TalentPanelUI.CheckRequiredCurrencyOfMagnificationButton((int)type);

            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        // 스텟 UI 업데이트
        private static void SetUpgradeUI(TalentItemUI talentItemUI)
        {
            talentItemUI.UpdateSquadTalentUI();
        }
    }
}