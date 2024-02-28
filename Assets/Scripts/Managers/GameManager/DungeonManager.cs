using System;
using System.Collections;
using Controller.UI.BattleMenuUI;
using Controller.UI.BottomMenuUI;
using Controller.UI.BottomMenuUI.BottomMenuPanel.DungeonPanel;
using Creature.CreatureClass.MonsterClass;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.BottomMenuManager.SquadPanel;
using Resources.ScriptableObjects.Scripts;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers.GameManager
{
    public class DungeonManager : MonoBehaviour
    {
        public static DungeonManager Instance;
        
        public static Action<BigInteger, BigInteger> CheckRemainedBossHealth;

        [SerializeField] private DungeonSo[] dungeonSo;
        [SerializeField] private StageUI stageUI;

        [Header("=== 스테이지 UI 목록 ===")] [SerializeField]
        private GameObject[] stageUIs;
        
        [Header("=== 던전 UI 목록 ===")] [SerializeField]
        private GameObject[] dungeonUIs;
        
        [Header("=== 던전 맵 목록 ===")] [SerializeField]
        private GameObject[] dungeonMap;

        [Header("=== 전투 결과창 UI ===")] [SerializeField]
        private GameObject dungeonRewardResultUI;

        [Header("보스 몬스터 스폰 포지션")] [SerializeField] private Transform bossMonsterSpawnPosition;
        [Header("보스 몬스터 프리팹")] [SerializeField] private GameObject bossMonsterPrefab;
        [Header("보스 몬스터")] [SerializeField] private GameObject bossMonster;
        
        [Header("=== 던전 정보 ===")]
        // [SerializeField] private Enums.DungeonClearType dungeonClearType;
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

        [Header("=== 던전 러너 상태 ===")]
        public bool isDungeonRunnerRunning;
        [SerializeField] private int maxSquadCount;
        [SerializeField] private bool isWaveTimerRunning;
        [SerializeField] private bool stopWaveTimer;
        [SerializeField] private int monsterSpawnCountsPerSubStage;
        [SerializeField] private bool isClear;

        [Header("=== 던전 레벨 당 일반 몬스터 스탯 증가량 ===")] public int increaseNormalMonsterStatValue;
        [Header("=== 던전 레벨 당 보스 몬스터 스탯 증가량 (던전 레벨^(x/100)) ===")] public int increaseBossMonsterStatValuePercent;
        [Header("=== 던전 보상 증가량 (%) ===")] public int increaseRewardPercent = 20;

        private void Awake()
        {
            Instance = this;
        }

        public void InitDungeonManager()
        {
            isDungeonRunnerRunning = false;
            
            InitializeEventListeners();
            UpdateDungeonPanelScrollViewAllItemUI();
        }

        private void InitializeEventListeners()
        {
            for (var i = 0; i < UIManager.Instance.dungeonPanelUI.dungeonItems.Length; i++)
            {
                var index = i;
                UIManager.Instance.dungeonPanelUI.dungeonItems[index].enterDungeonButton.onClick.AddListener(() =>
                {
                    if (UIManager.Instance.dungeonPanelUI.dungeonItems[index].dungeonType == Enums.DungeonType.GoldDungeon && int.Parse(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.GoldDungeonTicket)) < 1) return;
                    if (UIManager.Instance.dungeonPanelUI.dungeonItems[index].dungeonType == Enums.DungeonType.EquipmentEnhanceStoneDungeon && int.Parse(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.EquipmentEnhanceStoneDungeonTicket)) < 1) return;
                    if (UIManager.Instance.dungeonPanelUI.dungeonItems[index].dungeonType == Enums.DungeonType.SquadEnhanceStoneDungeon && int.Parse(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.SquadEnhanceStoneDungeonTicket)) < 1) return;

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

                    StageManager.Instance.InactivateStageMap();
                    ProjectileManager.Instance.DestroyAllProjectile();
                    
                    dungeonUIs[index].SetActive(true);
                    dungeonMap[index].SetActive(true);

                    if (UIManager.Instance.dungeonPanelUI.dungeonItems[index].dungeonType == Enums.DungeonType.SquadEnhanceStoneDungeon)
                    {
                        CheckRemainedBossHealth += UpdateBossKillUI;
                        bossMonster = Instantiate(bossMonsterPrefab, bossMonsterSpawnPosition);
                        bossMonster.GetComponent<BossMonster>().InitializeBossMonsterData(currentDungeonLevel);
                        
                        QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.PlaySquadEnhanceStoneDungeon, 1);

                        targetScore = 1;
                    }
                    else
                    {
                        monsterSpawnCountsPerSubStage = dungeonSo[index].monsterSpawnCountsPerSubStage;
                        targetScore = dungeonSo[index].targetScore;

                        switch (dungeonSo[index].dungeonType)
                        {
                            case Enums.DungeonType.GoldDungeon:
                                QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.PlayGoldDungeon, 1);
                                break;
                            case Enums.DungeonType.EquipmentEnhanceStoneDungeon:
                                QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.PlayEquipmentEnhanceStoneDungeon, 1);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
             
                    }
                    
                    StageManager.CheckRemainedMonsterAction += CalculateRemainedMonster;
                    StageManager.CheckRemainedSquadAction += CalculateRemainedSquad;

                    isDungeonRunnerRunning = true;
                    StartDungeonRunner();
                });
            }
        }

        private void StartDungeonRunner()
        {
            SquadBattleManager.Instance.cameraController.InitializeCameraPosition();

            dungeonUIs[(int) currentDungeonType].GetComponent<DungeonUI>().UpdateDungeonTimerUI($"{(int)waveTime}");
            
            switch (currentDungeonType)
            {
                case Enums.DungeonType.GoldDungeon:
                    StartCoroutine(KillCountDungeonRunner());
                    break;
                case Enums.DungeonType.SquadEnhanceStoneDungeon:
                    StartCoroutine(BossKillDungeonRunner());
                    break;
                case Enums.DungeonType.EquipmentEnhanceStoneDungeon:
                    StartCoroutine(KillCountDungeonRunner());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateDungeonPanelScrollViewAllItemUI()
        {
            for (var i = 0; i < UIManager.Instance.dungeonPanelUI.dungeonItems.Length; i++)
            {
                var level = ES3.Load($"{dungeonSo[i].dungeonType}/{nameof(currentDungeonLevel)}", 1);
                BigInteger rewardCal = dungeonSo[i].reward * (increaseRewardPercent + 100) * level / 100;
                
                switch (UIManager.Instance.dungeonPanelUI.dungeonItems[i].dungeonType)
                {
                    case Enums.DungeonType.GoldDungeon:
                        UIManager.Instance.dungeonPanelUI.dungeonItems[i].UpdateButtonUI(int.Parse(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.GoldDungeonTicket)) >= 1);
                        break;
                    case Enums.DungeonType.SquadEnhanceStoneDungeon:
                        UIManager.Instance.dungeonPanelUI.dungeonItems[i].UpdateButtonUI(int.Parse(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.SquadEnhanceStoneDungeonTicket)) >= 1);
                        break;
                }
                
                UIManager.Instance.dungeonPanelUI.dungeonItems[i].UpdateDungeonItemUI(level, rewardCal);
            }
        }
        
        private void UpdateDungeonPanelScrollViewItemUI()
        {
            BigInteger rewardCal = dungeonSo[(int) currentDungeonType].reward * (increaseRewardPercent + 100) * currentDungeonLevel / 100;
            UIManager.Instance.dungeonPanelUI.dungeonItems[(int) currentDungeonType].UpdateDungeonItemUI(currentDungeonLevel, rewardCal);
        }

        private void CalculateRemainedMonster()
        {
            currentRemainedMonsterCount--;
            currentScore++;

            UpdateKillCountUI();

            if (currentRemainedMonsterCount <= 0)
            {
                if (currentScore < targetScore)
                {
                    switch (currentDungeonType)
                    {
                        case Enums.DungeonType.GoldDungeon:
                            SpawnMonster(Enums.MonsterClassType.Human);
                            break;
                        case Enums.DungeonType.EquipmentEnhanceStoneDungeon:
                            SpawnMonster(Enums.MonsterClassType.General);
                            break;
                    }
                }
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
            isDungeonRunnerRunning = false;

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
                    QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.PlaySquadEnhanceStoneDungeon, 1);
                    break;
                case Enums.DungeonType.EquipmentEnhanceStoneDungeon:
                    QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.PlayEquipmentEnhanceStoneDungeon, 1);
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
            dungeonRewardResultUI.GetComponent<DungeonRewardPanelUI>().UpdateRewardUI(SpriteManager.Instance.GetCurrencySprite(currentDungeonRewardType), $"+ {currentDungeonReward.ChangeMoney()}");
            dungeonRewardResultUI.GetComponent<DungeonRewardPanelUI>().PopUpDungeonClearMessage(isClear);

            if (isClear)
            {
                currentDungeonLevel++;
                
                switch (currentDungeonType)
                {
                    case Enums.DungeonType.GoldDungeon:
                        AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.GoldDungeonTicket, 1);
                        break;
                    case Enums.DungeonType.SquadEnhanceStoneDungeon:
                        AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.SquadEnhanceStoneDungeonTicket, 1);
                        break;
                    case Enums.DungeonType.EquipmentEnhanceStoneDungeon:
                        AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.EquipmentEnhanceStoneDungeonTicket, 1);
                        break;
                }
                
                AccountManager.Instance.AddCurrency(currentDungeonRewardType, currentDungeonReward);
                
                UpdateDungeonPanelScrollViewItemUI();
                
                ES3.Save($"{currentDungeonType}/{nameof(currentDungeonLevel)}", currentDungeonLevel);
            }
            
            yield return new WaitForSeconds(1.5f);

            dungeonRewardResultUI.GetComponent<DungeonRewardPanelUI>().gameObject.SetActive(false);

            yield return new WaitForSeconds(1.0f);

            DespawnSquad();
            DespawnMonster();
            
            ProjectileManager.Instance.DestroyAllProjectile();
            SquadBattleManager.Instance.cameraController.InitializeCameraPosition();
            
            if (currentDungeonType == Enums.DungeonType.SquadEnhanceStoneDungeon) Destroy(bossMonster);
            
            SetTimerUI(60);

            StageManager.Instance.prepareNewSubStage = true;
            StageManager.Instance.currentWave = 1;
            StageManager.Instance.isWaveTimerRunning = false;
            StageManager.Instance.initializeStageResultChecker = true;
            StageManager.Instance.SetCurrentMainStageInfo();
            
            // stageMap.SetActive(true);
            
            foreach (var map in dungeonMap) map.SetActive(false);
            foreach (var stageUI in stageUIs) stageUI.SetActive(true);
            UIManager.Instance.stageRewardPanelUI.gameObject.SetActive(false);
            
            StopAllCoroutines();
            StageManager.Instance.StartStageRunner();
        }

        private IEnumerator KillCountDungeonRunner()
        {
            currentRemainedMonsterCount = targetScore;
            UpdateKillCountUI();

            yield return new WaitForSeconds(1.0f);

            DespawnSquad();
            DespawnMonster();
            ProjectileManager.Instance.DestroyAllProjectile();
            SpawnSquad();
            SquadBattleManager.Instance.cameraController.SetCameraTarget(0);

            yield return new WaitForSeconds(1.0f);

            switch (currentDungeonType)
            {
                case Enums.DungeonType.GoldDungeon:
                    SpawnMonster(Enums.MonsterClassType.Human);
                    break;
                case Enums.DungeonType.EquipmentEnhanceStoneDungeon:
                    SpawnMonster(Enums.MonsterClassType.General);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (isWaveTimerRunning == false) StartCoroutine(WaveTimer());
        }
        
        private IEnumerator BossKillDungeonRunner()
        {
            currentRemainedMonsterCount = dungeonSo[2].targetScore;
            UpdateBossKillUI(bossMonster.GetComponent<BossMonster>().currentBossHealth, bossMonster.GetComponent<BossMonster>().maxBossHealth);

            yield return new WaitForSeconds(1.0f);
            
            DespawnSquad();
            DespawnMonster();
            ProjectileManager.Instance.DestroyAllProjectile();
            SpawnSquad();
            SquadBattleManager.Instance.cameraController.SetCameraTarget(0);
            
            if (isWaveTimerRunning == false) StartCoroutine(WaveTimer());
        }

        private void UpdateKillCountUI()
        {
            dungeonUIs[(int) currentDungeonType].GetComponent<DungeonUI>().UpdateDungeonAllUI(dungeonSo[(int) currentDungeonType].dungeonName, $"{currentScore} / {targetScore}", int.Parse((currentScore * 100 / targetScore).ToString()));
        }

        private void UpdateBossKillUI(BigInteger currentHealth, BigInteger maxHealth)
        {
            // dungeonUIs[1].GetComponent<DungeonUI>().UpdateDungeonAllUI(dungeonSo[1].dungeonName, $"{BigInteger.ToInt32(currentHealth * 100 / maxHealth).ToString()} %", BigInteger.ToInt32(currentHealth * 100 / maxHealth));
            dungeonUIs[2].GetComponent<DungeonUI>().UpdateDungeonAllUI(dungeonSo[2].dungeonName, $"{currentHealth.ChangeMoney()} / {maxHealth.ChangeMoney()}", BigInteger.ToInt64Safely(currentHealth * 100 / maxHealth));
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

        private void SpawnMonster(Enums.MonsterClassType monsterClassType)
        {
            currentRemainedMonsterCount = monsterSpawnCountsPerSubStage;
            
            MonsterManager.Instance.SpawnMonsters(monsterClassType, monsterSpawnCountsPerSubStage);
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
            stageUI.SetUIText(Enums.UITextType.Timer, $"{currentTime}");
        }
    }
}