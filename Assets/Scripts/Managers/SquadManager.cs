using System;
using System.Collections;
using Controller.UI;
using Controller.UI.BottomMenuUI;
using Creature.CreatureClass.SquadClass;
using Creature.Data;
using Function;
using UnityEngine;
using UnityEngine.Serialization;
using Enum = Data.Enum;

namespace Managers
{
    [Serializable]
    public struct SkillCoolTimer
    {
        public GameObject skill;
        public bool isSkillReady;
        public float remainedSkillCoolTime;
        public float maxSkillCoolTime;
    }

    public class SquadManager : MonoBehaviour
    {
        public static SquadManager Instance;
        public static Action<Equipment> EquipAction;
        public static Action<Equipment> UnEquipAction;

        [Header("=== Camera Settings=== ")]
        [SerializeField] private CameraController cameraController;

        [Space(5)]
        [Header("=== Squad Position Info ===")]
        public GameObject[] squads;

        [SerializeField] private Vector3[] squadSpawnPosition;

        [Space(5)]
        [Header("=== Squad Battle Info ===")]
        [Header("공격 범위")] [SerializeField]
        private float warriorAttackRange;

        [SerializeField] private float archerAttackRange;
        [SerializeField] private float wizardAttackRange;
        [Header("적 탐지 범위")] [SerializeField] private float followRange;
        [Header("이동 속도")] [SerializeField] private float moveSpeed;

        [Space(3)]
        [Header("=== 스킬 쿨타임 ===")] //TODO: 스킬 쿨 다운을 ScriptableObject에서 긁어와야 함
        [Header("워리어")]
        public SkillCoolTimer[] warriorSkillCoolTimer;

        [Header("아처")]
        public SkillCoolTimer[] archerSkillCoolTimer;

        [Header("위자드")]
        public SkillCoolTimer[] wizardSkillCoolTimer;

        [Space(5)]
        [Header("=== Squad Stats Info ===")]
        [Header("Base SquadStats")] [SerializeField]
        public SquadEntireStat squadEntireStat;

        [Header("Total SquadStats")]
        [SerializeField] private BigInteger totalWarriorAttack;
        [SerializeField] private BigInteger totalArcherAttack;
        [SerializeField] private BigInteger totalWizardAttack;
        [SerializeField] private BigInteger totalMaxHealth;
        [SerializeField] private BigInteger totalDefence;
        [SerializeField] private int totalPenetration;
        [SerializeField] private int totalAccuracy;
        [SerializeField] private int totalCriticalRate;
        [SerializeField] private int totalCriticalDamage;
        
        public SquadLevel SquadLevel;
        public SummonLevel SummonLevel;

        public Equipment EquippedSword { get; }
        public Equipment EquippedBow { get; }
        public Equipment EquippedStaff { get; }
        public Equipment EquippedHelmet { get; }
        public Equipment EquippedArmor { get; }
        public Equipment EquippedGauntlet { get; }

        private void Awake()
        {
            Instance = this;

            SummonLevel = new SummonLevel();
            SquadLevel = new SquadLevel();

            cameraController.InitCameraTarget(squads[0].transform);
        }

        public void InitSquadManager()
        {
            SetEventListeners();
            SetSquadStatsFromBaseStats();
            SetSkillCoolTimer();
            // SetupPlayerLevel();
            SetupSummonLevel();
        }

        private void SetSkillCoolTimer()
        {
            for (var i = 0; i < warriorSkillCoolTimer.Length; i++)
            {
                warriorSkillCoolTimer[i].isSkillReady = true;
                warriorSkillCoolTimer[i].remainedSkillCoolTime = warriorSkillCoolTimer[i].maxSkillCoolTime;
            }
        }

        private void SetupSummonLevel()
        {
            SummonLevel.InitializeData();
            SummonLevel.SetupEventListeners();
        }


        private void SetSquadStatsFromBaseStats()
        {
            totalWarriorAttack = squadEntireStat.BaseAttack;
            totalArcherAttack = squadEntireStat.BaseAttack;
            totalWizardAttack = squadEntireStat.BaseAttack;

            totalMaxHealth = squadEntireStat.BaseHealth;
            totalDefence = squadEntireStat.BaseDefense;
            totalPenetration = squadEntireStat.BasePenetration;
            totalAccuracy = squadEntireStat.BaseAccuracy;
            totalCriticalRate = squadEntireStat.BaseCriticalRate;
            totalCriticalDamage = squadEntireStat.BaseCriticalDamage;
        }

