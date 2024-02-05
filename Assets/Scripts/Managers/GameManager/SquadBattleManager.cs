using System;
using System.Collections;
using System.Collections.Generic;
using Controller.UI;
using Controller.UI.BottomMenuUI;
using Controller.UI.BottomMenuUI.BottomMenuPanel.InventoryPanel;
using Creature.CreatureClass.SquadClass;
using Creature.Data;
using Data;
using Function;
using Managers.BottomMenuManager.SquadPanel;
using Managers.BottomMenuManager.TalentPanel;
using UnityEngine;

namespace Managers.BattleManager
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
        public static Action<Equipment> EquipAction;

        [Header("=== Camera Settings=== ")] [SerializeField]
        public CameraController cameraController;

        [Space(5)] [Header("=== Squad Position Info ===")]
        public GameObject[] squads;

        [SerializeField] private Vector3[] squadSpawnPosition;

        [Space(5)] [Header("=== Squad Battle Info ===")] [Header("공격 범위")] [SerializeField]
        private float warriorAttackRange;

        [SerializeField] private float archerAttackRange;
        [SerializeField] private float wizardAttackRange;

        [Header("적 탐지 범위")] [SerializeField] private float followRange;

        [Header("이동 속도")] [SerializeField] private float moveSpeed;

        [Space(3)]
        [Header("=== 스킬 쿨타임 ===")] //TODO: 스킬 쿨 다운을 ScriptableObject에서 긁어와야 함
        [Header("Auto")]
        public bool autoSkill;

        [Header("워리어")] public SkillCoolTimer[] warriorSkillCoolTimer;

        [Header("아처")] public SkillCoolTimer[] archerSkillCoolTimer;

        [Header("위자드")] public SkillCoolTimer[] wizardSkillCoolTimer;

        [Space(5)] [Header("=== Squad Stats Info ===")] [Header("스킬 데미지")] [SerializeField]
        public List<int> warriorSkillDamagePercent;

        [SerializeField] public List<int> archerSkillDamagePercent;
        [SerializeField] public List<int> wizardSkillDamagePercent;

        [Header("Base SquadStats")] [SerializeField]
        public SquadEntireStat squadEntireStat;

        [Header("Total SquadStats")] public BigInteger totalWarriorAttack;

        public BigInteger totalArcherAttack;
        public BigInteger totalWizardAttack;
        public BigInteger totalAttack;
        public BigInteger totalMaxHealth;
        public BigInteger totalDefence;
        public BigInteger totalPenetration;
        public BigInteger totalAccuracy;
        public BigInteger totalCriticalRate;
        public BigInteger totalCriticalDamage;
        public BigInteger totalAcquisitionGold;
        public BigInteger totalAcquisitionExp;

        public SquadLevel SquadLevel;

        private void Awake()
        {
            Instance = this;
            
            SquadLevel = new SquadLevel();

            cameraController.InitCameraTarget(squads[0].transform);

            autoSkill = true;
        }

        public void InitSquadManager()
        {
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
            totalWarriorAttack = squadEntireStat.baseAttack;
            totalArcherAttack = squadEntireStat.baseAttack;
            totalWizardAttack = squadEntireStat.baseAttack;
            totalAttack = squadEntireStat.baseAttack;

            totalMaxHealth = squadEntireStat.baseHealth;
            totalDefence = squadEntireStat.baseDefense;
            totalPenetration = squadEntireStat.basePenetration;
            totalAccuracy = squadEntireStat.baseAccuracy;
            totalCriticalRate = squadEntireStat.baseCriticalRate;
            totalCriticalDamage = squadEntireStat.baseCriticalDamage;
            totalAcquisitionGold = squadEntireStat.baseAcquisitionGold;
            totalAcquisitionExp = squadEntireStat.baseAcquisitionExp;
        }

        // 이벤트 설정하는 메서드
        private void InitializeEventListeners()
        {
            SquadStatManager.Instance.OnUpgradeTotalSquadStatFromSquadStatPanel += squadEntireStat
                .UpdateStat;
            TalentManager.Instance.OnUpgradeTotalSquadStatFromSquadTalentPanel +=
                squadEntireStat.UpdateStat; //TODO : 재능 스탯 증가

            EquipAction += Equip;
        }

        public BigInteger GetTotalSquadStat(Enums.SquadStatType statusType)
        {
            switch (statusType)
            {
                case Enums.SquadStatType.WarriorAtk:
                    return totalWarriorAttack;
                case Enums.SquadStatType.ArcherAtk:
                    return totalArcherAttack;
                case Enums.SquadStatType.WizardAtk:
                    return totalWizardAttack;
                case Enums.SquadStatType.Health:
                    return totalMaxHealth;
                case Enums.SquadStatType.Defence:
                    return totalDefence;
                case Enums.SquadStatType.Penetration:
                    return totalPenetration;
                case Enums.SquadStatType.Accuracy:
                    return totalAccuracy;
                case Enums.SquadStatType.CriticalRate:
                    return totalCriticalRate;
                case Enums.SquadStatType.CriticalDamage:
                    return totalCriticalDamage;
                case Enums.SquadStatType.AcquisitionGold:
                    return totalAcquisitionGold;
                case Enums.SquadStatType.AcquisitionExp:
                    return totalAcquisitionExp;
                default:
                    Debug.Log("에러에러!!");
                    break;
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
                default:
                    Debug.Log("에러에러!!");
                    break;
            }

            return 0;
        }

        public void SetTotalSquadStat(Enums.SquadStatType squadStatType, BigInteger statValue)
        {
            switch (squadStatType)
            {
                //TODO : 워리어, 아처, 마법사 공격력을 따로 계산하는 산식 작성 요망
                case Enums.SquadStatType.Attack:
                    totalAttack = statValue;
                    totalWarriorAttack = statValue;
                    totalArcherAttack = statValue;
                    totalWizardAttack = statValue;

                    foreach (var squad in squads) squad.GetComponent<Squad>().damage = statValue;

                    break;
                case Enums.SquadStatType.Health:
                    totalMaxHealth = statValue;
                    break;
                case Enums.SquadStatType.Defence:
                    totalDefence = statValue;
                    break;
                case Enums.SquadStatType.Penetration:
                    totalPenetration = statValue;
                    break;
                case Enums.SquadStatType.Accuracy:
                    totalAccuracy = statValue;
                    break;
                case Enums.SquadStatType.CriticalRate:
                    totalCriticalRate = statValue;
                    break;
                case Enums.SquadStatType.CriticalDamage:
                    totalCriticalDamage = statValue;
                    break;
                case Enums.SquadStatType.AcquisitionGold:
                    totalAcquisitionGold = statValue;
                    break;
                case Enums.SquadStatType.AcquisitionExp:
                    totalAcquisitionExp = statValue;
                    break;
            }
        }

        private void Equip(Equipment equipment)
        {
            // TODO : 이전 장비 장착 해제
            equipment.isEquipped = true;
            equipment.SaveEquipmentAllInfo();
            
            // TODO: 장착 시 스탯 증가 반영해야 함
            // squadEntireStat.UpdateTotalStat(Enum.SquadStatType.Attack, equippedEquipment.equippedEffect);
            
            UIManager.Instance.inventoryPanelUI.equipmentButton[(int)equipment.equipmentType].GetComponent<InventoryPanelSelectedItemUI>().UpdateInventoryPanelSelectedItem(equipment.equipmentTier, SpriteManager.Instance.GetEquipmentSprite(equipment.equipmentType, equipment.equipmentIconIndex), SpriteManager.Instance.GetEquipmentBackground((int) equipment.equipmentRarity), SpriteManager.Instance.GetEquipmentBackgroundEffect((int) equipment.equipmentRarity));
            
            // Debug.Log("장비 장착" + equippedEquipment.id);
        }

        private void UnEquip(Equipment equipment)
        {
            equipment.isEquipped = false;
            InventoryPanelUI.UpdateEquipmentUIAction?.Invoke(equipment.isEquipped);
            // squadEntireStat.UpdateTotalStat(Enum.SquadStatType.Attack, -1 * equipment.equippedEffect);
            // equipment.SaveEquipmentAllInfo();
            // Debug.Log("장비 장착 해제" + equipment.id);
        }

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