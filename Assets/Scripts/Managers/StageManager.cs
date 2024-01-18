 using System;
using System.Collections;
using Controller.UI;
using ScriptableObjects.Scripts;
using UnityEngine;
using Enum = Data.Enum;

namespace Managers
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance;

        public static Action CheckRemainedMonster;
        public static Action CheckRemainedSquad;
        public static Action<bool> CheckStageProgressType;

        [SerializeField] private StageUI stageUIController;
        [SerializeField] private StageSo stageSo;
        [SerializeField] private int currentMainStage;
        [SerializeField] private int currentSubStage;
        [SerializeField] private int currentWave;
        [SerializeField] private int currentRemainedMonsterCount;
        [SerializeField] private float waveTime;

        [SerializeField] private bool waveClear;
        [SerializeField] private bool timeOver;
        [SerializeField] private bool squadAnnihilation;

        [SerializeField] private string currentMainStageName;
        [SerializeField] private bool nextStageChallenge;

        [SerializeField] private int currentSquadCount;

        private int maxSquadCount;
        private int maxMainStageCounts;
        private int subStageCountsPerMainStage;
        private int waveCountsPerSubStage;
        private int monsterSpawnCountsPerSubStage;
        private float stageLimitedTime;
        private bool GoToNextSubStage;

        private IEnumerator stageRunner;
        private IEnumerator waveTimer;

        [SerializeField] private bool isWaveTimerRunning;

        private void Awake()
        {
            Instance = this;
        }

        public void InitStageManager()
        {
            CheckRemainedMonster += CalculateRemainedMonster;
            CheckRemainedSquad += CalculateRemainedSquad;
            CheckStageProgressType += SetStageProgressType;

            maxMainStageCounts = stageSo.MainStageInfos.Count;
            subStageCountsPerMainStage = stageSo.SubStageCountsPerMainStage;
            waveCountsPerSubStage = stageSo.WaveCountsPerSubStage;
            monsterSpawnCountsPerSubStage = stageSo.MonsterSpawnCountsPerSubStage;
            stageLimitedTime = stageSo.StageLimitedTime;
            nextStageChallenge = true;
            GoToNextSubStage = true;

            waveTimer = WaveTimer();
            stageRunner = StageRunner();

            //TODO: Easy 뭐시깽이에서 불러와야 합미둥둥
            maxSquadCount = 3;
            currentMainStage = 1;
            currentSubStage = 1;
            currentWave = 1;
            currentSquadCount = 3;

            SetCurrentMainStageInfo();
        }

        private void SetCurrentMainStageInfo()
        {
            currentMainStageName = stageSo.MainStageInfos[currentMainStage - 1].MainStageName;
        }

        public void StartStageRunner()
        {
            StartCoroutine(stageRunner);
        }

        private void CalculateRemainedMonster()
        {
            currentRemainedMonsterCount--;

            if (currentRemainedMonsterCount == 0)
            {
                StopCoroutine(waveTimer);

                waveClear = true;

                currentWave++;

                if (currentWave > waveCountsPerSubStage)
                {
                    if (nextStageChallenge)
                    {
                        currentWave -= waveCountsPerSubStage;
                        currentSubStage++;

                        if (currentSubStage > subStageCountsPerMainStage)
                        {
                            GoToNextSubStage = true;
                            currentSubStage -= subStageCountsPerMainStage;
                            currentMainStage++;

                            if (currentMainStage > maxMainStageCounts)
                            {
                                currentMainStage = maxMainStageCounts;
                                currentSubStage = subStageCountsPerMainStage;
                            }

                            SetCurrentMainStageInfo();
                        }
                    }
                    else
                    {
                        GoToNextSubStage = true;
                        currentWave = 1;
                    }
                }

                SetCurrentMainStageInfo();
                StartCoroutine(StageRunner());
            }
        }

        private void CalculateRemainedSquad()
        {
            currentSquadCount--;

            if (currentSquadCount == 0) squadAnnihilation = true;
        }

        private void SetStageProgressType(bool challenge)
        {
            nextStageChallenge = challenge;
        }

        private IEnumerator StageRunner()
        {
            IniStageRunner();
            SetUI();


            yield return new WaitForSeconds(1.0f);

            if (GoToNextSubStage)
            {
                SpawnSquad();
                GoToNextSubStage = false;

                yield return new WaitForSeconds(1.0f);
            }

            SpawnMonster();
            StartCoroutine(waveTimer);
        }

        private void IniStageRunner()
        {
            squadAnnihilation = false;
            waveClear = false;
            timeOver = false;
        }

        private void SetUI()
        {
            stageUIController.SetUIText(Enum.UITextType.CurrentStageName, $"{currentMainStageName}{currentSubStage}");
            stageUIController.SetUIText(Enum.UITextType.CurrentWave, $"{currentWave} / {waveCountsPerSubStage}");
            stageUIController.SetUISlider(Enum.UISliderType.CurrentWaveSlider,
                1.0f * currentWave / waveCountsPerSubStage);
        }

        private void SpawnSquad()
        {
            currentSquadCount = maxSquadCount;
            SquadManager.Instance.SpawnSquad();
        }

        private void SpawnMonster()
        {
            currentRemainedMonsterCount = monsterSpawnCountsPerSubStage;
            MonsterManager.Instance.GenerateMonsters(stageSo.MainStageInfos[currentMainStage - 1].MainStageMonsterTypes,
                monsterSpawnCountsPerSubStage);
        }

        private IEnumerator WaveTimer()
        {
            isWaveTimerRunning = true;
            timeOver = false;

            waveTime = stageLimitedTime;

            while (0 < waveTime)
            {
                SetTimerUI((int)waveTime);
                waveTime -= Time.deltaTime;

                if (waveTime <= 0) timeOver = true;
                yield return null;
            }

            isWaveTimerRunning = false;
        }

        private void SetTimerUI(int currentTime)
        {
            stageUIController.SetUIText(Enum.UITextType.Timer, $"{currentTime}");
        }
    }
}