        // 이벤트 설정하는 메서드
        private void SetEventListeners()
        {
            SquadStatManager.Instance.UpgradeTotalSquadBaseStatAction += squadEntireStat.IncreaseBaseStatBySquadStatPanel;
            SquadStatManager.Instance.UpgradeTotalSquadPercentStatAction += squadEntireStat.IncreasePercentStatBySquadStatPanel;
            
            EquipAction += Equip;
            UnEquipAction += UnEquip;
        }

        public BigInteger GetTotalSquadStat(Enum.SquadStatType statusType)
        {
            switch (statusType)
            {
                case Enum.SquadStatType.WarriorAtk:
                    return totalWarriorAttack;
                case Enum.SquadStatType.ArcherAtk:
                    return totalArcherAttack;
                case Enum.SquadStatType.WizardAtk:
                    return totalWizardAttack;
                case Enum.SquadStatType.Health:
                    return totalMaxHealth;
                case Enum.SquadStatType.Defence:
                    return totalDefence;
                case Enum.SquadStatType.Penetration:
                    return totalPenetration;
                case Enum.SquadStatType.Accuracy:
                    return totalAccuracy;
                case Enum.SquadStatType.CriticalRate:
                    return totalCriticalRate;
                case Enum.SquadStatType.CriticalDamage:
                    return totalCriticalDamage;
                default:
                    Debug.Log("에러에러!!");
                    break;
            }

            return 0;
        }

        public float GetTotalSubSquadStat(Enum.SquadStatType statusType)
        {
            switch (statusType)
            {
                case Enum.SquadStatType.WarriorAttackRange:
                    return warriorAttackRange;
                case Enum.SquadStatType.ArcherAttackRange:
                    return archerAttackRange;
                case Enum.SquadStatType.WizardAttackRange:
                    return wizardAttackRange;
                case Enum.SquadStatType.MoveSpeed:
                    return moveSpeed;
                case Enum.SquadStatType.FollowRange:
                    return followRange;
                default:
                    Debug.Log("에러에러!!");
                    break;
            }

            return 0;
        }

        public void SetTotalSquadStat(Enum.SquadStatType squadStatType, BigInteger statValue)
        {
            switch (squadStatType)
            {
                //TODO : 워리어, 아처, 마법사 공격력을 따로 계산하는 산식 작성 요망
                case Enum.SquadStatType.Attack:
                    totalWarriorAttack = statValue;
                    totalArcherAttack = statValue;
                    totalWizardAttack = statValue;

                    foreach (var squad in squads) squad.GetComponent<Squad>().attack = statValue;

                    break;

                case Enum.SquadStatType.Health:
                    totalMaxHealth = statValue;
                    break;

                case Enum.SquadStatType.Defence:
                    totalDefence = statValue;
                    break;
            }
        }

        public void SetTotalSquadStat(Enum.SquadStatType squadStatType, int statValue)
        {
            switch (squadStatType)
            {
                case Enum.SquadStatType.Penetration:
                    totalPenetration = statValue;
                    break;
                case Enum.SquadStatType.Accuracy:
                    totalAccuracy = statValue;
                    break;
                case Enum.SquadStatType.CriticalRate:
                    totalCriticalRate = statValue;
                    break;
                case Enum.SquadStatType.CriticalDamage:
                    totalCriticalDamage = statValue;
                    break;
            }
        }

        private void Equip(Equipment equipment)
        {
            UnEquip(equipment);

            var equippedEquipment = equipment.type switch
            {
                Enum.EquipmentType.Sword => EquippedSword,
                Enum.EquipmentType.Bow => EquippedBow,
                Enum.EquipmentType.Staff => EquippedStaff,
                Enum.EquipmentType.Helmet => EquippedHelmet,
                Enum.EquipmentType.Armor => EquippedArmor,
                Enum.EquipmentType.Gauntlet => EquippedGauntlet,
                _ => null
            };

            equippedEquipment = equipment.GetComponent<Equipment>();
            equippedEquipment.isEquipped = true;

            squadEntireStat.IncreasePercentStat(Enum.SquadStatType.Attack, equippedEquipment.equippedEffect);

            InventoryUI.UpdateEquipmentUIAction?.Invoke(equippedEquipment.isEquipped);
            equippedEquipment.SaveEquipmentAllInfo();
            Debug.Log("장비 장착" + equippedEquipment.id);
        }

        private void UnEquip(Equipment equipment)
        {
            equipment.isEquipped = false;
            InventoryUI.UpdateEquipmentUIAction?.Invoke(equipment.isEquipped);
            squadEntireStat.DecreasePercentStat(Enum.SquadStatType.Attack, equipment.equippedEffect);
            equipment.SaveEquipmentAllInfo();
            Debug.Log("장비 장착 해제" + equipment.id);
            equipment = null;
        }

        #region Battle

