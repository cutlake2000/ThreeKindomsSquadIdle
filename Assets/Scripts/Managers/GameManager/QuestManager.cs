using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Controller.UI.TopMenuUI.QuestPanel;
using Data;
using Managers.BattleManager;
using Managers.BottomMenuManager.InventoryPanel;
using Managers.BottomMenuManager.TalentPanel;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers.GameManager
{
    [Serializable]
    public class Quest
    {
        public string name;
        public Enums.QuestType questType; // 퀘스트 타입
        public int progress; // 현재 진행량
        public int increaseProgress;
        public int targetProgress; // 목표 진행량
        public Enums.QuestRewardType questRewardType;
        public int reward; // 보상
        public bool isLoopQuest;
        public Enums.OpenContent openContent;
    }

    [Serializable]
    public class QuestTarget
    {
        public Enums.QuestType questType;
        public GameObject[] targetMarks;
        public GameObject[] activeTarget;
        public GameObject[] inactiveTarget;
    }
    
    public class QuestManager : MonoBehaviour
    {
        public Action<Enums.QuestType, int> IncreaseQuestProgressAction;
        public static QuestManager Instance;
        public TextAsset questCsv;

        public int questLevel;
        public int targetQuestLevel;
        public Quest currentQuest;
        public QuestTarget currentQuestTarget;
        public bool isCurrentQuestClear;

        [Header("Quest Mark")]
        public GameObject initialQuestMark;
        public GameObject questMark;
        public List<Quest> quests;
        public List<QuestTarget> questTargets;
        public GameObject backboardPanel;
        
        [Header("Quest Reward")]
        public Sprite targetQuestRewardSprite;
        public string targetQuestRewardText;
        public string targetQuestDescriptionText;

        [Header("일회성 퀘스트 개수")] public int oneTimeQuest;
        [Header("반복성 퀘스트 개수")] public int loopQuest;
        [Header("콘텐츠 오픈 인덱스")] public List<int> contentsOpenIndex;

        private const string QUEST_SAVE_KEY = "QUEST";

        private void Awake()
        {
            Instance = this;
        }

        public void InitQuestManager()
        {
            IncreaseQuestProgressAction += IncreaseQuestProgress;
            CreateQuestsFromCsv();

            foreach (var index in contentsOpenIndex.Where(index => questLevel >= index))
            {
                UpdateLockContents(index);
            }
            
            UpdateAllQuestProgress();
        }

        private void UpdateAllQuestProgress()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent($"current_quest_{questLevel}");
            
            if (questLevel > oneTimeQuest) // 메인 퀘스트가 아닌 반복 퀘스트인 경우
            {
                targetQuestLevel = (questLevel - oneTimeQuest) % loopQuest + oneTimeQuest;
                currentQuest = quests[targetQuestLevel];

                var targetProcess = 50 + 10 * ((questLevel - oneTimeQuest) / loopQuest + 1);
                
                switch (currentQuest.questType)
                {
                    case Enums.QuestType.AttackTalentLevel:
                        currentQuest.name = ParsingIncreaseTalentStatType(currentQuest.name, targetProcess);
                        currentQuest.targetProgress = targetProcess;
                        break;
                    case Enums.QuestType.HealthTalentLevel:
                        currentQuest.name = ParsingIncreaseTalentStatType(currentQuest.name, targetProcess);
                        currentQuest.targetProgress = targetProcess;
                        break;
                    case Enums.QuestType.StageClear:
                        currentQuest.name = ParsingIncreaseStageLevel((questLevel - oneTimeQuest) / loopQuest);
                        currentQuest.targetProgress = 20 + 5 * ((questLevel - oneTimeQuest) / loopQuest + 1) + 1;
                        break;
                }
            }
            else
            {
                targetQuestLevel = questLevel;
                currentQuest = quests[questLevel];

                if (currentQuest.questType == Enums.QuestType.StageClear) currentQuest.targetProgress++;
                
                UpdateLockContents(questLevel);
            }
            
            foreach (var questTarget in questTargets.Where(questTarget => questTarget.questType == currentQuest.questType))
            {
                currentQuestTarget = questTarget;
            }
            
            questMark.SetActive(true);
            
            if (currentQuestTarget.targetMarks != null)
            {
                foreach (var tm in currentQuestTarget.targetMarks)
                {
                    tm.SetActive(true);
                }
            }
            
            targetQuestRewardSprite = SpriteManager.Instance.GetCurrencySprite((Enums.CurrencyType)Enum.Parse(typeof(Enums.CurrencyType), $"{quests[targetQuestLevel].questRewardType}"));
            targetQuestRewardText = $"{quests[targetQuestLevel].reward}";
            targetQuestDescriptionText = $"{quests[targetQuestLevel].name}";
            
            if (isCurrentQuestClear) UIManager.Instance.questPanelUI.completedMark.SetActive(true);
            else
            {
                switch (currentQuest.questType)
                {
                    case Enums.QuestType.AttackTalentLevel: 
                        if (TalentManager.Instance.talentItem[0].currentLevel >= currentQuest.targetProgress)
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.HealthTalentLevel:
                        if (TalentManager.Instance.talentItem[1].currentLevel >= currentQuest.targetProgress)
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.StageClear:
                        if (StageManager.Instance.currentAccumulatedStage >= currentQuest.targetProgress)
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.AutoEquipSword:
                        if (!(InventoryManager.Instance.equippedSword.equipmentRarity == Enums.EquipmentRarity.Common && InventoryManager.Instance.equippedSword.equipmentTier == 5))
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.AutoEquipBow:
                        if (!(InventoryManager.Instance.equippedBow.equipmentRarity == Enums.EquipmentRarity.Common && InventoryManager.Instance.equippedBow.equipmentTier == 5))
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.AutoEquipStaff:
                        if (!(InventoryManager.Instance.equippedStaff.equipmentRarity == Enums.EquipmentRarity.Common && InventoryManager.Instance.equippedStaff.equipmentTier == 5))
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.AutoEquipHelmet:
                        if (!(InventoryManager.Instance.equippedHelmet.equipmentRarity == Enums.EquipmentRarity.Common && InventoryManager.Instance.equippedHelmet.equipmentTier == 5))
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.AutoEquipArmor:
                        if (!(InventoryManager.Instance.equippedArmor.equipmentRarity == Enums.EquipmentRarity.Common && InventoryManager.Instance.equippedArmor.equipmentTier == 5))
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.AutoEquipGauntlet:
                        if (!(InventoryManager.Instance.equippedGauntlet.equipmentRarity == Enums.EquipmentRarity.Common && InventoryManager.Instance.equippedGauntlet.equipmentTier == 5))
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.TouchLoopButton:
                        if (StageManager.Instance.challengeProgress)
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.TouchChallengeButton:
                        if (StageManager.Instance.challengeProgress == false)
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                    case Enums.QuestType.TouchAutoSkillButton:
                        if (SquadBattleManager.Instance.autoSkill)
                        {
                            currentQuest.progress = currentQuest.targetProgress;
                            isCurrentQuestClear = true;
                            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
                            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                        }
                        break;
                }   
            }

            UpdateQuestPanelUI();
        }

        public void IncreaseQuestProgress(Enums.QuestType questType, int currentValue)
        {
            if (currentQuest.questType != questType) return;
            
            if (questType is Enums.QuestType.AttackTalentLevel or Enums.QuestType.HealthTalentLevel or Enums.QuestType.StageClear)
            {
                quests[targetQuestLevel].progress = currentValue;
            }
            else
            {
                quests[targetQuestLevel].progress += currentValue;   
            }

            if (quests[targetQuestLevel].progress < quests[targetQuestLevel].targetProgress) return;
            
            if (currentQuestTarget.targetMarks != null)
            {
                foreach (var tm in currentQuestTarget.targetMarks)
                {
                    tm.SetActive(false);
                }
            }
                
            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
            isCurrentQuestClear = true;
            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
        }

        private void UpdateLockContents(int index)
        {
            int targetIndex;
            int targetSubIndex;
            
            switch (quests[index].openContent)
            {
                case Enums.OpenContent.TalentPanel:
                    targetIndex = 2;
                    UIManager.Instance.bottomMenuPanelUI.UpdateLockButtonUI(targetIndex);
                    break;
                case Enums.OpenContent.InventoryPanel:
                    targetIndex = 1;
                    UIManager.Instance.bottomMenuPanelUI.UpdateLockButtonUI(targetIndex);
                    break;
                case Enums.OpenContent.SquadConfigurePanel:
                    targetIndex = 0;
                    UIManager.Instance.bottomMenuPanelUI.UpdateLockButtonUI(targetIndex);
                    break;
                case Enums.OpenContent.SummonWeaponPanel:
                    targetIndex = 4;
                    targetSubIndex = 1;
                    UIManager.Instance.bottomMenuPanelUI.UpdateLockButtonUI(targetIndex);
                    UIManager.Instance.summonPanelUI.UpdateLockItemUI(targetSubIndex);
                    break;
                case Enums.OpenContent.SummonGearPanel:
                    targetSubIndex = 2;
                    UIManager.Instance.summonPanelUI.UpdateLockItemUI(targetSubIndex);
                    break;
                case Enums.OpenContent.SummonCharacterPanel:
                    targetSubIndex = 0;
                    UIManager.Instance.summonPanelUI.UpdateLockItemUI(targetSubIndex);
                    break;
                case Enums.OpenContent.GoldDungeonPanel:
                    targetIndex = 5;
                    targetSubIndex = 0;
                    UIManager.Instance.bottomMenuPanelUI.UpdateLockButtonUI(targetIndex);
                    UIManager.Instance.dungeonPanelUI.UpdateLockItemUI(targetSubIndex);
                    break;
                case Enums.OpenContent.SquadEnhanceStoneDungeonPanel:
                    targetSubIndex = 2;
                    UIManager.Instance.dungeonPanelUI.UpdateLockItemUI(targetSubIndex);
                    break;
                case Enums.OpenContent.EquipmentEnhanceStoneDungeonPanel:
                    targetSubIndex = 1;
                    UIManager.Instance.dungeonPanelUI.UpdateLockItemUI(targetSubIndex);
                    break;
            }
        }

        private static string ParsingIncreaseTalentStatType(string oldName, int targetLevel)
        {
            // 정규식을 사용하여 문자열에서 "Lv." 다음에 오는 숫자를 추출
            const string pattern = @"Lv\.(\d+)";
            var match = Regex.Match(oldName, pattern);

            // 문자열에서 숫자를 추출하여 정수로 변환
            var currentLevel = 0;
            if (match.Success)
            {
                currentLevel = int.Parse(match.Groups[1].Value);
            }

            // 새로운 레벨 값을 문자열에 삽입하여 새로운 문자열 생성
            var newString = oldName.Replace($"Lv.{currentLevel}", $"Lv.{targetLevel}");

            // 결과 출력
            return newString;
        }

        private static string ParsingIncreaseStageLevel(int currentLevel)
        {
            int stageNumber;
            int subStageNumber;

            // 스테이지와 서브 스테이지 번호 계산
            if (currentLevel <= 5)
            {
                stageNumber = 1;
                subStageNumber = 25 + 5 * currentLevel;
            }
            else
            {
                stageNumber = (currentLevel - 5) / 5 + 1;
                subStageNumber = (currentLevel - 5) % 5 + 5;
            }

            // 결과 문자열 생성
            var result = $"{stageNumber}-{subStageNumber} 스테이지 돌파하기";

            return result;
        }

        private void CreateQuestsFromCsv()
        {
            questCsv = UnityEngine.Resources.Load<TextAsset>("CSV/GuideQuest");
            questLevel = ES3.Load($"{nameof(questLevel)}", 0);
            
            if (questLevel == 0) initialQuestMark.SetActive(true);
            
            quests = new List<Quest>();
            isCurrentQuestClear = ES3.Load($"{nameof(currentQuest)}", false);

            var lines = questCsv.text.Split('\n');

            for (var i = 1; i < lines.Length; i++) // 첫 번째 줄(헤더) 건너뛰기
            {
                var line = lines[i];
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 6)
                    {
                        var questName = fields[1].Trim();
                        var questTargetProgress = int.Parse(fields[2].Trim());
                        var questProgress = 0;
                        var questRewardType = (Enums.QuestRewardType)Enum.Parse(typeof(Enums.QuestRewardType), fields[3].Trim());
                        var questReward = int.Parse(fields[4].Trim());
                        var questType = (Enums.QuestType)Enum.Parse(typeof(Enums.QuestType), fields[5].Trim());
                        var questIsLoop = fields[6].Trim() == "Loop";
                        var questOpenContent= (Enums.OpenContent)Enum.Parse(typeof(Enums.OpenContent), fields[7].Trim());
                        
                        var quest = new Quest
                        {
                            name = questName,
                            targetProgress = questTargetProgress,
                            progress = questProgress,
                            questRewardType = questRewardType,
                            reward = questReward,
                            questType = questType,
                            isLoopQuest = questIsLoop,
                            openContent = questOpenContent
                        };

                        if (quest.openContent != Enums.OpenContent.None) contentsOpenIndex.Add(i - 1);
                        if (quest.isLoopQuest == false)
                        {
                            oneTimeQuest++;
                        }
                        else
                        {
                            loopQuest++;
                        }
                        
                        quests.Add(quest);
                    }
                    else
                    {
                        Debug.LogError("Invalid targetProgress value in CSV: " + fields[2]);
                    }
                }
                else
                {
                    Debug.LogError("Invalid line in CSV: " + line);
                }
            }

            ES3.Save(QUEST_SAVE_KEY, quests);
        }

        private void UpdateQuestPanelUI()
        {
            var questType = currentQuest.isLoopQuest switch
            {
                true => "반복 퀘스트",
                false => "메인 퀘스트"
            };

            var questName = $"<color=#D7AB56><b>{questType} {questLevel + 1}</b></color>";
            var questDescription = $"{targetQuestDescriptionText}";
            var questPanelText = $"{questName}\n{questDescription}";
            
            UIManager.Instance.questPanelUI.UpdateQuestPanelUI(targetQuestRewardSprite, targetQuestRewardText, questPanelText);
        }

        public void UpdateQuestRewardPanelUI()
        {
            UIManager.Instance.questPanelUI.questResultPanelUI.UpdateQuestResultPanelUI(targetQuestRewardSprite, targetQuestRewardText, isCurrentQuestClear);
        }
        
        public void TargetQuestClear()
        {
            AccountManager.Instance.AddCurrency((Enums.CurrencyType)Enum.Parse(typeof(Enums.CurrencyType), $"{quests[targetQuestLevel].questRewardType}"), quests[targetQuestLevel].reward);
            
            questLevel++;
            ES3.Save($"{nameof(questLevel)}", questLevel);
            ES3.Save($"{nameof(currentQuest)}", false);
            
            UIManager.Instance.questPanelUI.completedMark.SetActive(false);
            isCurrentQuestClear = false;
            
            UpdateAllQuestProgress();
        }
    }
}