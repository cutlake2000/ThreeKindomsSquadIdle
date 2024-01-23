using System;
using System.Collections;
using Controller.UI;
using Controller.UI.BottomMenuUI.DungeonPanel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Enum = Data.Enum;

namespace Managers
{
    public class DungeonManager : MonoBehaviour
    {
        public static DungeonManager Instance;
        
        public static Action CheckRemainedMonster;
        public static Action CheckRemainedSquad;
        
        [SerializeField] private StageUI stageUIController;

        [Header("=== UI On/Off 패널 목록 ===")]
        [SerializeField] private GameObject[] stageUIs;
        [Header("=== 던전 정보 ===")]
        [SerializeField] private Enum.DungeonClearType dungeonClearType;
        [SerializeField] private string currentDungeonName;
        [SerializeField] private int currentDungeonLevel;
        [SerializeField] private int currentScore;
        [SerializeField] private int targetScore;
        [SerializeField] private float waveTime;
        [SerializeField] private float stageLimitedTime = 60.0f;
        [SerializeField] private float currentSquadCount;
        [SerializeField] private float currentRemainedMonsterCount;
        
        [Header("--- 던전 러너 상태 ---")]
        [SerializeField] private bool nextStageChallenge;
        [SerializeField] private int maxSquadCount;
        [SerializeField] private bool isWaveTimerRunning;
        [SerializeField] private bool stopWaveTimer;
        [SerializeField] private int monsterSpawnCountsPerSubStage;

        [Header("=== 던전 보상 증가량 (%) ===")]
        public int increaseRewardPercent = 20;
        public int baseClearReward = 2000;
        public int maxDungeonLevel = 10;
        public DungeonItemUI[] dungeonItems;
        public Scene goldDungeonScene;

        private void Awake()
        {
            Instance = this;
        }
        
        public void InitDungeonManager()
        {
            InitializeEventListeners();
            SetDungeonData();
            UpdateAllDungeonUI();
        }

        private void InitializeEventListeners()
        {
            for (var i = 0; i < dungeonItems.Length; i++)
            {
                var index = i;
                dungeonItems[i].enterDungeonButton.onClick.AddListener(() =>
                {
                    StageManager.Instance.StopStageRunner();
                    
                    StageManager.CheckRemainedMonsterAction -= StageManager.Instance.CalculateRemainedMonster;
                    StageManager.CheckRemainedSquadAction -= StageManager.Instance.CalculateRemainedSquad;
                    
                    StageManager.CheckRemainedMonsterAction += CalculateRemainedMonster;
                    StageManager.CheckRemainedSquadAction += CalculateRemainedSquad;

                    foreach (var stageUI in stageUIs)
                    {
                        stageUI.SetActive(false);
                    }

                    currentDungeonName = "황금 미믹 던전";
                    currentDungeonLevel = 1;
                    currentScore = 0;
                    targetScore = 100;
                    waveTime = 60.0f;
                    monsterSpawnCountsPerSubStage = 10;

                    StageManager.Instance.isStageRunnerRunning = true;
                    
                    StartCoroutine(KillCountDungeonRunner());
                });
                
                dungeonItems[index].previousStageButton.onClick.AddListener(() => ChooseStage(index, -1));
                dungeonItems[index].nextStageButton.onClick.AddListener(() => ChooseStage(index, 1));
            }
        }
        
        private void ChooseStage(int index, int levelIndex)
        {
            dungeonItems[index].currentDungeonLevel += levelIndex;

            if (dungeonItems[index].currentDungeonLevel <= 0) dungeonItems[index].currentDungeonLevel = 1;
            if (dungeonItems[index].currentDungeonLevel <= maxDungeonLevel) dungeonItems[index].currentDungeonLevel = maxDungeonLevel;
            dungeonItems[index].currentDungeonReward = (int)(baseClearReward * Mathf.Pow((100 + increaseRewardPercent) / 100, dungeonItems[index].currentDungeonLevel));
            
            dungeonItems[index].SetSquadStatUI();
        }

        private void SetDungeonData()
        {
            for (var i = 0; i < dungeonItems.Length; i++)
            {
                dungeonItems[i].dungeonType = (Enum.DungeonType)i;
                dungeonItems[i].currentDungeonLevel = ES3.Load($"{nameof(Enum.DungeonType)}/{(Enum.DungeonType)i}/currentLevel : ", 1);
                dungeonItems[i].currentDungeonReward = (int)(baseClearReward * Mathf.Pow((100 + increaseRewardPercent) / 100, dungeonItems[i].currentDungeonLevel));

                dungeonItems[i].SetSquadStatUI();
            }
        }
        
