using System;
using System.Collections;
using Controller.UI.BattleMenuUI;
using Controller.UI.BottomMenuUI;
using Data;
using Function;
using Managers.BattleManager;
using ScriptableObjects.Scripts;
using UnityEngine;

namespace Managers.GameManager
{
    [Serializable]
    public class StageReward
    {
        public Enums.CurrencyType rewardType;
        public int baseReward;
        public int increaseValue;

        public BigInteger GetStageReward(int level)
        {
            return baseReward + increaseValue * (level - 1);
        }
    }
    
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance;
        
        public static Action CheckRemainedMonsterAction;
        public static Action CheckRemainedSquadAction;
        public static Action<bool> CheckStageProgressType;

        [SerializeField] private StageUI stageUIController;
        [SerializeField] private StageSo stageSo;

        public StageReward[] stageRewards;
        
        [Header("=== 전투 결과창 UI ===")]
        public GameObject stageResultUI;

        [Header("=== 스테이지 정보 ===")]
        [SerializeField] private string currentMainStageName;
        public int currentStageIndex; // 누적 스테이지 정보
        [SerializeField] private bool isClear;
        public int currentAccumulatedStage;
        [SerializeField] private int currentMainStage;
        [SerializeField] private int currentSubStage;
        [SerializeField] public int currentWave;
        [SerializeField] private int currentRemainedMonsterCount;
        [SerializeField] private float waveTime;
        [SerializeField] private float stageLimitedTime = 60.0f;
        [SerializeField] private int currentSquadCount;

        [Header("--- 스테이지 러너 상태 ---")]
        [SerializeField] private bool nextStageChallenge;
        [SerializeField] private int maxSquadCount;
        [SerializeField] private int maxMainStageCounts;
        [SerializeField] private int subStageCountsPerMainStage;
        [SerializeField] private int waveCountsPerSubStage;
        [SerializeField] private int monsterSpawnCountsPerSubStage;
        [SerializeField] public bool goToNextSubStage;
        [SerializeField] public bool isWaveTimerRunning;
        [SerializeField] private bool stopWaveTimer;
        public bool initStageResult;

        private void Awake()
        {
            Instance = this;
        }

        public void InitStageManager()
        {
            CheckRemainedMonsterAction += CalculateRemainedMonster;
            CheckRemainedSquadAction += CalculateRemainedSquad;
            CheckStageProgressType += SetStageProgressType;

            nextStageChallenge = ES3.Load("CheckStageProgressType", true);

            stageUIController.SetStageProgressButton(nextStageChallenge);
            
            maxMainStageCounts = stageSo.MainStageInfos.Count;
            subStageCountsPerMainStage = stageSo.SubStageCountsPerMainStage;
            waveCountsPerSubStage = stageSo.WaveCountsPerSubStage;
            monsterSpawnCountsPerSubStage = stageSo.MonsterSpawnCountsPerSubStage;
            nextStageChallenge = true;
            goToNextSubStage = true;
            stopWaveTimer = false;
            initStageResult = true;
            
            maxSquadCount = 3;
            currentSquadCount = 3;
            currentAccumulatedStage = ES3.Load($"{nameof(StageManager)}/{nameof(currentAccumulatedStage)}", 1);
            currentMainStage = ES3.Load($"{nameof(StageManager)}/{nameof(currentMainStage)}", 1);
            currentSubStage = ES3.Load($"{nameof(StageManager)}/{nameof(currentSubStage)}", 1);
            currentWave = 0;
            
            SetCurrentMainStageInfo();
        }

        public void SetCurrentMainStageInfo()
        {
            currentMainStageName = stageSo.MainStageInfos[currentMainStage - 1].MainStageName;
        }

        public void StartStageRunner()
        {
            StartCoroutine(StageRunner());
        }

        public void CalculateRemainedMonster()
        {
            currentRemainedMonsterCount--;

            if (currentRemainedMonsterCount > 0) return;
            
            currentWave++;

            if (currentWave > waveCountsPerSubStage - 1)
            {
                isClear = true;
                
                if (nextStageChallenge)
                {
                    currentSubStage++;
                    currentAccumulatedStage++;
                    QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.StageClear, currentAccumulatedStage);

                    stopWaveTimer = true;
                    goToNextSubStage = true;

                    if (currentSubStage > subStageCountsPerMainStage)
                    {
                        currentSubStage -= subStageCountsPerMainStage;
                        currentMainStage++;

                        if (currentMainStage > maxMainStageCounts)
                        {
                            currentMainStage = maxMainStageCounts;
                            currentSubStage = subStageCountsPerMainStage;
                        }
                        
                        ES3.Save($"{nameof(StageManager)}/{nameof(currentMainStage)}", currentMainStage);

                        SetCurrentMainStageInfo();
                    }
                    
                    QuestManager.Instance.IncreaseQuestProgress(Enums.QuestType.StageClear, currentAccumulatedStage);
                    ES3.Save($"{nameof(StageManager)}/{nameof(currentSubStage)}", currentSubStage);
                    ES3.Save($"{nameof(StageManager)}/{nameof(currentAccumulatedStage)}", currentAccumulatedStage);
                }
                else
                {
                    goToNextSubStage = true;

                    ES3.Save($"{nameof(StageManager)}/{nameof(currentStageIndex)}", currentSubStage);
                }
            }

