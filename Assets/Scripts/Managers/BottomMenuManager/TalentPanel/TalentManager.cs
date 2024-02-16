using System;
using Controller.Effects;
using Controller.UI.BottomMenuUI.BottomMenuPanel.TalentPanel;
using Creature.Data;
using Data;
using Function;
using Keiwando.BigInteger;
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
                    UpgradeSquadTalentPanelStat(index));
                talentItem[i].GetComponent<TalentItemUI>().upgradeButton.GetComponent<HoldButton>().onHold
                    .AddListener(() => UpgradeSquadTalentPanelStat(index));
            }
        }

        // UpdateData 초기화 메서드 - 여기서 스텟퍼센트 조정 가능
        private void UpdateAllSquadTalentData()
        {
            for (var i = 0; i < squadTalentSo.Length; i++)
            {
                talentItem[i].currentLevelUpCost = new BigInteger[3];
                talentItem[i].squadTalentName = squadTalentSo[i].squadTalentName;
                talentItem[i].statTypeFromSquadTalentPanel = squadTalentSo[i].statTypeFromSquadTalentPanel;
                talentItem[i].increaseTalentValueType = squadTalentSo[i].increaseTalentValueType;
                talentItem[i].initialLevelUpCost = squadTalentSo[i].initialLevelUpCost;
                talentItem[i].extraLevelUpCost = squadTalentSo[i].levelUpCost;
                talentItem[i].increaseTalentValue = squadTalentSo[i].increaseTalentValue;
                talentItem[i].currentLevel = ES3.Load($"{nameof(SquadTalentSo)}/{talentItem[i].statTypeFromSquadTalentPanel}/currentLevel : ", 0);
                talentItem[i].currentLevelUpCost[0] = CalculateLevelUpCostOfTalent(talentItem[i].initialLevelUpCost, talentItem[i].currentLevel, talentItem[i].extraLevelUpCost, 1);
                talentItem[i].currentLevelUpCost[1] = CalculateLevelUpCostOfTalent(talentItem[i].initialLevelUpCost, talentItem[i].currentLevel, talentItem[i].extraLevelUpCost, 10);
                talentItem[i].currentLevelUpCost[2] = CalculateLevelUpCostOfTalent(talentItem[i].initialLevelUpCost, talentItem[i].currentLevel, talentItem[i].extraLevelUpCost, 100);
                talentItem[i].currentIncreasedStat = talentItem[i].currentLevel * talentItem[i].increaseTalentValue;
                talentItem[i].squadTalentSprite = squadTalentSo[i].squadTalentImage;
                talentItem[i].UpgradeTotalSquadStatBySquadTalentItem = OnUpgradeTotalSquadStatFromSquadTalentPanel;

                talentItem[i].InitSquadTalentUI();
                
                SquadBattleManager.Instance.squadEntireStat.UpdateStat(talentItem[i].statTypeFromSquadTalentPanel, talentItem[i].currentIncreasedStat, talentItem[i].increaseTalentValueType == Enums.IncreaseStatValueType.BaseStat);
            }
        }

        // 모든 스텟 UI 업데이트
        private void UpdateAllSquadTalentUI()
        {
            foreach (var squadTalent in talentItem) squadTalent.UpdateSquadTalentUI(levelUpMagnification);
        }

        public void UpgradeSquadTalentPanelStat(int index)
        {
            var talent = talentItem[index];
            
            if (!AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.Gold, talent.currentLevelUpCost[(int)Mathf.Log10(levelUpMagnification)])) return;
            if (talentItem[index].upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;

            var effect = objectPool.SpawnFromPool(Enums.PoolType.EffectEnhance);
            effect.transform.position = talent.effectTarget.position;
            effect.SetActive(true);
            effect.GetComponent<ParticleSystem>().Play();
            
            talent.currentLevel += levelUpMagnification;
            talent.currentIncreasedStat += talent.increaseTalentValue * levelUpMagnification;
            
            UpdateCurrentTalentItemLevelUpCost(talent);

            ES3.Save($"{nameof(SquadTalentSo)}/{talent.statTypeFromSquadTalentPanel}/currentLevel : ", talent.currentLevel);
            var isBaseStat = talent.increaseTalentValueType == Enums.IncreaseStatValueType.BaseStat;
            talent.UpgradeTotalSquadStatBySquadTalentItem?.Invoke(talent.statTypeFromSquadTalentPanel, talent.increaseTalentValue * levelUpMagnification, isBaseStat);

            switch (talent.statTypeFromSquadTalentPanel)
            {
                case Enums.SquadStatType.Attack:
                    QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.AttackTalentLevel, talent.currentLevel);
                    break;
                case Enums.SquadStatType.Health:
                    QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.HealthTalentLevel, talent.currentLevel);
                    break;
            }
            
            SetUpgradeUI(talentItem[index]);
            TalentPanelUI.CheckRequiredCurrencyOfMagnificationButton(index, levelUpMagnification);

            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        private static void UpdateCurrentTalentItemLevelUpCost(TalentItemUI currentTalentItem)
        {
            for (var i = 0; i < currentTalentItem.currentLevelUpCost.Length; i++)
            {
                currentTalentItem.currentLevelUpCost[i] = CalculateLevelUpCostOfTalent(currentTalentItem.initialLevelUpCost, currentTalentItem.currentLevel, currentTalentItem.extraLevelUpCost, (int) Mathf.Pow(10, i));   
            }
        }

        private static BigInteger CalculateLevelUpCostOfTalent(int initialCost, int currentLevel, int extraCost, int levelMagnification)
        {
            BigInteger returnCost = 0;

            for (var i = 0; i < levelMagnification; i++)
            {
                var currentPrice = initialCost + (currentLevel + i) * extraCost;
                returnCost += currentPrice;
            }

            return returnCost;
        }

        // 스텟 UI 업데이트
        private void SetUpgradeUI(TalentItemUI talentItemUI)
        {
            talentItemUI.UpdateSquadTalentUI(levelUpMagnification);
        }
    }
}