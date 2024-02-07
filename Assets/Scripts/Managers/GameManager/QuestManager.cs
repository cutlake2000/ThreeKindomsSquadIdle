using System;
using System.Collections.Generic;
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
        public string description;
        public Enums.QuestType questType; // 퀘스트 타입
        public int progress; // 현재 진행량
        public int increaseProgress;
        public int targetProgress; // 목표 진행량
        public Enums.QuestRewardType questRewardType;
        public int reward; // 보상
    }

    [Serializable]
    public class QuestTarget
    {
        public Enums.QuestType questType;
        public GameObject targetMark;
        public GameObject[] activeTarget;
        public GameObject[] inactiveTarget;
    }
    
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance;
        public TextAsset questCsv;

        public int questLevel;
        public GameObject questMark;
        public List<Quest> quests;
        public List<QuestTarget> questTargets;

        private const string QUEST_SAVE_KEY = "QUEST";

        private void Awake()
        {
            Instance = this;
        }

        public void InitQuestManager()
        {
            SetAllQuests();
            UpdateAllQuestProgress();
        }

        private void UpdateAllQuestProgress()
        {
            foreach (var quest in quests)
            {
                switch (quest.questType)
                {
                    case Enums.QuestType.AttackTalentLevel:
                        quest.targetProgress = quest.increaseProgress * ((questLevel + 5) / 5);
                        quest.name = $"{quest.description}{quest.targetProgress} 달성";
                        break;
                    case Enums.QuestType.HealthTalentLevel:
                        quest.targetProgress = quest.increaseProgress * ((questLevel + 5) / 5);
                        quest.name = $"{quest.description}{quest.targetProgress} 달성";
                        break;
                    case Enums.QuestType.DefenceTalentLevel:
                        quest.targetProgress = quest.increaseProgress * ((questLevel + 5) / 5);
                        quest.name = $"{quest.description}{quest.targetProgress} 달성";
                        break;
                    case Enums.QuestType.SquadLevel:
                        quest.targetProgress = quest.increaseProgress * ((questLevel + 5) / 5);
                        quest.name = $"{quest.description}{quest.targetProgress} 달성";
                        break;
                    case Enums.QuestType.StageClear:
                        quest.targetProgress = questLevel > 50 ? 5 : questLevel + 5;
                        quest.name =
                            $"{quest.description} {quest.targetProgress / 50 + 1}-{quest.targetProgress} 클리어";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }   
            }

            UpdateQuestPanelUI();
        }

        public void IncreaseQuestProgress(Enums.QuestType questType, int currentValue)
        {
            var index = (int)questType;
            var currentTargetIndex = questLevel % 5;
                
            quests[index].progress = currentValue;

            if (quests[index].progress < quests[index].targetProgress) return;
            if (index == questLevel % 5) UIManager.Instance.questPanelUI.completedMark.SetActive(true);
            
            if (currentTargetIndex != 5 && questTargets[currentTargetIndex].targetMark.activeInHierarchy && questTargets[currentTargetIndex].questType == questType)
            {
                questTargets[currentTargetIndex].targetMark.SetActive(false);
            }
        }

        private void SetAllQuests()
        {
            if (ES3.KeyExists(QUEST_SAVE_KEY))
            {
                questLevel = ES3.Load($"{nameof(questLevel)}", 0);
                quests = ES3.Load<List<Quest>>(QUEST_SAVE_KEY);
            }
            else
            {
                CreateQuestsFromCsv();

                foreach (var quest in quests)
                {
                    quest.progress = quest.questType switch
                    {
                        Enums.QuestType.AttackTalentLevel => TalentManager.Instance.talentItem[0].currentLevel,
                        Enums.QuestType.HealthTalentLevel => TalentManager.Instance.talentItem[1].currentLevel,
                        Enums.QuestType.DefenceTalentLevel => TalentManager.Instance.talentItem[2].currentLevel,
                        Enums.QuestType.SquadLevel => AccountManager.Instance.accountLevel,
                        Enums.QuestType.StageClear => StageManager.Instance.currentStageIndex,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
            }
        }

        private void CreateQuestsFromCsv()
        {
            questCsv = Resources.Load<TextAsset>("CSV/QuestData");
            questLevel = ES3.Load($"{nameof(questLevel)}", 0);
            quests = new List<Quest>();

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
                            description = fields[1].Trim(), // CSV에 설명이 없으므로 빈 문자열 할당
                            increaseProgress = int.Parse(fields[2].Trim()),
                            questRewardType =
                                (Enums.QuestRewardType)System.Enum.Parse(typeof(Enums.QuestRewardType), fields[3].Trim()),
                            reward = int.Parse(fields[4].Trim()),
                            questType = (Enums.QuestType)System.Enum.Parse(typeof(Enums.QuestType), fields[5].Trim()),
                            progress = 0, // 초기 진행량은 0
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
            var targetQuestIndex = questLevel % 5;
            var targetQuestRewardSprite = SpriteManager.Instance.GetCurrencySprite(
                (Enums.CurrencyType)Enum.Parse(typeof(Enums.QuestRewardType), $"{quests[targetQuestIndex].questRewardType}"));
            var targetQuestRewardText = $"{quests[targetQuestIndex].reward}";
            var targetQuestDescriptionText = $"{quests[targetQuestIndex].name}";
            UIManager.Instance.questPanelUI.UpdateQuestPanelUI(targetQuestRewardSprite, targetQuestRewardText, targetQuestDescriptionText);
            UIManager.Instance.questPanelUI.questResultPanelUI.UpdateQuestResultPanelUI(targetQuestRewardSprite, targetQuestRewardText);

            if (quests[questLevel % 5].progress >= Instance.quests[questLevel % 5].targetProgress)
            {
                UIManager.Instance.questPanelUI.completedMark.SetActive(true);
            }
            
            if (questLevel < 4)
            {
                if (!questMark.activeInHierarchy)
                {
                    questMark.SetActive(true);
                }
                
                questTargets[questLevel % 5].targetMark.SetActive(true);
            }
            else if (questLevel == 5)
            {
                questMark.SetActive(false);
            }
        }
        
        public void TargetQuestClear()
        {
            var targetQuestIndex = questLevel % 5;
            AccountManager.Instance.AddCurrency((Enums.CurrencyType)Enum.Parse(typeof(Enums.QuestRewardType), $"{quests[targetQuestIndex].questRewardType}"), quests[targetQuestIndex].reward);
            UIManager.Instance.questPanelUI.questResultPanelUI.gameObject.SetActive(true);
            
            questLevel++;
            ES3.Save($"{nameof(questLevel)}", questLevel);
            UIManager.Instance.questPanelUI.completedMark.SetActive(false);

            if (questLevel % 5 == 0)
            {
                UpdateAllQuestProgress();   
            }
            else
            {
                UpdateQuestPanelUI();
            }
        }
    }
}