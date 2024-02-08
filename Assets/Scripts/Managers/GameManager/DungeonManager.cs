using System;
using System.Collections;
using Controller.UI.BattleMenuUI;
using Controller.UI.BottomMenuUI;
using Controller.UI.BottomMenuUI.BottomMenuPanel.DungeonPanel;
using Creature.CreatureClass.MonsterClass;
using Data;
using Function;
using Managers.BattleManager;
using ScriptableObjects.Scripts;
using UnityEngine;

namespace Managers.GameManager
{
    public class DungeonManager : MonoBehaviour
    {
        public static DungeonManager Instance;
        
        public static Action<BigInteger, BigInteger> CheckRemainedBossHealth;

        [SerializeField] private DungeonSo[] dungeonSo;
        [SerializeField] private StageUI stageUIController;

        [Header("=== 스테이지 UI 목록 ===")] [SerializeField]
        private GameObject[] stageUIs;
        
        [Header("=== 던전 UI 목록 ===")] [SerializeField]
        private GameObject[] dungeonUIs;

        [Header("=== 스테이지 맵 ===")] [SerializeField]
        private GameObject stageMap;
        
        [Header("=== 던전 맵 목록 ===")] [SerializeField]
        private GameObject[] dungeonMap;

        [Header("=== 전투 결과창 UI ===")] [SerializeField]
        private GameObject dungeonRewardResultUI;

        [Header("보스 몬스터 스폰 포지션")] [SerializeField] private Transform bossMonsterSpawnPosition;
        [Header("보스 몬스터 프리팹")] [SerializeField] private GameObject bossMonsterPrefab;
        [Header("보스 몬스터")] [SerializeField] private GameObject bossMonster;
        
        [Header("=== 던전 정보 ===")]
        [SerializeField] private Enums.DungeonClearType dungeonClearType;
        [SerializeField] private string currentDungeonName;
        [SerializeField] private int currentDungeonLevel;
        [SerializeField] private Enums.DungeonType currentDungeonType;
        [SerializeField] private Enums.CurrencyType currentDungeonRewardType;
        [SerializeField] private int currentScore;
        [SerializeField] private int targetScore;
        [SerializeField] private float waveTime;
        [SerializeField] private float stageLimitedTime = 60.0f;
        [SerializeField] private float currentSquadCount;
        [SerializeField] private float currentRemainedMonsterCount;
        [SerializeField] private BigInteger currentDungeonReward;

        [Header("--- 던전 러너 상태 ---")]
        [SerializeField] private bool nextStageChallenge;

        [SerializeField] private int maxSquadCount;
        [SerializeField] private bool isWaveTimerRunning;
        [SerializeField] private bool stopWaveTimer;
        [SerializeField] private int monsterSpawnCountsPerSubStage;
        [SerializeField] private bool isClear;

        [Header("=== 던전 보상 증가량 (%) ===")] public int increaseRewardPercent = 20;

        public int baseClearReward = 2000;
        public DungeonItemUI[] dungeonItems;

        private void Awake()
        {
            Instance = this;
        }

        public void InitDungeonManager()
        {
            InitializeEventListeners();
            UpdateDungeonPanelScrollViewAllItemUI();
        }

        private void InitializeEventListeners()
        {
            for (var i = 0; i < dungeonItems.Length; i++)
            {
                var index = i;
                dungeonItems[index].enterDungeonButton.onClick.AddListener(() =>
                {
                    switch (dungeonItems[index].dungeonType)
                    {
                        case Enums.DungeonType.GoldDungeon:
                            if (int.Parse(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.GoldDungeonTicket)) < 1) return;
                            AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.GoldDungeonTicket, 1);
                            break;
                        case Enums.DungeonType.SquadEnhanceStoneDungeon:
                            if (int.Parse(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.EnhanceDungeonTicket)) < 1) return;
                            AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.EnhanceDungeonTicket, 1);
                            break;
                    }
                    
                    UpdateDungeonPanelScrollViewAllItemUI();
                    
                    StageManager.Instance.StopStageRunner();

                    StageManager.CheckRemainedMonsterAction -= StageManager.Instance.CalculateRemainedMonster;
                    StageManager.CheckRemainedSquadAction -= StageManager.Instance.CalculateRemainedSquad;

                    foreach (var stageUI in stageUIs) stageUI.SetActive(false);

                    isClear = false;

                    currentDungeonName = dungeonSo[index].dungeonName;
                    currentDungeonLevel = ES3.Load($"{dungeonSo[index].dungeonType}/{nameof(currentDungeonLevel)}", 1);
                    currentDungeonType = dungeonSo[index].dungeonType;
                    currentDungeonRewardType = dungeonSo[index].rewardType;
                    currentScore = 0;
                    currentDungeonReward = dungeonSo[index].reward * (increaseRewardPercent + 100) * currentDungeonLevel / 100;
                    waveTime = dungeonSo[index].waveTime;
                    stageMap.SetActive(false);
                    
