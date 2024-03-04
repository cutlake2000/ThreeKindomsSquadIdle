using System;
using System.Collections;
using System.Collections.Generic;
using Controller.UI;
using Controller.UI.BattleMenuUI;
using Controller.UI.BottomMenuUI.BottomMenuPanel.InventoryPanel;
using Creature.CreatureClass.SquadClass;
using Creature.Data;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.BottomMenuManager.InventoryPanel;
using Managers.BottomMenuManager.SquadPanel;
using Managers.BottomMenuManager.TalentPanel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.GameManager
{
    [Serializable]
    public struct SkillCoolTimer
    {
        public GameObject skill;
        public bool orderToInstantiate;
        public bool isSkillReady;
        public float remainedSkillCoolTime;
        public float maxSkillCoolTime;
    }

    public class SquadBattleManager : MonoBehaviour
    {
        public static SquadBattleManager Instance;

        [Header("=== Camera Settings=== ")] [SerializeField]
        public CameraController cameraController;

        [Space(5)] [Header("=== Squad Position Info ===")]
        public GameObject[] squads;

        [SerializeField] private Vector3[] squadSpawnPosition;

        [Space(5)] [Header("=== Squad Battle Info ===")]
        [Header("공격 범위")]
        [SerializeField] private float warriorAttackRange;
        [SerializeField] private float archerAttackRange;
        [SerializeField] private float wizardAttackRange;

        [Header("적 탐지 범위")] [SerializeField] private float followRange;

        [Header("이동 속도")] [SerializeField] private float moveSpeed;
        [Header("이동 Multiplier")] public int[] squadMoveSpeedMultiplier;

        [Space(3)]
        [Header("=== 스킬 쿨타임 ===")] //TODO: 스킬 쿨 다운을 ScriptableObject에서 긁어와야 함
        [Header("자동 스킬 여부")] public bool autoSkill;
        [Header("워리어")] public SkillCoolTimer[] warriorSkillCoolTimer;
        [Header("아처")] public SkillCoolTimer[] archerSkillCoolTimer;
        [Header("위자드")] public SkillCoolTimer[] wizardSkillCoolTimer;

        [Space(5)] [Header("=== Squad Stats Info ===")]
        [Header("스킬 데미지")]
        [SerializeField] public List<int> warriorSkillDamagePercent;
        [SerializeField] public List<int> archerSkillDamagePercent;
        [SerializeField] public List<int> wizardSkillDamagePercent;

        [Header("Base SquadStats")] [SerializeField]
        public SquadEntireStat squadEntireStat;
        
        public BigInteger TotalAttack = new();
        public BigInteger TotalWarriorAttack = new();
        public BigInteger TotalArcherAttack = new();
        public BigInteger TotalWizardAttack = new();
     
        public BigInteger TotalMaxHealth = new();
        public BigInteger TotalWarriorHealth = new();
        public BigInteger TotalArcherHealth = new();
        public BigInteger TotalWizardHealth = new();
        
        public BigInteger TotalDefence = new();
        public BigInteger TotalWarriorDefence = new();
        public BigInteger TotalArcherDefence = new();
        public BigInteger TotalWizardDefence = new();
        
        public BigInteger TotalPenetration = new();
        public BigInteger TotalAccuracy = new();
        public BigInteger TotalCriticalRate = new();
        public BigInteger TotalCriticalDamage = new();
        public BigInteger TotalAcquisitionGold = new();
        public BigInteger TotalAcquisitionExp = new();

        [Header("피해량 보정 (%) ")] public int totalAttackAdjustValue;
        [Header("클래스 별 데미지 피해량 보정 (%) ")] public int totalDamageReduction;
        [Header("워리어 공격력 / 체력 / 방어력 보정치 (%)")] public int[] warriorTotalStatAdjustValue;
        [Header("아처 공격력 / 체력 / 방어력 보정치 보정치 (%)")] public int[] archerTotalStatAdjustValue;
        [Header("위자드 공격력 / 체력 / 방어력 보정치 보정치 (%)")] public int[] wizardTotalStatAdjustValue;

        public SquadLevel SquadLevel;

        private void Awake()
        {
            Instance = this;
            SquadLevel = new SquadLevel();
            autoSkill = ES3.Load("AutoSkill", false);
        }

        public void InitSquadBattleManager()
        {
            cameraController.InitCameraTarget();
            
            InitializeEventListeners();
            SetSquadStatsFromBaseStats();
            SetSkillCoolTimer();
            // SetupPlayerLevel();
        }

        private void SetSkillCoolTimer()
        {
            for (var i = 0; i < warriorSkillCoolTimer.Length; i++)
            {
                warriorSkillCoolTimer[i].isSkillReady = true;
                warriorSkillCoolTimer[i].remainedSkillCoolTime = warriorSkillCoolTimer[i].maxSkillCoolTime;
            }
        }


        private void SetSquadStatsFromBaseStats()
        {
            squadEntireStat.UpdatePercentStat(Enums.SquadStatType.CriticalRate, 0);
            squadEntireStat.UpdatePercentStat(Enums.SquadStatType.CriticalDamage, 0);
            squadEntireStat.UpdatePercentStat(Enums.SquadStatType.AcquisitionGold, 0);
            squadEntireStat.UpdatePercentStat(Enums.SquadStatType.AcquisitionExp, 0);
            
            TotalAttack = squadEntireStat.baseAttack;
            TotalWarriorAttack = TotalAttack;
            TotalArcherAttack = TotalAttack;
            TotalWizardAttack = TotalAttack;

            TotalMaxHealth = squadEntireStat.baseHealth;
            TotalWarriorHealth = TotalMaxHealth;
            TotalArcherHealth = TotalMaxHealth;
            TotalWizardHealth = TotalMaxHealth;
            
            TotalDefence = squadEntireStat.baseDefense;
            TotalWarriorDefence = TotalDefence;
            TotalArcherDefence = TotalDefence;
            TotalWizardDefence = TotalDefence;
            
            TotalPenetration = squadEntireStat.basePenetration;
            TotalAccuracy = squadEntireStat.baseAccuracy;
            TotalCriticalRate = squadEntireStat.baseCriticalRate;
            TotalCriticalDamage = squadEntireStat.baseCriticalRate;
            TotalAcquisitionGold = squadEntireStat.baseAcquisitionGold;
            TotalAcquisitionExp = squadEntireStat.baseAcquisitionExp;
        }

        // 이벤트 설정하는 메서드
        private void InitializeEventListeners()
        {
            SquadStatManager.Instance.OnUpgradeTotalSquadStatFromSquadStatPanel += squadEntireStat.UpdateStat;
            TalentManager.Instance.OnUpgradeTotalSquadStatFromSquadTalentPanel += squadEntireStat.UpdateStat; //TODO : 재능 스탯 증가
        }

        public BigInteger GetTotalSquadStat(Enums.SquadStatType statusType)
        {
            switch (statusType)
            {
                case Enums.SquadStatType.Attack:
                    return TotalAttack;
                case Enums.SquadStatType.WarriorAtk:
                    return TotalWarriorAttack * warriorTotalStatAdjustValue[0] / 100;
                case Enums.SquadStatType.ArcherAtk:
                    return TotalArcherAttack * archerTotalStatAdjustValue[0] / 100;
                case Enums.SquadStatType.WizardAtk:
                    return TotalWizardAttack * wizardTotalStatAdjustValue[0] / 100;
                
                case Enums.SquadStatType.Health:
                    return TotalMaxHealth;
                case Enums.SquadStatType.WarriorHealth:
                    return TotalMaxHealth * warriorTotalStatAdjustValue[1] / 100;
                case Enums.SquadStatType.ArcherHealth:
                    return TotalMaxHealth * archerTotalStatAdjustValue[1] / 100;
                case Enums.SquadStatType.WizardHealth:
                    return TotalMaxHealth * wizardTotalStatAdjustValue[1] / 100;
                
                case Enums.SquadStatType.Defence:
                    return TotalDefence;
                case Enums.SquadStatType.WarriorDefence:
                    return TotalDefence * warriorTotalStatAdjustValue[2] / 100;
                case Enums.SquadStatType.ArcherDefence:
                    return TotalDefence * archerTotalStatAdjustValue[2] / 100;
                case Enums.SquadStatType.WizardDefence:
                    return TotalDefence * wizardTotalStatAdjustValue[2] / 100;
                
                case Enums.SquadStatType.Penetration:
                    return TotalPenetration;
                case Enums.SquadStatType.Accuracy:
                    return TotalAccuracy;
                case Enums.SquadStatType.CriticalRate:
                    return TotalCriticalRate;
                case Enums.SquadStatType.CriticalDamage:
                    return TotalCriticalDamage;
                case Enums.SquadStatType.AcquisitionGold:
                    return TotalAcquisitionGold;
                case Enums.SquadStatType.AcquisitionExp:
                    return TotalAcquisitionExp;
            }

            return 0;
        }

        public float GetTotalSubSquadStat(Enums.SquadStatType statusType)
        {
            switch (statusType)
            {
                case Enums.SquadStatType.WarriorAttackRange:
                    return warriorAttackRange;
                case Enums.SquadStatType.ArcherAttackRange:
                    return archerAttackRange;
                case Enums.SquadStatType.WizardAttackRange:
                    return wizardAttackRange;
                case Enums.SquadStatType.MoveSpeed:
                    return moveSpeed;
                case Enums.SquadStatType.FollowRange:
                    return followRange;
            }

            return 0;
        }
        
        public BigInteger GetTotalCombatPower()
        {
            var returnValue = TotalAttack + TotalDefence + TotalMaxHealth + TotalAccuracy + TotalPenetration + TotalCriticalRate + TotalCriticalDamage;
            
            return returnValue;
        }

        public void SetTotalSquadStat(Enums.SquadStatType squadStatType, BigInteger statValue)
        {
            switch (squadStatType)
            {
                case Enums.SquadStatType.WarriorAtk:
                    TotalWarriorAttack = statValue;
                    break;
                case Enums.SquadStatType.ArcherAtk:
                    TotalArcherAttack = statValue;
                    break;
                case Enums.SquadStatType.WizardAtk:
                    TotalWizardAttack = statValue;
                    break;
                case Enums.SquadStatType.Attack:
                    TotalAttack = statValue;
                    break;
                case Enums.SquadStatType.Health:
                    TotalMaxHealth = statValue;
                    break;
                case Enums.SquadStatType.Defence:
                    TotalDefence = statValue;
                    break;
                case Enums.SquadStatType.Penetration:
                    TotalPenetration = statValue;
                    break;
                case Enums.SquadStatType.Accuracy:
                    TotalAccuracy = statValue;
                    break;
                case Enums.SquadStatType.CriticalRate:
                    TotalCriticalRate = statValue;
                    break;
                case Enums.SquadStatType.CriticalDamage:
                    TotalCriticalDamage = statValue;
                    break;
                case Enums.SquadStatType.AcquisitionGold:
                    TotalAcquisitionGold = statValue;
                    break;
                case Enums.SquadStatType.AcquisitionExp:
                    TotalAcquisitionExp = statValue;
                    break;
            }
        }

        public void Equip(Equipment equipment)
        {
            // TODO : 이전 장비 장착 해제
            equipment.isEquipped = true;
            equipment.SaveEquipmentDataIntoES3Loader();
            
            // TODO: 장착 시 스탯 증가 반영해야 함
            foreach (var equippedEffect in equipment.equippedEffects)
            {
                squadEntireStat.UpdateStat(equippedEffect.statType, equippedEffect.baseIncreaseValue, equippedEffect.increaseStatType == Enums.IncreaseStatValueType.BaseStat);
            }
            
            // foreach (var ownedEffect in equipment.ownedEffects)
            // {
            //     if (ownedEffect.increaseStatType == Enums.IncreaseStatValueType.BaseStat)
            //     {
            //         squadEntireStat.UpdateStat(ownedEffect.statType, ownedEffect.increaseValue, true);
            //     }
            //     else
            //     {
            //         squadEntireStat.UpdateStat(ownedEffect.statType, ownedEffect.increaseValue, false);
            //     }
            // }
            
            UIManager.Instance.inventoryPanelUI.equipmentButton[(int)equipment.equipmentType].GetComponent<InventoryPanelSelectedItemUI>().UpdateInventoryPanelSelectedItem(equipment.equipmentTier, SpriteManager.Instance.GetEquipmentSprite(equipment.equipmentType, equipment.equipmentIconIndex), SpriteManager.Instance.GetEquipmentBackground((int) equipment.equipmentRarity), SpriteManager.Instance.GetEquipmentBackgroundEffect((int) equipment.equipmentRarity));
            
            InventoryManager.Instance.SaveAllEquipmentInfo();
        }

        // private void UnEquip(Equipment equipment)
        // {
        //     equipment.isEquipped = false;
        //     InventoryPanelUI.UpdateEquipmentUIAction?.Invoke(equipment.isEquipped);
        //     squadEntireStat.UpdateTotalStat(Enum.SquadStatType.Attack, -1 * equipment.equippedEffect);
        //     equipment.SaveEquipmentAllInfo();
        //     Debug.Log("장비 장착 해제" + equipment.id);
        // }

        #region Battle

        public void DespawnSquad()
        {
            foreach (var squad in squads)
                if (squad.activeInHierarchy)
                    squad.SetActive(false);
        }

        public void SpawnSquad()
        {
            var index = 0;

            foreach (var squad in squads)
            {
                squad.SetActive(true);
                squad.transform.position = squadSpawnPosition[index];

                index++;
            }

            if (cameraController.currentCameraTarget == null)
                cameraController.currentCameraTarget = squads[0].transform;
        }

        #endregion

        #region SkillCoolTimer

        public void RunSkillCoolTimer(Enums.CharacterType type, int index)
        {
            StartCoroutine(CoolTimer(type, index));
        }

        private IEnumerator CoolTimer(Enums.CharacterType type, int index)
        {
            switch (type)
            {
                case Enums.CharacterType.Warrior:
                    warriorSkillCoolTimer[index].remainedSkillCoolTime = warriorSkillCoolTimer[index].maxSkillCoolTime;
                    warriorSkillCoolTimer[index].isSkillReady = false;
                    UIManager.Instance.squadSkillCoolTimerUI.warriorSkillCoolTimerUI[index]
                        .ActivateSkillCoolTimer(warriorSkillCoolTimer[index].isSkillReady);

                    while (true)
                    {
                        warriorSkillCoolTimer[index].remainedSkillCoolTime -= Time.deltaTime;

                        if (warriorSkillCoolTimer[index].remainedSkillCoolTime <= 0) break;
                        UIManager.Instance.squadSkillCoolTimerUI.warriorSkillCoolTimerUI[index]
                            .UpdateSkillCoolTimerText(warriorSkillCoolTimer[index].remainedSkillCoolTime,
                                warriorSkillCoolTimer[index].maxSkillCoolTime);

                        yield return null;
                    }

                    warriorSkillCoolTimer[index].remainedSkillCoolTime = 0.0f;
                    UIManager.Instance.squadSkillCoolTimerUI.warriorSkillCoolTimerUI[index].UpdateSkillCoolTimerText(
                        warriorSkillCoolTimer[index].remainedSkillCoolTime,
                        warriorSkillCoolTimer[index].maxSkillCoolTime);

                    warriorSkillCoolTimer[index].isSkillReady = true;
                    UIManager.Instance.squadSkillCoolTimerUI.warriorSkillCoolTimerUI[index]
                        .ActivateSkillCoolTimer(warriorSkillCoolTimer[index].isSkillReady);
                    yield break;

                case Enums.CharacterType.Archer:
                    archerSkillCoolTimer[index].remainedSkillCoolTime = archerSkillCoolTimer[index].maxSkillCoolTime;
                    archerSkillCoolTimer[index].isSkillReady = false;
                    UIManager.Instance.squadSkillCoolTimerUI.archerSkillCoolTimerUI[index]
                        .ActivateSkillCoolTimer(archerSkillCoolTimer[index].isSkillReady);

                    while (true)
                    {
                        archerSkillCoolTimer[index].remainedSkillCoolTime -= Time.deltaTime;

                        if (archerSkillCoolTimer[index].remainedSkillCoolTime <= 0) break;
                        UIManager.Instance.squadSkillCoolTimerUI.archerSkillCoolTimerUI[index]
                            .UpdateSkillCoolTimerText(archerSkillCoolTimer[index].remainedSkillCoolTime,
                                archerSkillCoolTimer[index].maxSkillCoolTime);

                        yield return null;
                    }

                    archerSkillCoolTimer[index].remainedSkillCoolTime = 0.0f;
                    UIManager.Instance.squadSkillCoolTimerUI.archerSkillCoolTimerUI[index].UpdateSkillCoolTimerText(
                        archerSkillCoolTimer[index].remainedSkillCoolTime,
                        archerSkillCoolTimer[index].maxSkillCoolTime);

                    archerSkillCoolTimer[index].isSkillReady = true;
                    UIManager.Instance.squadSkillCoolTimerUI.archerSkillCoolTimerUI[index]
                        .ActivateSkillCoolTimer(archerSkillCoolTimer[index].isSkillReady);
                    yield break;

                case Enums.CharacterType.Wizard:
                    wizardSkillCoolTimer[index].remainedSkillCoolTime = wizardSkillCoolTimer[index].maxSkillCoolTime;
                    wizardSkillCoolTimer[index].isSkillReady = false;
                    UIManager.Instance.squadSkillCoolTimerUI.wizardSkillCoolTimerUI[index]
                        .ActivateSkillCoolTimer(wizardSkillCoolTimer[index].isSkillReady);

                    while (true)
                    {
                        wizardSkillCoolTimer[index].remainedSkillCoolTime -= Time.deltaTime;

                        if (wizardSkillCoolTimer[index].remainedSkillCoolTime <= 0) break;
                        UIManager.Instance.squadSkillCoolTimerUI.wizardSkillCoolTimerUI[index]
                            .UpdateSkillCoolTimerText(wizardSkillCoolTimer[index].remainedSkillCoolTime,
                                wizardSkillCoolTimer[index].maxSkillCoolTime);

                        yield return null;
                    }

                    wizardSkillCoolTimer[index].remainedSkillCoolTime = 0.0f;
                    UIManager.Instance.squadSkillCoolTimerUI.wizardSkillCoolTimerUI[index].UpdateSkillCoolTimerText(
                        wizardSkillCoolTimer[index].remainedSkillCoolTime,
                        wizardSkillCoolTimer[index].maxSkillCoolTime);

                    wizardSkillCoolTimer[index].isSkillReady = true;
                    UIManager.Instance.squadSkillCoolTimerUI.wizardSkillCoolTimerUI[index]
                        .ActivateSkillCoolTimer(wizardSkillCoolTimer[index].isSkillReady);
                    yield break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion
    }
}