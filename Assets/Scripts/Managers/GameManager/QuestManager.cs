using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Controller.UI.TopMenuUI.QuestPanel;
using Data;
using Managers.BattleManager;
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

        private const string QUEST_SAVE_KEY = "QUEST";

        private void Awake()
        {
            Instance = this;
        }

        public void InitQuestManager()
        {
            IncreaseQuestProgressAction += IncreaseQuestProgress;
            CreateQuestsFromCsv();
            UpdateAllQuestProgress();
            UpdateQuestRewardPanelUI();
        }

        private void UpdateAllQuestProgress()
        {
            if (questLevel >= 46)
            {
                var questIndex = (questLevel - 46) % 9 + 46;
                currentQuest = quests[questIndex];

                var targetLevel = 30 + 5 * (questLevel - 46) / 9;
                
                switch (questIndex)
                {
                    case 0:
                        break;
                    case 1:
                        currentQuest.name = ParsingIncreaseTalentStatType(currentQuest.name, targetLevel);
                        currentQuest.targetProgress = targetLevel;
                        break;
                    case 2:
                        currentQuest.name = ParsingIncreaseTalentStatType(currentQuest.name, targetLevel);
                        currentQuest.targetProgress = targetLevel;
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        currentQuest.name = ParsingIncreaseStageLevel(targetLevel);
                        currentQuest.targetProgress = targetLevel;
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    case 8:
                        break;
                }
            }
            else
            {
                currentQuest = quests[questLevel];
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
            
            targetQuestRewardSprite = SpriteManager.Instance.GetCurrencySprite((Enums.CurrencyType)Enum.Parse(typeof(Enums.CurrencyType), $"{quests[questLevel].questRewardType}"));
            targetQuestRewardText = $"{quests[questLevel].reward}";
            targetQuestDescriptionText = $"{quests[questLevel].name}";

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
                case Enums.QuestType.SummonWeapon10:
                    break;
                case Enums.QuestType.AutoEquipSword:
                    break;
                case Enums.QuestType.AutoEquipBow:
                    break;
                case Enums.QuestType.AutoEquipStaff:
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
                case Enums.QuestType.SummonGear10:
                    break;
                case Enums.QuestType.AutoEquipHelmet:
                    break;
                case Enums.QuestType.AutoEquipArmor:
                    break;
                case Enums.QuestType.AutoEquipGauntlet:
                    break;
                case Enums.QuestType.SummonSquad10:
                    break;
                case Enums.QuestType.EquipSquad:
                    break;
                case Enums.QuestType.PlayGoldDungeon:
                    break;
                case Enums.QuestType.CompositeSword:
                    break;
                case Enums.QuestType.CompositeBow:
                    break;
                case Enums.QuestType.CompositeStaff:
                    break;
                case Enums.QuestType.CompositeHelmet:
                    break;
                case Enums.QuestType.CompositeArmor:
                    break;
                case Enums.QuestType.CompositeGauntlet:
                    break;
                case Enums.QuestType.PlayEnhanceStoneDungeon:
                    break;
                case Enums.QuestType.LevelUpCharacter:
                    break;
                case Enums.QuestType.SummonWeapon100:
                    break;
                case Enums.QuestType.SummonGear100:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateQuestPanelUI();
        }

        public void IncreaseQuestProgress(Enums.QuestType questType, int currentValue)
        {
            if (currentQuest.questType != questType) return;
            
            if (questType is Enums.QuestType.AttackTalentLevel or Enums.QuestType.HealthTalentLevel or Enums.QuestType.StageClear)
            {
                quests[questLevel].progress = currentValue;
            }
            else
            {
                quests[questLevel].progress += currentValue;   
            }

            if (quests[questLevel].progress < quests[questLevel].targetProgress) return;
            
            if (currentQuestTarget.targetMarks != null)
            {
                foreach (var tm in currentQuestTarget.targetMarks)
                {
                    tm.SetActive(true);
                }
            }
                
            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
            isCurrentQuestClear = true;
            ES3.Save($"{nameof(currentQuest)}", isCurrentQuestClear);
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
            var result = $"{stageNumber}-{subStageNumber}";

            return result;
        }

        private void CreateQuestsFromCsv()
        {
            questCsv = Resources.Load<TextAsset>("CSV/GuideQuest");
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
                        var quest = new Quest
                        {
                            name = fields[1].Trim(),
                            targetProgress = int.Parse(fields[2].Trim()),
                            progress = 0,
                            questRewardType = (Enums.QuestRewardType)Enum.Parse(typeof(Enums.QuestRewardType), fields[3].Trim()),
                            reward = int.Parse(fields[4].Trim()),
                            questType = (Enums.QuestType)Enum.Parse(typeof(Enums.QuestType), fields[5].Trim()),
                            isLoopQuest = fields[6].Trim() == "Loop"
                        };

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
            UIManager.Instance.questPanelUI.UpdateQuestPanelUI(targetQuestRewardSprite, targetQuestRewardText, targetQuestDescriptionText);
        }

        private void UpdateQuestRewardPanelUI()
        {
            UIManager.Instance.questPanelUI.questResultPanelUI.UpdateQuestResultPanelUI(targetQuestRewardSprite, targetQuestRewardText);
        }
        
        public void TargetQuestClear()
        {
            UpdateQuestRewardPanelUI();
            
            AccountManager.Instance.AddCurrency((Enums.CurrencyType)Enum.Parse(typeof(Enums.CurrencyType), $"{quests[questLevel].questRewardType}"), quests[questLevel].reward);
            UIManager.Instance.questPanelUI.questResultPanelUI.gameObject.SetActive(true);
            
            questLevel++;
            ES3.Save($"{nameof(questLevel)}", questLevel);

            UIManager.Instance.questPanelUI.completedMark.SetActive(false);

            UpdateAllQuestProgress();
        }
    }
}