                    dungeonUIs[index].SetActive(true);
                    dungeonMap[index].SetActive(true);

                    if (dungeonItems[index].dungeonType == Enums.DungeonType.SquadEnhanceStoneDungeon)
                    {
                        CheckRemainedBossHealth += UpdateBossKillUI;
                        bossMonster = Instantiate(bossMonsterPrefab, bossMonsterSpawnPosition);
                        bossMonster.GetComponent<BossMonster>().InitializeBossMonsterData(currentDungeonLevel);
                        
                        QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.PlayEnhanceStoneDungeon, 1);

                        targetScore = 1;
                    }
                    else
                    {
                        monsterSpawnCountsPerSubStage = dungeonSo[index].monsterSpawnCountsPerSubStage;
                        targetScore = dungeonSo[index].targetScore;
                        
                        QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.PlayGoldDungeon, 1);
                    }
                    
                    StageManager.CheckRemainedMonsterAction += CalculateRemainedMonster;
                    StageManager.CheckRemainedSquadAction += CalculateRemainedSquad;

                    StartDungeonRunner(index);
                });
            }
        }

        private void StartDungeonRunner(int index)
        {
            switch (index)
            {
                case 0:
                    StartCoroutine(KillCountDungeonRunner());
                    break;
                case 1:
                    StartCoroutine(BossKillDungeonRunner());
                    break;
            }
        }

        private void UpdateDungeonPanelScrollViewAllItemUI()
        {
            for (var i = 0; i < dungeonItems.Length; i++)
            {
                var level = ES3.Load($"{dungeonSo[i].dungeonType}/{nameof(currentDungeonLevel)}", 1);
                BigInteger rewardCal = dungeonSo[i].reward * (increaseRewardPercent + 100) * level / 100;
                
                switch (dungeonItems[i].dungeonType)
                {
                    case Enums.DungeonType.GoldDungeon:
                        dungeonItems[i].UpdateButtonUI(int.Parse(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.GoldDungeonTicket)) >= 1);
                        break;
                    case Enums.DungeonType.SquadEnhanceStoneDungeon:
                        dungeonItems[i].UpdateButtonUI(int.Parse(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.EnhanceDungeonTicket)) >= 1);
                        break;
                }
                
                dungeonItems[i].UpdateDungeonItemUI(level, rewardCal);
            }
        }
        
        private void UpdateDungeonPanelScrollViewItemUI()
        {
            BigInteger rewardCal = dungeonSo[(int) currentDungeonType].reward * (increaseRewardPercent + 100) * currentDungeonLevel / 100;
            dungeonItems[(int) currentDungeonType].UpdateDungeonItemUI(currentDungeonLevel, rewardCal);
        }

        private void CalculateRemainedMonster()
        {
            currentRemainedMonsterCount--;
            currentScore++;

            UpdateKillCountUI();

            if (currentRemainedMonsterCount <= 0)
            {
                if (currentScore < targetScore)
                    SpawnMonster(currentDungeonLevel);
                else
                {
                    isClear = true;
                    StartCoroutine(RunStageRunner());
                }
            }
        }

        private void CalculateRemainedSquad()
        {
            --currentSquadCount;

            if (currentSquadCount <= 0)
            {
                isClear = false;
                StartCoroutine(RunStageRunner());   
            }
        }

        private void CalculateRemainedTime()
        {
            if (waveTime > 0) return;
            
            isClear = false;
            StartCoroutine(RunStageRunner());
        }

        private IEnumerator RunStageRunner()
        {
            currentSquadCount = 3;

            foreach (var dungeonUI in dungeonUIs)
            {
                dungeonUI.SetActive(false);
            }

            switch (currentDungeonType)
            {
                case Enums.DungeonType.GoldDungeon:
                    QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.PlayGoldDungeon, 1);
                    break;
                case Enums.DungeonType.SquadEnhanceStoneDungeon:
                    QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.PlayEnhanceStoneDungeon, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            StageManager.CheckRemainedMonsterAction -= CalculateRemainedMonster;
            StageManager.CheckRemainedSquadAction -= CalculateRemainedSquad;
            
            StageManager.CheckRemainedMonsterAction += StageManager.Instance.CalculateRemainedMonster;
            StageManager.CheckRemainedSquadAction += StageManager.Instance.CalculateRemainedSquad;

            stopWaveTimer = true;
            currentSquadCount = maxSquadCount;

            dungeonRewardResultUI.SetActive(true);
            Debug.Log("패널 온!");
            dungeonRewardResultUI.GetComponent<DungeonRewardPanelUI>().UpdateRewardUI(SpriteManager.Instance.GetCurrencySprite(currentDungeonRewardType), $"+ {currentDungeonReward.ChangeMoney()}");
            dungeonRewardResultUI.GetComponent<DungeonRewardPanelUI>().PopUpDungeonClearMessage(isClear);

            if (isClear)
            {
                currentDungeonLevel++;
                AccountManager.Instance.AddCurrency(currentDungeonRewardType, currentDungeonReward);
                UpdateDungeonPanelScrollViewItemUI();
                ES3.Save($"{currentDungeonType}/{nameof(currentDungeonLevel)}", currentDungeonLevel);
            }

            yield return new WaitForSeconds(1.5f);

            dungeonRewardResultUI.GetComponent<DungeonRewardPanelUI>().gameObject.SetActive(false);

            yield return new WaitForSeconds(1.0f);

            DespawnSquad();
            DespawnMonster();
            
            if (currentDungeonType == Enums.DungeonType.SquadEnhanceStoneDungeon) Destroy(bossMonster);
            
            SetTimerUI(60);

            StageManager.Instance.goToNextSubStage = true;
            StageManager.Instance.currentWave = 1;
            StageManager.Instance.isWaveTimerRunning = false;
            StageManager.Instance.initStageResult = true;
            StageManager.Instance.SetCurrentMainStageInfo();
            
            stageMap.SetActive(true);
            
            foreach (var map in dungeonMap) map.SetActive(false);
            foreach (var stageUI in stageUIs) stageUI.SetActive(true);
            
            StageManager.Instance.StartStageRunner();
        }

        private IEnumerator KillCountDungeonRunner()
        {
            currentRemainedMonsterCount = dungeonSo[0].targetScore;
            UpdateKillCountUI();

            yield return new WaitForSeconds(1.0f);

            DespawnSquad();
            DespawnMonster();
            SpawnSquad();

            yield return new WaitForSeconds(1.0f);

            SpawnMonster(currentDungeonLevel);
            if (isWaveTimerRunning == false) StartCoroutine(WaveTimer());
        }
        
        private IEnumerator BossKillDungeonRunner()
        {
            currentRemainedMonsterCount = dungeonSo[1].targetScore;
            UpdateBossKillUI(bossMonster.GetComponent<BossMonster>().currentBossHealth, bossMonster.GetComponent<BossMonster>().maxBossHealth);

            yield return new WaitForSeconds(1.0f);
            
            DespawnSquad();
            DespawnMonster();
            SpawnSquad();
            
            if (isWaveTimerRunning == false) StartCoroutine(WaveTimer());
        }

        private void UpdateKillCountUI()
        {
            dungeonUIs[0].GetComponent<DungeonUI>().UpdateDungeonAllUI(dungeonSo[0].dungeonName, $"{currentScore} / {targetScore}", int.Parse((currentScore * 100 / targetScore).ToString()));
        }

        private void UpdateBossKillUI(BigInteger currentHealth, BigInteger maxHealth)
        {
            dungeonUIs[1].GetComponent<DungeonUI>().UpdateDungeonAllUI(dungeonSo[1].dungeonName, $"{currentHealth.ChangeMoney()} / {maxHealth.ChangeMoney()}", int.Parse((currentHealth * 100 / maxHealth).ToString()));
        }
        
        private void SpawnSquad()
        {
            currentSquadCount = maxSquadCount;
            SquadBattleManager.Instance.SpawnSquad();
        }

        private void DespawnSquad()
        {
            SquadBattleManager.Instance.DespawnSquad();
        }

        private void SpawnMonster(int level)
        {
            currentRemainedMonsterCount = monsterSpawnCountsPerSubStage;
            //TODO: So로 빼서 몬스터 타입 지정해줄 것
            MonsterManager.Instance.SpawnMonsters(Enums.MonsterClassType.Human, level, monsterSpawnCountsPerSubStage);
        }

        private void DespawnMonster()
        {
            MonsterManager.Instance.DespawnAllMonster();
        }

        private IEnumerator WaveTimer()
        {
            isWaveTimerRunning = true;

            InitWaveTimer();

            dungeonUIs[(int) currentDungeonType].GetComponent<DungeonUI>().UpdateDungeonTimerUI($"{(int)waveTime}");

            while (true)
            {
                dungeonUIs[(int) currentDungeonType].GetComponent<DungeonUI>().UpdateDungeonTimerUI($"{(int)waveTime}");
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
            stageUIController.SetUIText(Enums.UITextType.Timer, $"{currentTime}");
        }
    }
}