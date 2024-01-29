using Creature.Data;
using Data;
using Managers.BattleManager;
using Managers.BottomMenuManager.TalentPanel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.TalentPanel
{
    public class TalentItemUI : MonoBehaviour
    {
        [Header("아이콘")] public Sprite squadTalentSprite;
        [Header("스탯 이름")] public string squadTalentName;
        [Header("초기값")] public int initTalentValue;
        [Header("레벨 업 비용")] public int levelUpCost = 1;
        [Header("스탯 증가 타입")] public Enum.StatTypeBySquadTalentPanel statTypeBySquadTalentPanel;
        [Header("스탯 증가량 타입")] public Enum.IncreaseStatValueType increaseTalentValueType;
        [Header("스탯 증가량")] public int increaseTalentValue;
        [Header("현재 스탯 레벨")] public int currentLevel;
        [Header("최대 스탯 레벨")] public int maxLevel = 10000;
        [Header("현재 스탯 강화 비용")] public int currentLevelUpCost;
        [Header("현재 스탯 증가량")] public int currentIncreasedStat;
        [Header("UI")] public Image squadTalentImage;

        public TMP_Text squadTalentNameText;
        public TMP_Text squadTalentLevelText;
        public TMP_Text squadTalentMaxLevelText;
        public TMP_Text squadCurrentIncreasedStatText;
        public TMP_Text squadTalentRequiredCurrencyText;
        public Button upgradeButton;
        public Button upgradeBlockButton;
        public System.Action<Enum.StatTypeBySquadTalentPanel, int> UpgradeTotalSquadStatBySquadTalentItem;

        public void InitSquadTalentUI()
        {
            squadTalentImage.sprite = squadTalentSprite;
            squadTalentMaxLevelText.text = $"최대 레벨 | {maxLevel}";
            squadTalentNameText.text = $"{squadTalentName}";
        }

        // 스텟 UI 업데이트 하는 메서드
        public void UpdateSquadTalentUI()
        {
            squadTalentLevelText.text = $"Lv. {currentLevel}";

            switch (increaseTalentValueType)
            {
                case Enum.IncreaseStatValueType.BaseStat:
                    squadCurrentIncreasedStatText.text = currentIncreasedStat == 0 ? "0" : $"{currentIncreasedStat}";
                    break;
                case Enum.IncreaseStatValueType.PercentStat:
                    squadCurrentIncreasedStatText.text =
                        currentIncreasedStat == 0 ? "0%" : $"{(double)currentIncreasedStat / 100}%";
                    break;
            }

            squadTalentRequiredCurrencyText.text = $"<sprite=15> {currentLevelUpCost}";
        }

        // 스텟 업데이트 하는 메서드
        public void UpdateSquadTalent(int count)
        {
            currentLevel += count;
            currentIncreasedStat += increaseTalentValue * count;

            ES3.Save($"{nameof(SquadEntireStat)}/{statTypeBySquadTalentPanel}/currentLevel : ", currentLevel);
            UpgradeTotalSquadStatBySquadTalentItem?.Invoke(statTypeBySquadTalentPanel, increaseTalentValue * count);

            switch (statTypeBySquadTalentPanel)
            {
                case Enum.StatTypeBySquadTalentPanel.Attack:
                    QuestManager.Instance.IncreaseQuestProgress(QuestManager.Instance.quests[0].questType, currentIncreasedStat);
                    break;
                case Enum.StatTypeBySquadTalentPanel.Health:
                    QuestManager.Instance.IncreaseQuestProgress(QuestManager.Instance.quests[1].questType, currentIncreasedStat);
                    break;
                case Enum.StatTypeBySquadTalentPanel.Defence:
                    QuestManager.Instance.IncreaseQuestProgress(QuestManager.Instance.quests[2].questType, currentIncreasedStat);
                    break;
            }
        }

        // 스텟 로드할 때 부르는 메서드
        public void LoadSquadStatLevel()
        {
            for (var i = 0; i < currentLevel; i++)
            {
                currentIncreasedStat += increaseTalentValue;

                UpgradeTotalSquadStatBySquadTalentItem?.Invoke(statTypeBySquadTalentPanel, increaseTalentValue);
            }
        }
    }
}