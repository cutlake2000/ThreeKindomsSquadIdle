using System;
using System.Collections.Generic;
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
        public GameObject targetMark;
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
        
        [Header("Quest Mark")]
        public GameObject questMark;
        public List<Quest> quests;
        public List<QuestTarget> questTargets;

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
            currentQuest = quests[questLevel];
            targetQuestRewardSprite = SpriteManager.Instance.GetCurrencySprite((Enums.CurrencyType)Enum.Parse(typeof(Enums.QuestRewardType), $"{quests[questLevel].questRewardType}"));
            targetQuestRewardText = $"{quests[questLevel].reward}";
            targetQuestDescriptionText = $"{quests[questLevel].name}";

            switch (currentQuest.questType)
            {
                case Enums.QuestType.AttackTalentLevel:
                    if (TalentManager.Instance.talentItem[0].currentLevel >= currentQuest.targetProgress)
                    {
                        UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                    }
                    break;
                case Enums.QuestType.HealthTalentLevel:
                    if (TalentManager.Instance.talentItem[1].currentLevel >= currentQuest.targetProgress)
                    {
                        UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                    }
                    break;
                case Enums.QuestType.SummonWeapon:
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
                        UIManager.Instance.questPanelUI.completedMark.SetActive(true);
                    }
                    break;
                case Enums.QuestType.SummonGear:
                    break;
                case Enums.QuestType.AutoEquipHelmet:
                    break;
                case Enums.QuestType.AutoEquipArmor:
                    break;
                case Enums.QuestType.AutoEquipGauntlet:
                    break;
                case Enums.QuestType.SummonSquad:
                    break;
                case Enums.QuestType.EquipSquad:
                    break;
                case Enums.QuestType.UseSkill:
                    break;
                case Enums.QuestType.TouchAutoButton:
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
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateQuestPanelUI();
        }

        public void IncreaseQuestProgress(Enums.QuestType questType, int currentValue)
        {
            if (currentQuest.questType != questType) return;
            
            if (questType is Enums.QuestType.AttackTalentLevel or Enums.QuestType.HealthTalentLevel or Enums.QuestType.StageClear or Enums.QuestType.StageClear)
            {
                quests[questLevel].progress = currentValue; // TODO: +=, = ?   
            }
            else
            {
                quests[questLevel].progress += currentValue;   
            }

            if (quests[questLevel].progress >= quests[questLevel].targetProgress)
            {
                UIManager.Instance.questPanelUI.completedMark.SetActive(true);
            }
        }

        private void CreateQuestsFromCsv()
        {
            questCsv = Resources.Load<TextAsset>("CSV/GuideQuest");
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
            
            AccountManager.Instance.AddCurrency((Enums.CurrencyType)Enum.Parse(typeof(Enums.QuestRewardType), $"{quests[questLevel].questRewardType}"), quests[questLevel].reward);
            UIManager.Instance.questPanelUI.questResultPanelUI.gameObject.SetActive(true);
            
            questLevel++;
            ES3.Save($"{nameof(questLevel)}", questLevel);

            UIManager.Instance.questPanelUI.completedMark.SetActive(false);

            UpdateAllQuestProgress();
        }
    }
}