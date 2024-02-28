using System;
using System.Collections.Generic;
using Creature.Data;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.BottomMenuManager.InventoryPanel;
using Managers.BottomMenuManager.SquadPanel;
using Managers.GameManager;
using Module;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.InventoryPanel
{
    public class InventoryPanelUI : MonoBehaviour
    {
        public static Action<bool> UpdateEquipmentUIAction;
        public static event Action<Equipment> OnClickSelectEquipment;

        [Header("장착 중인 장비들을 보여주는 패널")] public GameObject squadEquipmentStatusPanel;
        [Header("선택한 장비 정보를 띄워주는 패널")] public GameObject selectedEquipmentPanel;
        [Header("장착한 장비 버튼")] public GameObject[] equipmentButton;
        [Header("장비 스크롤뷰 패널")] public GameObject[] scrollViewEquipmentPanel;
        [Header("선택한 장비 종류")] public TMP_Text currentSelectedEquipmentType;

        [Header("스쿼드 구성 패널 아이템 UI")]
        public List<GameObject> inventoryScrollViewItemSwords = new();
        public List<GameObject> inventoryScrollViewItemBows = new();
        public List<GameObject> inventoryScrollViewItemStaffs = new();
        public List<GameObject> inventoryScrollViewItemHelmets = new();
        public List<GameObject> inventoryScrollViewItemArmors = new();
        public List<GameObject> inventoryScrollViewItemGauntlets = new();

        [Header("캐릭터 스폰 위치")] public List<GameObject> spawnTargetPosition;
        
        [Header("장비 스크롤뷰 / 스킨 전환 버튼")] public List<Button> equipmentViewButtons;

        [Space(5)]
        [Header("=== 장비 선택 팝업 창 ===")]
        [Header("선택 장비 정보 패널 나가기 버튼")] public Button selectedEquipmentPanelExitButton;
        [Header("전체 합성 버튼")] public Button allCompositeButton;
        [Header("전체 합성 잠금 버튼")] public Button allCompositeLockButton;
        [Header("자동 장착 버튼")] public Button autoEquipButton;
        [Header("자동 장착 잠금 버튼")] public Button autoEquipLockButton;
        [Header("레벨 업 버튼")] public Button levelUpButton;
        [Header("레벨 업 잠금 버튼")] public Button levelUpLockButton;
        [Header("요구 재화")] public TMP_Text requiredCurrencyText;
        [Header("선택 장비")] public Equipment selectEquipment;
        [Header("선택 장비 인덱스")] public int currentSelectedInventoryItemIndex;

        [Header("선택 장비 정보")]
        public TMP_Text selectEquipmentTier;
        public Image selectEquipmentIcon;
        public Image selectEquipmentBackground;
        public Image selectEquipmentBackgroundEffect;
        public TMP_Text selectEquipmentName;
        public TMP_Text selectEquipmentGrade;
        public List<TMP_Text> selectEquipmentEquippedEffect;
        public List<TMP_Text> selectEquipmentOwnedEffect;
        public TMP_Text selectEquipmentLevel;

        // 이벤트 설정하는 메서드
        private void OnEnable()
        {
            OnClickSelectEquipment += SelectEquipment;
        }

        private void OnDisable()
        {
            OnClickSelectEquipment -= SelectEquipment;
            
            selectedEquipmentPanel.gameObject.SetActive(false);
            squadEquipmentStatusPanel.gameObject.SetActive(true);
            selectedEquipmentPanelExitButton.gameObject.SetActive(false);
        }

        // 버튼 클릭 리스너 설정하는 메서드
        public void InitializeEventListener() 
        {
            for (var i = 0; i < equipmentButton.Length; i++)
            {
                var index = i;
                equipmentButton[i].GetComponent<Button>().onClick.AddListener(() => OnClickEquipment(index));
            }

            for (var i = 0; i < 3; i++)
            {
                var index = i;
                equipmentButton[i].GetComponent<Button>().onClick.AddListener(()=> OnClickWeapon(index));
            }

            allCompositeButton.onClick.AddListener(OnClickAllComposite);
            autoEquipButton.onClick.AddListener(OnClickAutoEquip);
            selectedEquipmentPanelExitButton.onClick.AddListener(OnClockSelectedEquipmentPanelExit);
            // levelUpButton.onClick.AddListener(OnClickLevelUp);
            
            allCompositeLockButton.GetComponent<LockButtonUI>().InitializeEventListener();
            autoEquipLockButton.GetComponent<LockButtonUI>().InitializeEventListener();
            
            levelUpButton.onClick.AddListener(OnClickEquipmentLevelUp);
            levelUpButton.GetComponent<HoldButton>().onHold.AddListener(OnClickEquipmentLevelUp);
            levelUpLockButton.GetComponent<LockButtonUI>().InitializeEventListener();
            
            equipmentViewButtons[1].GetComponent<LockButtonUI>().InitializeEventListener();
        }

        private void OnClockSelectedEquipmentPanelExit()
        {
            selectedEquipmentPanelExitButton.gameObject.SetActive(false);
            selectedEquipmentPanel.SetActive(false);
            squadEquipmentStatusPanel.SetActive(true);
        }
        
        private void OnClickWeapon(int index)
        {
            for (var i = 0; i < spawnTargetPosition.Count; i++)
            {
                spawnTargetPosition[i].SetActive(i == index);
            }
        }

        private void OnClickEquipment(int index)
        {
            for (var i = 0; i < scrollViewEquipmentPanel.Length; i++)
            {
                scrollViewEquipmentPanel[i].SetActive(i == index);
            }
            
            UpdateEquipmentTypeUI(index);
            UpdateAutoEquipButton(InventoryManager.Instance.canAutoEquip[index]);
            UpdateAllCompositeButton(InventoryManager.Instance.canAllComposite[index]);
            
            var equippedEquipment = index switch
            {
                0 => InventoryManager.Instance.equippedSword,
                1 => InventoryManager.Instance.equippedBow,
                2 => InventoryManager.Instance.equippedStaff,
                3 => InventoryManager.Instance.equippedHelmet,
                4 => InventoryManager.Instance.equippedArmor,
                5 => InventoryManager.Instance.equippedGauntlet,
                _ => null
            };

            SelectEquipment(equippedEquipment);
        }

        public void UpdateEquipmentTypeUI(int index)
        {
            switch (index)
            {
                case 0:
                    currentSelectedEquipmentType.text = $"<sprite={(int)Enums.IconType.EquipmentType_Sword} color=#000000> 근접 무기";
                    break;
                case 1:
                    currentSelectedEquipmentType.text = $"<sprite={(int)Enums.IconType.EquipmentType_Bow} color=#000000> 원거리 무기";
                    break;
                case 2:
                    currentSelectedEquipmentType.text = $"<sprite={(int)Enums.IconType.EquipmentType_Staff} color=#000000> 마법 무기";
                    break;
                case 3:
                    currentSelectedEquipmentType.text = $"<sprite={(int)Enums.IconType.EquipmentType_Helmet} color=#000000> 투구";
                    break;
                case 4:
                    currentSelectedEquipmentType.text = $"<sprite={(int)Enums.IconType.EquipmentType_Armor} color=#000000> 갑옷";
                    break;
                case 5:
                    currentSelectedEquipmentType.text = $"<sprite={(int)Enums.IconType.EquipmentType_Gauntlet} color=#000000> 장갑";
                    break;
            }
        }

        public void UpdateLevelUpButtonUI()
        {
            var canLevelUp = selectEquipment is { isPossessed: true, equipmentLevel: < InventoryManager.EquipmentMaxLevel } && selectEquipment.RequiredCurrencyForLevelUp() < new BigInteger(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.EquipmentEnhanceStone));
            
            levelUpButton.gameObject.SetActive(canLevelUp);
            levelUpLockButton.gameObject.SetActive(!canLevelUp);
        }

        // 장비 선택 이벤트 트리거 하는 메서드 
        public static void SelectEquipmentAction(Equipment equipment)
        {
            OnClickSelectEquipment?.Invoke(equipment);
        }

        // 장비 클릭 했을 때 불리는 메서드
        public void SelectEquipment(Equipment equipment)
        {
            if (selectEquipment.equipmentId == equipment.equipmentId && selectedEquipmentPanel.activeInHierarchy)
            {
                squadEquipmentStatusPanel.SetActive(true);
                selectedEquipmentPanel.SetActive(false);
                if (selectedEquipmentPanelExitButton.gameObject.activeInHierarchy) selectedEquipmentPanelExitButton.gameObject.SetActive(false);
            }
            else
            {
                if (squadEquipmentStatusPanel.activeInHierarchy) squadEquipmentStatusPanel.SetActive(false);
                if (!selectedEquipmentPanel.activeInHierarchy) selectedEquipmentPanel.SetActive(true);
                if (selectedEquipmentPanelExitButton.gameObject.activeInHierarchy == false) selectedEquipmentPanelExitButton.gameObject.SetActive(true);
                
                selectEquipment = equipment;
                
                UpdateSelectedEquipmentUI(selectEquipment);
                UpdateLevelUpButtonUI();
                UpdateAutoEquipButton(InventoryManager.Instance.canAutoEquip[(int) equipment.equipmentType]);
                UpdateAllCompositeButton(InventoryManager.Instance.canAllComposite[(int) equipment.equipmentType]);
            }
        }

        // 선택 장비 데이터 UI로 보여주는 메서드
        public void UpdateSelectedEquipmentUI(Equipment equipment)
        {
            selectEquipmentTier.text = $"{equipment.equipmentTier} 티어";
            selectEquipmentIcon.sprite = SpriteManager.Instance.GetEquipmentSprite(equipment.equipmentType, equipment.equipmentIconIndex);
            selectEquipmentBackground.sprite = SpriteManager.Instance.GetEquipmentBackground((int)equipment.equipmentRarity);
            selectEquipmentBackgroundEffect.sprite = SpriteManager.Instance.GetEquipmentBackgroundEffect((int)equipment.equipmentRarity);
            selectEquipmentName.text = equipment.equipmentName;
            selectEquipmentLevel.text = $"{equipment.equipmentLevel} / {InventoryManager.EquipmentMaxLevel}";
            selectEquipmentGrade.text = equipment.equipmentRarity switch
            {
                Enums.EquipmentRarity.Common => "커먼",
                Enums.EquipmentRarity.Uncommon => "언커먼",
                Enums.EquipmentRarity.Magic => "매직",
                Enums.EquipmentRarity.Rare => "레어",
                Enums.EquipmentRarity.Unique => "유니크",
                // Enums.EquipmentRarity.Legend => "레전드",
                _ => throw new ArgumentOutOfRangeException()
            };

            foreach (var t in selectEquipmentEquippedEffect)
            {
                t.gameObject.SetActive(false);
            }
            
            foreach (var t in selectEquipmentOwnedEffect)
            {
                t.gameObject.SetActive(false);
            }

            for (var i = 0; i < equipment.equippedEffects.Count; i++)
            {
                selectEquipmentEquippedEffect[i].text = equipment.equippedEffects[i].statType switch
                {
                    Enums.SquadStatType.WarriorAtk => $"(근접) 공격력 {UIManager.FormatCurrency(equipment.equippedEffects[i].CurrentIncreaseValue)}% 증가",
                    Enums.SquadStatType.ArcherAtk => $"(원거리) 공격력 {UIManager.FormatCurrency(equipment.equippedEffects[i].CurrentIncreaseValue)}% 증가",
                    Enums.SquadStatType.WizardAtk => $"(마법) 공격력 {UIManager.FormatCurrency(equipment.equippedEffects[i].CurrentIncreaseValue)}% 증가",
                    Enums.SquadStatType.Attack => $"공격력 {UIManager.FormatCurrency(equipment.equippedEffects[i].CurrentIncreaseValue)}% 증가",
                    Enums.SquadStatType.Health => $"체력 {UIManager.FormatCurrency(equipment.equippedEffects[i].CurrentIncreaseValue)}% 증가",
                    Enums.SquadStatType.Defence => $"방어력 {UIManager.FormatCurrency(equipment.equippedEffects[i].CurrentIncreaseValue)}% 증가",
                    _ => throw new ArgumentOutOfRangeException()
                };
                selectEquipmentEquippedEffect[i].gameObject.SetActive(true);
            }

            UpdateOwnedEffectsText();
            UpdateRequiredCurrencyUI(selectEquipment.RequiredCurrencyForLevelUp().ChangeMoney());
        }

        public GameObject FindInventoryItemList(string id)
        {
            var splitString = id.Split('_');
            var targetRarityIndex = (int)Enum.Parse(typeof(Enums.EquipmentRarity), splitString[0]) * 5;
            var targetType = (Enums.EquipmentType)Enum.Parse(typeof(Enums.EquipmentType), splitString[2]);
            var targetTierIndex = 5 - Convert.ToInt32(splitString[1]);
            var targetIndex = targetRarityIndex + targetTierIndex;

            switch (targetType)
            {
                case Enums.EquipmentType.Sword:
                    return UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemSwords[targetIndex];
                case Enums.EquipmentType.Bow:
                    return UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemBows[targetIndex];
                case Enums.EquipmentType.Staff:
                    return UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemStaffs[targetIndex];
                case Enums.EquipmentType.Helmet:
                    return UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemHelmets[targetIndex];
                case Enums.EquipmentType.Armor:
                    return UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemArmors[targetIndex];
                case Enums.EquipmentType.Gauntlet:
                    return UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemGauntlets[targetIndex];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnClickEquipmentLevelUp()
        {
            if (selectEquipment is not { isPossessed: true } || selectEquipment.equipmentLevel >= InventoryManager.EquipmentMaxLevel) return;
            
            if (selectEquipment.RequiredCurrencyForLevelUp() >
                new BigInteger(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.EquipmentEnhanceStone))) return;

            AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.EquipmentEnhanceStone,
                selectEquipment.RequiredCurrencyForLevelUp());
            
            selectEquipment.EquipmentLevelUp();
            selectEquipment.SaveEquipmentDataIntoES3Loader();
            
            UpdateSelectedEquipmentUI(selectEquipment);
            UpdateLevelUpButtonUI();
            FindInventoryItemList(selectEquipment.equipmentId).GetComponent<InventoryPanelItemUI>().UpdateInventoryPanelItemLevelUI(selectEquipment.equipmentLevel, InventoryManager.EquipmentMaxLevel);

            QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.LevelUpEquipment, 1);
            
            InventoryManager.Instance.SaveAllEquipmentInfo();
        }

        // 강화 판넬 버튼 눌렸을 때 불리는 메서드
        public void OnClickEnhancePanel()
        {
            // var enhanceEquipmentTemp = InventoryManager.GetEquipment(selectEquipment.equipmentId);
            // selectEquipment.GetComponent<Equipment>().SetEquipmentInfo(enhanceEquipmentTemp.GetComponent<Equipment>());
            //
            // if (enhanceEquipmentTemp == null) return;
            //
            // selectEquipmentLevel.text =
            //     $"Lv.{enhanceEquipmentTemp.level} / {enhanceEquipmentTemp.maxLevel}"; //장비 강화(0 / 0)
            // selectEquipmentEquippedEffect.text =
            //     $"장착 효과 : {enhanceEquipmentTemp.equippedEffect + enhanceEquipmentTemp.basicEquippedEffect}"; // 장착 효과 0 → 0
            // selectEquipmentOwnedEffect.text =
            //     $"보유 효과  : {enhanceEquipmentTemp.ownedEffect + enhanceEquipmentTemp.basicOwnedEffect}";
            // // enhanceCurrencyText.text = AccountManager.Instance.GetCurrencyAmount(Data.Enum.CurrencyType.EnhanceStone);
            // // requiredCurrencyText.text = enhanceEquipmentTemp.GetEnhanceStone().ToString();
            // selectEquipment.SetUI();
        }

        private void OnClickAllComposite()
        {
            InventoryManager.Instance.AllComposite(selectEquipment.equipmentType);
        }

        // 강화 버튼 눌렸을 때 불리는 메서드
        // public void OnClickLevelUp()
        // {
        //     if (selectEquipment.equipmentLevel >= InventoryManager.EquipmentMaxLevel) return;
        //
        //     if (selectEquipment.GetEnhanceStone() >
        //         new BigInteger(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.WeaponEnhanceStone))) return;
        //
        //     AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.WeaponEnhanceStone,
        //         selectEquipment.GetEnhanceStone());
        //     selectEquipment.Enhance();
        //     
        //     UpdateSelectedEquipmentUI(selectEquipment);
        //     UpdateSelectEquipmentData();
        //     OnClickEnhancePanel();
        //
        //     //TODO : Achievement
        //     // AchievementManager.Instance.IncreaseAchievementValue(Data.Enum.AchieveType.Enhance, 1);
        // }

        private void OnClickExit()
        {
            selectedEquipmentPanel.SetActive(false);
            squadEquipmentStatusPanel.SetActive(true);
        }

        private void OnClickAutoEquip()
        {
            InventoryManager.Instance.AutoEquip(selectEquipment.equipmentType);
        }

        public void UpdateAllCompositeButton(bool canComposite)
        {
            allCompositeButton.gameObject.SetActive(canComposite);
            allCompositeLockButton.gameObject.SetActive(!canComposite);
        }

        public void UpdateAutoEquipButton(bool canAutoEquip)
        {
            autoEquipButton.gameObject.SetActive(canAutoEquip);
            autoEquipLockButton.gameObject.SetActive(!canAutoEquip);
        }

        public void UpdateRequiredCurrencyUI(string currency)
        {
            requiredCurrencyText.text = $"<size=18><sprite={(int)Enums.IconType.CurrencyType_EnhanceStoneEquipment}></size>  {currency}";
        }

        public void UpdateOwnedEffectsText()
        {
            for (var i = 0; i < selectEquipment.ownedEffects.Count; i++)
            {
                selectEquipmentOwnedEffect[i].text = selectEquipment.ownedEffects[i].statType switch
                {
                    Enums.SquadStatType.WarriorAtk => $"(근접) 공격력 {selectEquipment.ownedEffects[i].CurrentIncreaseValue} 증가",
                    Enums.SquadStatType.ArcherAtk => $"(원거리) 공격력 {selectEquipment.ownedEffects[i].CurrentIncreaseValue} 증가",
                    Enums.SquadStatType.WizardAtk => $"(마법) 공격력 {selectEquipment.ownedEffects[i].CurrentIncreaseValue} 증가",
                    Enums.SquadStatType.Attack => $"공격력 {selectEquipment.ownedEffects[i].CurrentIncreaseValue} 증가",
                    Enums.SquadStatType.Health => $"체력 {selectEquipment.ownedEffects[i].CurrentIncreaseValue} 증가",
                    Enums.SquadStatType.Defence => $"방어력 {selectEquipment.ownedEffects[i].CurrentIncreaseValue} 증가",
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                selectEquipmentOwnedEffect[i].gameObject.SetActive(true);
            }
        }

        // // 선택한 장비 데이터 업데이트 (저장한다고 생각하면 편함)
        // public void UpdateSelectEquipmentData()
        // {
        //     InventoryManager.SetEquipment(selectEquipment.equipmentId, selectEquipment);
        // }
    }
}