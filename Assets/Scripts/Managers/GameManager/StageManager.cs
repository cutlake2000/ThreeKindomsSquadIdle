using System;
using System.Collections;
using System.Collections.Generic;
using Controller.UI;
using Controller.UI.BattleMenuUI;
using Controller.UI.BottomMenuUI;
using Controller.UI.BottomMenuUI.PopUpUI;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.BottomMenuManager.SquadPanel;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

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

        [SerializeField] private StageSo stageSo;
        
        [Header("=== 스테이지 UI ===")]
        [SerializeField] private StageUI stageUIController;

        [Header("=== 전투 결과창 UI ===")]
        public GameObject stageResultUI;
        
        [Header("=== 스테이지 정보 ===")]
        public int currentStageIndex; // 누적 스테이지 정보
        public int currentAccumulatedStage;
        [SerializeField] private string currentMainStageName;
        [SerializeField] private bool isClear;
        [SerializeField] private int currentMainStage;
        [SerializeField] private int currentSubStage;
        [SerializeField] public int currentWave;
        [SerializeField] private int currentRemainedMonsterCount;
        [SerializeField] private float waveTime;
        [SerializeField] private float stageLimitedTime = 60.0f;
        [SerializeField] private int currentSquadCount;

        [Header("=== 스테이지 보상 ===")]
        public StageReward[] stageRewards;
        
        [Header("=== 스테이지 맵 ===")]
        [SerializeField] private List<GameObject> stageMaps;
        
        [Header("=== 스테이지 러너 상태 ===")]
        public bool challengeProgress; // 돌파 / 반복 여부
        [SerializeField] private int maxSquadCount;
        [SerializeField] private int maxMainStageCounts;
        [SerializeField] private int subStageCountsPerMainStage;
        [SerializeField] private int waveCountsPerSubStage;
        [SerializeField] private int monsterSpawnCountsPerSubStage;
        [SerializeField] public bool isWaveTimerRunning;
        [SerializeField] private bool stopWaveTimer;
        [SerializeField] private bool isFinalStage;
        public bool prepareNewSubStage;
        public bool initializeStageResultChecker;

        private void Awake()
        {
            Instance = this;
        }

        public void InitStageManager()
        {
            CheckRemainedMonsterAction += CalculateRemainedMonster;
            CheckRemainedSquadAction += CalculateRemainedSquad;
            CheckStageProgressType += SetStageProgressType;
            
            challengeProgress = ES3.Load($"{nameof(challengeProgress)}", true);

            stageUIController.SetStageProgressButton(challengeProgress);
            
            maxMainStageCounts = stageSo.MainStageInfos.Count;
            subStageCountsPerMainStage = stageSo.SubStageCountsPerMainStage;
            
            waveCountsPerSubStage = stageSo.WaveCountsPerSubStage;
            monsterSpawnCountsPerSubStage = stageSo.MonsterSpawnCountsPerSubStage;
            stopWaveTimer = false;
            initializeStageResultChecker = true;
            prepareNewSubStage = true;
            isFinalStage = false;
            
            maxSquadCount = 3;
            currentSquadCount = 3;
            currentAccumulatedStage = ES3.Load($"{nameof(StageManager)}/{nameof(currentAccumulatedStage)}", 1);
            currentMainStage = ES3.Load($"{nameof(StageManager)}/{nameof(currentMainStage)}", 1);
            currentSubStage = ES3.Load($"{nameof(StageManager)}/{nameof(currentSubStage)}", 1);
            currentWave = 0;
            
            SetCurrentMainStageInfo();
            UpdateAllStageUI();
        }

        public void SetCurrentMainStageInfo()
        {
            currentMainStageName = $"{stageSo.MainStageInfos[currentMainStage - 1].MainStageName} {currentMainStage}-";
            
            if (!stageUIController.gameObject.activeInHierarchy) stageUIController.gameObject.SetActive(true);
        }

        public void StartStageRunner()
        {
            isWaveTimerRunning = false;
            
            StopAllCoroutines();
            StartCoroutine(StageRunner());
        }

        public void CalculateRemainedMonster()
        {
            currentRemainedMonsterCount--;

            if (currentRemainedMonsterCount > 0) return;
            
            isClear = true;
            currentWave++;

            if (currentWave > waveCountsPerSubStage - 1)
            {
                stopWaveTimer = true;
                prepareNewSubStage = true;
                
                if (challengeProgress)
                {
                    currentSubStage++;
                    currentAccumulatedStage++;

                    if (currentSubStage > subStageCountsPerMainStage)
                    {
                        currentSubStage -= subStageCountsPerMainStage;
                        currentMainStage++;

                        if (currentMainStage > maxMainStageCounts)
                        {
                            currentMainStage = maxMainStageCounts;
                            currentSubStage = subStageCountsPerMainStage;
                            
                            currentAccumulatedStage--;
                            
                            stageUIController.SetStageProgressButton(false);
                            challengeProgress = false;
                        }
                        
                        ES3.Save($"{nameof(StageManager)}/{nameof(currentMainStage)}", currentMainStage);
                    }
                    
                    QuestManager.Instance.IncreaseQuestProgress(Enums.QuestType.StageClear, currentAccumulatedStage);
                    
                    ES3.Save($"{nameof(StageManager)}/{nameof(currentSubStage)}", currentSubStage);
                    ES3.Save($"{nameof(StageManager)}/{nameof(currentAccumulatedStage)}", currentAccumulatedStage);
                }
            }
            
            StartCoroutine(StageRunner());
        }

        public void CalculateRemainedSquad()
        {
            currentSquadCount--;

            if (currentSquadCount > 0) return;
            
            isClear = false;
            stopWaveTimer = true;
            
            currentSubStage--;
            currentAccumulatedStage--;

            if (currentSubStage < 1)
            {
                if (currentMainStage > 1)
                {
                    currentMainStage--;
                    currentSubStage = subStageCountsPerMainStage;
                    ES3.Save($"{nameof(StageManager)}/{nameof(currentMainStage)}", currentMainStage);
                }
                else
                {
                    currentSubStage = 1;
                    currentAccumulatedStage = 1;
                }
            }

            challengeProgress = false;
            prepareNewSubStage = true;
            
            ES3.Save($"{nameof(challengeProgress)}", challengeProgress);
            ES3.Save($"{nameof(StageManager)}/{nameof(currentSubStage)}", currentSubStage);
            ES3.Save($"{nameof(StageManager)}/{nameof(currentAccumulatedStage)}", currentAccumulatedStage);
            
            CheckStageProgressType.Invoke(challengeProgress);
            
            StartCoroutine(StageRunner());
        }

        private void CalculateRemainedTime()
        {
            if (waveTime > 0) return;
            isClear = false;
            stopWaveTimer = true;
            
            currentSubStage--;
            currentAccumulatedStage--;
            
            if (currentSubStage < 1)
            {
                if (currentMainStage > 1)
                {
                    currentMainStage--;
                    currentSubStage = subStageCountsPerMainStage;
                    ES3.Save($"{nameof(StageManager)}/{nameof(currentMainStage)}", currentMainStage);
                }
                else
                {
                    currentSubStage = 1;
                }
            }
            
            challengeProgress = false;
            prepareNewSubStage = true;
            
            ES3.Save($"{nameof(challengeProgress)}", challengeProgress);
            ES3.Save($"{nameof(StageManager)}/{nameof(currentSubStage)}", currentSubStage);
            ES3.Save($"{nameof(StageManager)}/{nameof(currentAccumulatedStage)}", currentAccumulatedStage);
            
            CheckStageProgressType.Invoke(challengeProgress);
            
            StartCoroutine(StageRunner());
        }

        private void SetStageProgressType(bool challenge)
        {
            challengeProgress = challenge;
            ES3.Save($"{nameof(challengeProgress)}", challengeProgress);
        }

        private IEnumerator StageRunner()
        {
            var temp = currentMainStage % stageMaps.Count == 0 ? stageMaps.Count - 1 : currentMainStage % stageMaps.Count - 1;

            SetCurrentMainStageInfo();
            UpdateSliderUI();
            
            if (stageResultUI.activeInHierarchy) stageResultUI.SetActive(false);

            if (prepareNewSubStage)
            {
                isWaveTimerRunning = false;
                
                if (initializeStageResultChecker == false)
                {
                    stageResultUI.GetComponent<StageRewardPanelUI>().UpdateRewardUI(SpriteManager.Instance.GetCurrencySprite(stageRewards[0].rewardType), $"+ {stageRewards[0].GetStageReward(currentAccumulatedStage).ChangeMoney()}", SpriteManager.Instance.GetCurrencySprite(stageRewards[1].rewardType), $"+ {stageRewards[1].GetStageReward(currentAccumulatedStage).ChangeMoney()}");
                    stageResultUI.GetComponent<StageRewardPanelUI>().PopUpStageClearMessage(isClear);
                    stageResultUI.SetActive(true);
                    
                    yield return new WaitForSeconds(2f);
                }
                
                DespawnMonster();
                DespawnSquad();
                ProjectileManager.Instance.DestroyAllProjectile();
                SquadBattleManager.Instance.cameraController.InitializeCameraPosition();
                SetTimerUI((int)stageLimitedTime);
                
                for (var index = 0; index < stageMaps.Count; index++)
                {
                    var stageMap = stageMaps[index];
                
                    stageMap.SetActive(temp == index);
                }
                
                currentWave = 0;

                if (initializeStageResultChecker == false)
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
                        
                        stageResultUI.SetActive(false);
                    }
                    // else if (QuestManager.Instance.questLevel < 18)
                    // {
                    //     UpdateAllStageUI();
                    //     stageResultUI.SetActive(false);
                    // }

                    UpdateAllStageUI();
                        
                    yield return new WaitForSeconds(1.0f);
                }
                
                SpawnSquad();
                
                initializeStageResultChecker = false;
                isClear = false;
                prepareNewSubStage = false;
                
                SquadBattleManager.Instance.cameraController.SetCameraTarget(0);
                
                yield return new WaitForSeconds(1.0f);
                UpdateAllStageUI();
            }
        
            SpawnMonster();
            
            if (isWaveTimerRunning == false) StartCoroutine(WaveTimer());
        }

        public List<Reward> GetCurrentStageClearReward(int clearCount)
        {
            var rewards = new List<Reward>();

            foreach (var stageReward in stageRewards)
            {
                var reward = new Reward((Enums.RewardType) Enum.Parse(typeof(Enums.RewardType), stageReward.rewardType.ToString()), stageReward.GetStageReward(currentAccumulatedStage) * clearCount);
                rewards.Add(reward);
            }

            return rewards;
        }

        private void UpdateAllStageUI()
        {
            stageUIController.SetUIText(Enums.UITextType.CurrentStageName, $"{currentMainStageName}{currentSubStage}");
            stageUIController.SetUIText(Enums.UITextType.CurrentWave, $"{currentWave} / {waveCountsPerSubStage}");
            stageUIController.SetUISlider(Enums.UISliderType.CurrentWaveSlider, 1.0f * currentWave / waveCountsPerSubStage);
            
            isFinalStage = currentMainStage == maxMainStageCounts && currentSubStage == subStageCountsPerMainStage;
            stageUIController.UpdateFinalStageEffect(isFinalStage);
            if (isFinalStage) stageUIController.SetStageProgressButton(false);
        }

        private void UpdateSliderUI()
        {
            stageUIController.SetUIText(Enums.UITextType.CurrentWave, $"{currentWave} / {waveCountsPerSubStage}");
            stageUIController.SetUISlider(Enums.UISliderType.CurrentWaveSlider, 1.0f * currentWave / waveCountsPerSubStage);
        }

        private void SpawnSquad()
        {
            currentSquadCount += maxSquadCount;
            SquadBattleManager.Instance.SpawnSquad();
        }

        private void DespawnSquad()
        {
            currentSquadCount = 0;
            SquadBattleManager.Instance.DespawnSquad();
        }

        private void SpawnMonster()
        {
            currentRemainedMonsterCount += monsterSpawnCountsPerSubStage;
            MonsterManager.Instance.SpawnMonsters(stageSo.MainStageInfos[currentMainStage - 1].MainStageMonsterTypes, monsterSpawnCountsPerSubStage);
        }

        private void DespawnMonster()
        {
            currentRemainedMonsterCount = 0;
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

        public void InactivateStageMap()
        {
            foreach (var stageMap in stageMaps)
            {
                stageMap.SetActive(false);
            }
        }
    }
}