            SetCurrentMainStageInfo();
            StartCoroutine(StageRunner());
        }

        public void CalculateRemainedSquad()
        {
            currentSquadCount--;

            if (currentSquadCount > 0) return;
            isClear = false;
            stopWaveTimer = true;
            goToNextSubStage = true;
            currentSquadCount = maxSquadCount;
            
            currentSubStage--;

            if (currentSubStage < 1)
            {
                if (currentMainStage > 1)
                {
                    currentMainStage--;
                }
                
                currentSubStage = subStageCountsPerMainStage;
            }

            nextStageChallenge = false;
            CheckStageProgressType.Invoke(nextStageChallenge);
            
            SetCurrentMainStageInfo();
            StartCoroutine(StageRunner());
        }

        private void CalculateRemainedTime()
        {
            if (waveTime > 0) return;
            isClear = false;
            stopWaveTimer = true;
            goToNextSubStage = true;
            
            currentSubStage--;
            currentAccumulatedStage--;

            if (currentSubStage < 1)
            {
                if (currentMainStage > 1)
                {
                    currentMainStage--;
                    currentSubStage = subStageCountsPerMainStage;
                }
                else
                {
                    currentSubStage = 1;
                    currentAccumulatedStage++;
                }
            }
            
            nextStageChallenge = false;
            CheckStageProgressType.Invoke(nextStageChallenge);
            
            SetCurrentMainStageInfo();
            StartCoroutine(StageRunner());
        }

        private void SetStageProgressType(bool challenge)
        {
            nextStageChallenge = challenge;
            ES3.Save("CheckStageProgressType", nextStageChallenge);
        }

        private IEnumerator StageRunner()
        {
            SetUI();

            yield return new WaitForSeconds(1.0f);

            if (goToNextSubStage)
            {
                SetUI();
                
                if (!initStageResult)
                {
                    stageResultUI.GetComponent<StageRewardPanelUI>().UpdateRewardUI(SpriteManager.Instance.GetCurrencySprite(stageRewards[0].rewardType), $"+ {stageRewards[0].GetStageReward(currentAccumulatedStage).ChangeMoney()}", SpriteManager.Instance.GetCurrencySprite(stageRewards[1].rewardType), $"+ {stageRewards[1].GetStageReward(currentAccumulatedStage).ChangeMoney()}");
                    stageResultUI.GetComponent<StageRewardPanelUI>().PopUpStageClearMessage(isClear);
                    stageResultUI.SetActive(true);
                    
                    yield return new WaitForSeconds(2f);
                }
                
                DespawnSquad();
                DespawnMonster();

                if (!initStageResult)
                {
                    if (isClear)
                    {
                        foreach (var reward in stageRewards)
                        {
                            if (reward.rewardType != Enums.CurrencyType.Exp)
                            {
                                AccountManager.Instance.AddCurrency(reward.rewardType, reward.GetStageReward(currentAccumulatedStage));   
                            }
                            else
                            {
                                AccountManager.Instance.AddExp(reward.GetStageReward(currentAccumulatedStage));
                            }
                        }
                    }
                    
                    stageResultUI.GetComponent<StageRewardPanelUI>().gameObject.SetActive(false);
                }

                yield return new WaitForSeconds(1.0f);
                
                SpawnSquad();
                currentWave = 0;
                goToNextSubStage = false;
                initStageResult = false;
                
                SetUI();
                
                yield return new WaitForSeconds(1.0f);
            }

            SpawnMonster();
            if (isWaveTimerRunning == false) StartCoroutine(WaveTimer());
        }

        private void SetUI()
        {
            stageUIController.SetUIText(Enums.UITextType.CurrentStageName, $"{currentMainStageName}{currentSubStage}");
            stageUIController.SetUIText(Enums.UITextType.CurrentWave, $"{currentWave} / {waveCountsPerSubStage}");
            stageUIController.SetUISlider(Enums.UISliderType.CurrentWaveSlider,
                1.0f * currentWave / waveCountsPerSubStage);
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
            MonsterManager.Instance.SpawnMonsters(stageSo.MainStageInfos[currentMainStage - 1].MainStageMonsterTypes, currentAccumulatedStage, monsterSpawnCountsPerSubStage);
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
            stageUIController.SetUIText(Enums.UITextType.Timer, $"{currentTime}");
        }

        public void StopStageRunner()
        {
            StopAllCoroutines();
            
            DespawnSquad();
            DespawnMonster();
        }
    }
}