        private void UpdateAllDungeonUI()
        {
            foreach (var dungeonItem in dungeonItems) dungeonItem.UpdateIncreaseDungeonUI();
        }

        private void CalculateRemainedMonster()
        {
            currentRemainedMonsterCount--;
            currentScore++;
            
            SetUI();

            if (currentRemainedMonsterCount <= 0)
            {
                if (currentScore < targetScore)
                {
                    SpawnMonster();   
                }
                else
                {
                    Debug.Log("성공");
                    
                    foreach (var stageUI in stageUIs)
                    {
                        stageUI.SetActive(true);
                    }
                }
            }
        }

        private void CalculateRemainedSquad()
        {
            currentSquadCount--;

            if (currentSquadCount > 0) return;
            stopWaveTimer = true;
            StageManager.Instance.currentWave = 1;
            currentSquadCount = maxSquadCount;
            
            DespawnSquad();
            
            Debug.Log("실패!");
            
            StageManager.Instance.goToNextSubStage = true;
            StageManager.Instance.SetCurrentMainStageInfo();
            StageManager.Instance.StartStageRunner();
            
            foreach (var stageUI in stageUIs)
            {
                stageUI.SetActive(true);
            }
        }

        private void CalculateRemainedTime()
        {
            if (waveTime > 0) return;
            stopWaveTimer = true;
            StageManager.Instance.currentWave = 1;
            currentSquadCount = maxSquadCount;
            
            DespawnSquad();
            
            Debug.Log("실패!");
            
            StageManager.Instance.goToNextSubStage = true;
                
            StageManager.Instance.SetCurrentMainStageInfo();
            StageManager.Instance.StartStageRunner();
            
            foreach (var stageUI in stageUIs)
            {
                stageUI.SetActive(true);
            }
        }
        
        public void UpgradeDungeonPanelDungeonLevel(Enum.SquadStatTypeBySquadPanel type)
        {
            // if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint, squadStatItem[(int)type].levelUpCost * SquadPanelUI.Instance.levelUpMagnification)) return;
            // if (squadStatItem[(int)type].upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;
            //
            // squadStatItem[(int)type].UpdateSquadStat(SquadPanelUI.Instance.levelUpMagnification);
            // SetUpgradeUI(squadStatItem[(int)type]);
            // SquadPanelUI.Instance.CheckRequiredStatPointOfMagnificationButton((int) type);

            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        private IEnumerator KillCountDungeonRunner()
        {
            SetUI();
            
            yield return new WaitForSeconds(1.0f);

            DespawnSquad();
            DespawnMonster();
            SpawnSquad();

            yield return new WaitForSeconds(1.0f);

            SpawnMonster();
            if (isWaveTimerRunning == false) StartCoroutine(WaveTimer());
        }

        private void SetUI()
        {
            stageUIController.SetUIText(Enum.UITextType.CurrentStageName, $"{currentDungeonName}{currentDungeonLevel}");
            stageUIController.SetUIText(Enum.UITextType.CurrentWave, $"{currentScore} / {targetScore}");
            stageUIController.SetUISlider(Enum.UISliderType.CurrentWaveSlider, 1.0f * currentScore / targetScore);
        }
        
        private void SpawnSquad()
        {
            currentSquadCount = maxSquadCount;
            SquadManager.Instance.SpawnSquad();
        }

        private void DespawnSquad()
        {
            SquadManager.Instance.DespawnSquad();
        }

        private void SpawnMonster()
        {
            currentRemainedMonsterCount = monsterSpawnCountsPerSubStage;
            //TODO: So로 빼서 몬스터 타입 지정해줄 것
            MonsterManager.Instance.SpawnMonsters(Enum.MonsterClassType.Human,
                monsterSpawnCountsPerSubStage);
        }
        
        private void DespawnMonster()
        {
            MonsterManager.Instance.DespawnAllMonster();
        }

        private IEnumerator WaveTimer()
        {
            isWaveTimerRunning = true;

            InitWaveTimer();
            SetTimerUI((int)waveTime);

            while (true)
            {
                SetTimerUI((int)waveTime);
                waveTime -= Time.deltaTime;
                CalculateRemainedTime();
                
                yield return null;

                if (!stopWaveTimer) continue;
                stopWaveTimer = false;
                    
                break;
            }

            isWaveTimerRunning = false;
        }

        private void InitWaveTimer()
        {
            stopWaveTimer = false;
            waveTime = stageLimitedTime;
        }

        private void SetTimerUI(int currentTime)
        {
            stageUIController.SetUIText(Enum.UITextType.Timer, $"{currentTime}");
        }
    }
}