        public void SpawnSquad()
        {
            var index = 0;

            foreach (var squad in squads)
            {
                squad.SetActive(true);
                squad.GetComponent<Squad>().InitCreature();
                squad.transform.position = squadSpawnPosition[index];

                index++;
            }

            if (cameraController.currentCameraTarget == null)
                cameraController.currentCameraTarget = squads[0].transform;
        }

        #endregion

        #region SkillCoolTimer

        public void RunSkillCoolTimer(Enum.SquadClassType type, int index)
        {
            StartCoroutine(CoolTimer(type, index));
        }

        private IEnumerator CoolTimer(Enum.SquadClassType type, int index)
        {
            switch (type)
            {
                case Enum.SquadClassType.Warrior:
                    warriorSkillCoolTimer[index].remainedSkillCoolTime = warriorSkillCoolTimer[index].maxSkillCoolTime; 
                    warriorSkillCoolTimer[index].isSkillReady = false;
                    SkillTimerUI.Instance.ActivateSkillTimer(type, index, warriorSkillCoolTimer[index].isSkillReady);
                    
                    while (true)
                    {
                        warriorSkillCoolTimer[index].remainedSkillCoolTime -= Time.deltaTime;

                        if (warriorSkillCoolTimer[index].remainedSkillCoolTime <= 0) break;
                        SkillTimerUI.Instance.SetSkillTimerText(type, index, warriorSkillCoolTimer[index].remainedSkillCoolTime, warriorSkillCoolTimer[index].maxSkillCoolTime);
                        
                        yield return null;
                    }

                    warriorSkillCoolTimer[index].remainedSkillCoolTime = 0.0f;
                    SkillTimerUI.Instance.SetSkillTimerText(type, index, warriorSkillCoolTimer[index].remainedSkillCoolTime, warriorSkillCoolTimer[index].maxSkillCoolTime);
                    
                    warriorSkillCoolTimer[index].isSkillReady = true;
                    SkillTimerUI.Instance.ActivateSkillTimer(type, index, warriorSkillCoolTimer[index].isSkillReady);
                    yield break;
                
                case Enum.SquadClassType.Archer:
                    archerSkillCoolTimer[index].remainedSkillCoolTime = archerSkillCoolTimer[index].maxSkillCoolTime; 
                    archerSkillCoolTimer[index].isSkillReady = false;
                    SkillTimerUI.Instance.ActivateSkillTimer(type, index, archerSkillCoolTimer[index].isSkillReady);
                    
                    while (true)
                    {
                        archerSkillCoolTimer[index].remainedSkillCoolTime -= Time.deltaTime;

                        if (archerSkillCoolTimer[index].remainedSkillCoolTime <= 0) break;
                        SkillTimerUI.Instance.SetSkillTimerText(type, index, archerSkillCoolTimer[index].remainedSkillCoolTime, archerSkillCoolTimer[index].maxSkillCoolTime);
                        
                        yield return null;
                    }

                    archerSkillCoolTimer[index].remainedSkillCoolTime = 0.0f;
                    SkillTimerUI.Instance.SetSkillTimerText(type, index, archerSkillCoolTimer[index].remainedSkillCoolTime, archerSkillCoolTimer[index].maxSkillCoolTime);
                    
                    archerSkillCoolTimer[index].isSkillReady = true;
                    SkillTimerUI.Instance.ActivateSkillTimer(type, index, archerSkillCoolTimer[index].isSkillReady);
                    yield break;
                case Enum.SquadClassType.Wizard:
                    wizardSkillCoolTimer[index].remainedSkillCoolTime = wizardSkillCoolTimer[index].maxSkillCoolTime; 
                    wizardSkillCoolTimer[index].isSkillReady = false;
                    SkillTimerUI.Instance.ActivateSkillTimer(type, index, wizardSkillCoolTimer[index].isSkillReady);
                    
                    while (true)
                    {
                        wizardSkillCoolTimer[index].remainedSkillCoolTime -= Time.deltaTime;

                        if (wizardSkillCoolTimer[index].remainedSkillCoolTime <= 0) break;
                        SkillTimerUI.Instance.SetSkillTimerText(type, index, wizardSkillCoolTimer[index].remainedSkillCoolTime, wizardSkillCoolTimer[index].maxSkillCoolTime);
                        
                        yield return null;
                    }

                    wizardSkillCoolTimer[index].remainedSkillCoolTime = 0.0f;
                    SkillTimerUI.Instance.SetSkillTimerText(type, index, wizardSkillCoolTimer[index].remainedSkillCoolTime, wizardSkillCoolTimer[index].maxSkillCoolTime);
                    
                    wizardSkillCoolTimer[index].isSkillReady = true;
                    SkillTimerUI.Instance.ActivateSkillTimer(type, index, wizardSkillCoolTimer[index].isSkillReady);
                    yield break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion
    }
}