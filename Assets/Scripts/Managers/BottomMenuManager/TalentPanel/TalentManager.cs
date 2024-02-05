using System;
using Controller.Effects;
using Controller.UI.BottomMenuUI.BottomMenuPanel.TalentPanel;
using Creature.Data;
using Data;
using Function;
using Managers.BattleManager;
using Managers.GameManager;
using Module;
using ScriptableObjects.Scripts;
using UnityEngine;

namespace Managers.BottomMenuManager.TalentPanel
{
    public class TalentManager : MonoBehaviour
    {
        public static TalentManager Instance;

        [SerializeField] private SquadTalentSo[] squadTalentSo;
        public ObjectPool objectPool;
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
            UpdateAllSquadTalentData();
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
        private void UpdateAllSquadTalentData()
        {
            for (var i = 0; i < squadTalentSo.Length; i++)
            {
                talentItem[i].squadTalentName = squadTalentSo[i].squadTalentName;
                talentItem[i].statTypeFromSquadTalentPanel = squadTalentSo[i].statTypeFromSquadTalentPanel;
                talentItem[i].increaseTalentValueType = squadTalentSo[i].increaseTalentValueType;
                talentItem[i].initialLevelUpCost = squadTalentSo[i].initialLevelUpCost;
                talentItem[i].extraLevelUpCost = squadTalentSo[i].levelUpCost;
                talentItem[i].increaseTalentValue = squadTalentSo[i].increaseTalentValue;
                talentItem[i].currentLevel = ES3.Load($"{nameof(SquadEntireStat)}/{(Enums.StatTypeFromSquadTalentPanel)i}/currentLevel : ", 0);
                talentItem[i].currentLevelUpCost = CalculateLevelUpCostOfTalent(talentItem[i].initialLevelUpCost, talentItem[i].currentLevel, talentItem[i].extraLevelUpCost);
                talentItem[i].currentIncreasedStat = talentItem[i].currentLevel * talentItem[i].increaseTalentValue;
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
            var index = (int)type;
            var talent = talentItem[index];
            
            if (!AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.Gold,
                    talent.currentLevelUpCost * levelUpMagnification)) return;
            if (talentItem[index].upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;

            var effect = objectPool.SpawnFromPool(Enums.PoolType.EffectEnhance);
            effect.transform.position = talent.effectTarget.position;
            effect.SetActive(true);
            effect.GetComponent<ParticleSystem>().Play();
            
            talent.currentLevelUpCost = CalculateLevelUpCostOfTalent(talent.initialLevelUpCost, talent.currentLevel, talent.extraLevelUpCost);
            talent.UpdateSquadTalent(levelUpMagnification);
            SetUpgradeUI(talentItem[index]);
            TalentPanelUI.CheckRequiredCurrencyOfMagnificationButton(index);

            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        private BigInteger CalculateLevelUpCostOfTalent(int initialCost, int currentLevel, int extraCost)
        {
            return initialCost + currentLevel * extraCost;
        }

        // 스텟 UI 업데이트
        private static void SetUpgradeUI(TalentItemUI talentItemUI)
        {
            talentItemUI.UpdateSquadTalentUI();
        }
    }
}