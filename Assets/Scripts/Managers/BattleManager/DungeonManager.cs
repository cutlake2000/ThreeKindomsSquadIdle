using System;
using System.Collections;
using Controller.UI;
using Controller.UI.BottomMenuUI;
using Controller.UI.BottomMenuUI.DungeonPanel;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enum = Data.Enum;

namespace Managers.BattleManager
{
    public class DungeonManager : MonoBehaviour
    {
        public static DungeonManager Instance;

        public static Action CheckRemainedMonster;
        public static Action CheckRemainedSquad;

        [SerializeField] private StageUI stageUIController;

        [Header("=== UI On/Off 패널 목록 ===")] [SerializeField]
        private GameObject[] stageUIs;

        [Header("=== 전투 결과창 UI ===")] [SerializeField]
        private GameObject stageResultUI;

        [Header("=== 던전 정보 ===")] [SerializeField]
        private Enum.DungeonClearType dungeonClearType;

        [SerializeField] private string currentDungeonName;
        [SerializeField] private int currentDungeonLevel;
        [SerializeField] private int currentScore;
        [SerializeField] private int targetScore;
        [SerializeField] private float waveTime;
        [SerializeField] private float stageLimitedTime = 60.0f;
        [SerializeField] private float currentSquadCount;
        [SerializeField] private float currentRemainedMonsterCount;

        [Header("--- 던전 러너 상태 ---")] [SerializeField]
        private bool nextStageChallenge;

        [SerializeField] private int maxSquadCount;
        [SerializeField] private bool isWaveTimerRunning;
        [SerializeField] private bool stopWaveTimer;
        [SerializeField] private int monsterSpawnCountsPerSubStage;

        [Header("=== 던전 보상 증가량 (%) ===")] public int increaseRewardPercent = 20;

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
            InitiateDungeonData();
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

                    foreach (var stageUI in stageUIs) stageUI.SetActive(false);



                    //TODO: So 등에서 값을 가져오도록
                    currentDungeonName = "황금 미믹 던전";
                    currentDungeonLevel = 1;
                    currentScore = 0;
                    targetScore = 100;
                    waveTime = 60.0f;
                    monsterSpawnCountsPerSubStage = 10;

                    StartCoroutine(KillCountDungeonRunner());
                });

                // dungeonItems[index].previousStageButton.onClick.AddListener(() => ChooseStage(index, -1));
                // dungeonItems[index].nextStageButton.onClick.AddListener(() => ChooseStage(index, 1));
            }
        }

        private void ChooseStage(int index, int levelIndex)
        {
            Debug.Log($"{levelIndex} levelIndex");

            dungeonItems[index].currentDungeonLevel += levelIndex;

            if (dungeonItems[index].currentDungeonLevel == 0)
                dungeonItems[index].currentDungeonLevel = 1;
            else if (dungeonItems[index].currentDungeonLevel > maxDungeonLevel)
                dungeonItems[index].currentDungeonLevel = maxDungeonLevel;
            else
                switch (levelIndex)
                {
                    case -1:
                        dungeonItems[index].currentDungeonReward = dungeonItems[index].currentDungeonReward * 100 /
                                                                   (100 + increaseRewardPercent);
                        break;
                    case 1:
                        dungeonItems[index].currentDungeonReward = dungeonItems[index].currentDungeonReward *
                            (100 + increaseRewardPercent) / 100;
                        break;
                }

            dungeonItems[index].SetDungeonUI();
        }

        private void InitiateDungeonData()
        {
            for (var i = 0; i < dungeonItems.Length; i++)
            {
                dungeonItems[i].dungeonType = (Enum.DungeonType)i;
                dungeonItems[i].clearDungeonLevel =
                    ES3.Load($"{nameof(Enum.DungeonType)}/{(Enum.DungeonType)i}/clearDungeonLevel : ", 1);
                dungeonItems[i].currentDungeonLevel = dungeonItems[i].clearDungeonLevel;
                dungeonItems[i].currentDungeonReward = baseClearReward;

                dungeonItems[i].SetDungeonUI();
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
                    SpawnMonster();
                else
                    StartCoroutine(RunStageRunner(true));
            }
        }

        private void CalculateRemainedSquad()
        {
            currentSquadCount--;

            if (currentSquadCount > 0) return;

            StartCoroutine(RunStageRunner(false));
        }

        private void CalculateRemainedTime()
        {
            if (waveTime > 0) return;

            StartCoroutine(RunStageRunner(false));
        }

        private IEnumerator RunStageRunner(bool isClear)
        {
            StageManager.CheckRemainedMonsterAction += StageManager.Instance.CalculateRemainedMonster;
            StageManager.CheckRemainedSquadAction += StageManager.Instance.CalculateRemainedSquad;

            StageManager.CheckRemainedMonsterAction -= CalculateRemainedMonster;
            StageManager.CheckRemainedSquadAction -= CalculateRemainedSquad;

            stopWaveTimer = true;
            currentSquadCount = maxSquadCount;

            stageResultUI.SetActive(true);
            Debug.Log("패널 온!");
            stageResultUI.GetComponent<StageResultPanelUI>().PopUpStageClearMessage(isClear);

            yield return new WaitForSeconds(1.5f);

            stageResultUI.GetComponent<StageResultPanelUI>().PopUnderStageClearMessage();
            stageResultUI.SetActive(false);

            yield return new WaitForSeconds(1.0f);

            DespawnSquad();
            DespawnMonster();

            SetTimerUI((int)waveTime);

            StageManager.Instance.currentWave = 1;
            StageManager.Instance.isWaveTimerRunning = false;
            StageManager.Instance.goToNextSubStage = true;
            StageManager.Instance.SetCurrentMainStageInfo();
            StageManager.Instance.StartStageRunner();

            foreach (var stageUI in stageUIs) stageUI.SetActive(true);
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
            SquadBattleManager.Instance.SpawnSquad();
        }

        private void DespawnSquad()
        {
            SquadBattleManager.Instance.DespawnSquad();
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