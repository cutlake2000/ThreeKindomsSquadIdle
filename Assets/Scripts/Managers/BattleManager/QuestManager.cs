using System;
using System.Collections.Generic;
using Managers.BottomMenuManager.SquadPanel;
using Managers.BottomMenuManager.TalentPanel;
using UnityEngine;
using UnityEngine.Serialization;
using Enum = Data.Enum;

namespace Managers.BattleManager
{
    [Serializable]
    public class Quest
    {
        public string questID; // 유니크한 퀘스트 식별자
        public string name;
        public string description;
        public Enum.QuestType questType; // 퀘스트 타입
        public int progress; // 현재 진행량
        public int increaseProgress;
        public int targetProgress; // 목표 진행량
        public Enum.QuestRewardType questRewardType;
        public int reward; // 보상
        public bool isCompleted; // 완료 여부

        // 퀘스트 완료 여부를 업데이트하는 메서드
        public void UpdateCompletion()
        {
            isCompleted = progress >= targetProgress;
        }
    }
    
    public class QuestManager : MonoBehaviour
    {
        public Action<Enum.QuestType> UpdateTargetQuestProgressAction;
        
        public static QuestManager Instance;
        public TextAsset questCsv;

        public int questLevel;
        public List<Quest> quests;

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
                quest.questType = quest.questType;
                quest.questRewardType = quest.questRewardType;

                switch (quest.questType)
                {
                    case Enum.QuestType.AttackTalentLevel:
                        quest.targetProgress = quest.increaseProgress * ((questLevel + 5) / 5);
                        quest.name = $"{quest.description}{quest.targetProgress} 달성";
                        break;
                    case Enum.QuestType.HealthTalentLevel:
                        quest.targetProgress = quest.increaseProgress * ((questLevel + 5) / 5);
                        quest.name = $"{quest.description}{quest.targetProgress} 달성";
                        break;
                    case Enum.QuestType.DefenceTalentLevel:
                        quest.targetProgress = quest.increaseProgress * ((questLevel + 5) / 5);
                        quest.name = $"{quest.description}{quest.targetProgress} 달성";
                        break;
                    case Enum.QuestType.SquadLevel:
                        quest.targetProgress = quest.increaseProgress * ((questLevel + 5) / 5);
                        quest.name = $"{quest.description}{quest.targetProgress} 달성";
                        break;
                    case Enum.QuestType.StageClear:
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

        public void IncreaseQuestProgress(Enum.QuestType questType, int currentValue)
        {
            quests[(int)questType].progress = currentValue;

            if (quests[(int)questType].progress < quests[(int)questType].targetProgress) return;
            quests[(int)questType].isCompleted = true;
            UIManager.Instance.questPanelUI.completedMark.SetActive(true);
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
                        Enum.QuestType.AttackTalentLevel => TalentManager.Instance.talentItem[0].currentLevel,
                        Enum.QuestType.HealthTalentLevel => TalentManager.Instance.talentItem[1].currentLevel,
                        Enum.QuestType.DefenceTalentLevel => TalentManager.Instance.talentItem[2].currentLevel,
                        Enum.QuestType.SquadLevel => AccountManager.Instance.accountLevel, //TODO : 캐릭터 레벨 기능 / 스테이지 저장 기능 구현 후 완성
                        Enum.QuestType.StageClear => StageManager.Instance.currentStageIndex,
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
                            questID = fields[0].Trim(),
                            description = fields[1].Trim(), // CSV에 설명이 없으므로 빈 문자열 할당
                            increaseProgress = int.Parse(fields[2].Trim()),
                            questRewardType =
                                (Enum.QuestRewardType)System.Enum.Parse(typeof(Enum.QuestRewardType), fields[3].Trim()),
                            reward = int.Parse(fields[4].Trim()),
                            questType = (Enum.QuestType)System.Enum.Parse(typeof(Enum.QuestType), fields[5].Trim()),
                            progress = 0, // 초기 진행량은 0
                            isCompleted = false // 초기 상태는 미완료
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
                (Enum.CurrencyType)System.Enum.Parse(typeof(Enum.QuestRewardType), $"{quests[targetQuestIndex].questRewardType}"));
            var targetQuestRewardText = $"{quests[targetQuestIndex].reward}";
            var targetQuestDescriptionText = $"{quests[targetQuestIndex].name}";
            UIManager.Instance.questPanelUI.UpdateQuestPanelUI(targetQuestRewardSprite, targetQuestRewardText,
                targetQuestDescriptionText);
        }
        
        public void TargetQuestClear()
        {
            var targetQuestIndex = questLevel % 5;
            
            AccountManager.Instance.AddCurrency((Enum.CurrencyType)System.Enum.Parse(typeof(Enum.QuestRewardType), $"{quests[targetQuestIndex].questRewardType}"), quests[targetQuestIndex].reward);

            questLevel++;
            ES3.Save($"{nameof(questLevel)}", questLevel);

            quests[targetQuestIndex].isCompleted = false;
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