 using System;
using System.Collections;
using Controller.UI;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
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
        
        [Header("=== 스테이지 정보 ===")]
        [SerializeField] private string currentMainStageName;
        [SerializeField] private int currentMainStage;
        [SerializeField] private int currentSubStage;
        [SerializeField] private int currentWave;
        [SerializeField] private int currentRemainedMonsterCount;
        [SerializeField] private float waveTime;
        [SerializeField] private float stageLimitedTime;
        [SerializeField] private int currentSquadCount;

        [Header("--- 스테이지 러너 상태 ---")]
        [SerializeField] private bool waveClear;
        [SerializeField] private bool timeOver;
        [SerializeField] private bool squadAnnihilation;
        [SerializeField] private bool nextStageChallenge;
        [SerializeField] private int maxSquadCount;
        [SerializeField] private int maxMainStageCounts;
        [SerializeField] private int subStageCountsPerMainStage;
        [SerializeField] private int waveCountsPerSubStage;
        [SerializeField] private int monsterSpawnCountsPerSubStage;
        [SerializeField] private bool goToNextSubStage;
        [SerializeField] private bool isWaveTimerRunning;
        [SerializeField] private bool stopWaveTimer;

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
            // stageLimitedTime = stageSo.StageLimitedTime; //TODO : SO에서 제한 시간 받아오도록 추후 수정, 테스트 목적으로 인스펙터에서 임시 시간값 지정했음
            nextStageChallenge = true;
            goToNextSubStage = true;
            stopWaveTimer = false;

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
            StartCoroutine(StageRunner());
        }

        private void CalculateRemainedMonster()
        {
            currentRemainedMonsterCount--;

            if (currentRemainedMonsterCount != 0) return;
            
            waveClear = true;
            currentWave++;

            if (currentWave > waveCountsPerSubStage)
            {
                if (nextStageChallenge)
                {
                    currentWave -= waveCountsPerSubStage;
                    currentSubStage++;

                    stopWaveTimer = true;

                    if (currentSubStage > subStageCountsPerMainStage)
                    {
                        goToNextSubStage = true;
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
                    goToNextSubStage = true;
                    currentWave = 1;
                }
            }

            SetCurrentMainStageInfo();
            StartCoroutine(StageRunner());
        }

        private void CalculateRemainedSquad()
        {
            currentSquadCount--;

            if (currentSquadCount > 0) return;
            squadAnnihilation = true; // TODO: 추후 삭제
            stopWaveTimer = true;
            timeOver = false;
            goToNextSubStage = true;
            currentWave = 1;
            currentSquadCount = maxSquadCount;
            
            DespawnSquad();
                
            SetCurrentMainStageInfo();
            StartCoroutine(StageRunner());
        }

        private void CalculateRemainedTime()
        {
            if (waveTime > 0) return;
            timeOver = true; // TODO: 추후 삭제
            
            stopWaveTimer = true;
            
            squadAnnihilation = false;
            goToNextSubStage = true;
            currentWave = 1;
            
            DespawnSquad();
                
            SetCurrentMainStageInfo();
            StartCoroutine(StageRunner());
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

            if (goToNextSubStage)
            {
                DespawnSquad();
                DespawnMonster();
                SpawnSquad();
                goToNextSubStage = false;

                yield return new WaitForSeconds(1.0f);
            }

            SpawnMonster();
            if (isWaveTimerRunning == false) StartCoroutine(WaveTimer());
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

        private void DespawnSquad()
        {
            SquadManager.Instance.DespawnSquad();
        }

        private void SpawnMonster()
        {
            currentRemainedMonsterCount = monsterSpawnCountsPerSubStage;
            MonsterManager.Instance.SpawnMonsters(stageSo.MainStageInfos[currentMainStage - 1].MainStageMonsterTypes,
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
            timeOver = false;
            waveTime = stageLimitedTime;
        }

        private void SetTimerUI(int currentTime)
        {
            stageUIController.SetUIText(Enum.UITextType.Timer, $"{currentTime}");
        }
    }
}