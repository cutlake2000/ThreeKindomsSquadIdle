using System;
using System.Collections.Generic;
using Creature.Data;
using Data;
using Function;
using Managers.BattleManager;
using Managers.BottomMenuManager.InventoryPanel;
using TMPro;
using UnityEngine;
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

        [Header("스쿼드 구성 패널 아이템 UI")]
        public List<GameObject> inventoryScrollViewItemSwords = new();
        public List<GameObject> inventoryScrollViewItemBows = new();
        public List<GameObject> inventoryScrollViewItemStaffs = new();
        public List<GameObject> inventoryScrollViewItemHelmets = new();
        public List<GameObject> inventoryScrollViewItemArmors = new();
        public List<GameObject> inventoryScrollViewItemGauntlets = new();

        [Header("캐릭터 스폰 위치")] public List<GameObject> spawnTargetPosition;

        [Space(5)]
        [Header("=== 장비 선택 팝업 창 ===")]
        [Header("전체 합성 버튼")] public Button allCompositeButton;

        [Header("자동 장착 버튼")] public Button autoEquipButton;
        [Header("레벨 업 버튼")] public Button levelUpButton;

        [Header("선택 장비")] public Equipment selectEquipment;
        [Header("선택 장비 인덱스")] public int currentSelectedInventoryITemIndex;

        [Header("선택 장비 정보")]
        public Image selectEquipmentIcon;
        public Image selectEquipmentBackground;
        public Image selectEquipmentBackgroundEffect;
        public TMP_Text selectEquipmentName;
        public TMP_Text selectEquipmentGrade;
        public TMP_Text selectEquipmentEquippedEffect;
        public TMP_Text selectEquipmentOwnedEffect;
        public TMP_Text selectEquipmentLevel;

        // 이벤트 설정하는 메서드
        private void OnEnable()
        {
            OnClickSelectEquipment += SelectEquipment;
            UpdateEquipmentUIAction += SetOnEquippedBtnUI;
        }

        private void OnDisable()
        {
            OnClickSelectEquipment -= SelectEquipment;
            UpdateEquipmentUIAction -= SetOnEquippedBtnUI;
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
            levelUpButton.onClick.AddListener(OnClickLevelUp);
        }

        private void OnClickEquipment(int index)
        {
            for (var i = 0; i < scrollViewEquipmentPanel.Length; i++) scrollViewEquipmentPanel[i].SetActive(i == index);
            for (var i = 0; i < spawnTargetPosition.Count; i++) spawnTargetPosition[i].SetActive(i == index);
        }
        
        private void OnClickWeapon(int index)
        {
            for (var i = 0; i < spawnTargetPosition.Count; i++) spawnTargetPosition[i].SetActive(i == index);
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
            }
            else
            {
                if (squadEquipmentStatusPanel.activeInHierarchy) squadEquipmentStatusPanel.SetActive(false);
                if (!selectedEquipmentPanel.activeInHierarchy) selectedEquipmentPanel.SetActive(true);

                selectEquipment = equipment;
            
                UpdateSelectedEquipmentUI(selectEquipment);
            }
        }

        // 선택 장비 데이터 UI로 보여주는 메서드
        private void UpdateSelectedEquipmentUI(Equipment equipment)
        {
            Debug.Log(selectEquipmentIcon);
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
                Enums.EquipmentRarity.Legend => "레전드",
                _ => throw new ArgumentOutOfRangeException()
            };
            
            selectEquipmentEquippedEffect.text =
                $"공격력 {equipment.equippedEffects[0].increaseValue}% 증가";
            selectEquipmentOwnedEffect.text = $"체력 {equipment.ownedEffects[0].increaseValue}% 증가";
        }
        
        // 장착 버튼 활성화 / 비활성화 메서드
        private void SetOnEquippedBtnUI(bool onEquipped)
        {
            // if (onEquipped)
            // {
            //     equipBtn.gameObject.SetActive(false);
            //     unEquipBtn.gameObject.SetActive(true);
            // }
            // else
            // {
            //     equipBtn.gameObject.SetActive(true);
            //     unEquipBtn.gameObject.SetActive(false);
            // }
        }

        // 강화 판넬 버튼 눌렸을 때 불리는 메서드
        public void OnClickEnhancePanel()
        {
            var enhanceEquipmentTemp = InventoryManager.GetEquipment(selectEquipment.equipmentId);
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
        public void OnClickLevelUp()
        {
            if (selectEquipment.equipmentLevel >= InventoryManager.EquipmentMaxLevel) return;

            if (selectEquipment.GetEnhanceStone() >
                new BigInteger(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.WeaponEnhanceStone))) return;

            AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.WeaponEnhanceStone,
                selectEquipment.GetEnhanceStone());
            selectEquipment.Enhance();
            UpdateSelectedEquipmentUI(selectEquipment);


            if (selectEquipment.isEquipped) OnClickEquip();

            UpdateSelectEquipmentData();

            OnClickEnhancePanel();

            //TODO : Achievement
            // AchievementManager.Instance.IncreaseAchievementValue(Data.Enum.AchieveType.Enhance, 1);
        }

        private void OnClickExit()
        {
            selectedEquipmentPanel.SetActive(false);
            squadEquipmentStatusPanel.SetActive(true);
        }

        // 장착 버튼 눌렸을 때 불리는 메서드
        public void OnClickEquip()
        {
            SquadBattleManager.EquipAction?.Invoke(InventoryManager.GetEquipment(selectEquipment.equipmentId));
        }

        private void OnClickAutoEquip()
        {
            InventoryManager.Instance.AutoEquip(selectEquipment.equipmentType);
            SquadBattleManager.EquipAction?.Invoke(InventoryManager.GetEquipment(selectEquipment.equipmentId));
        }

        // 선택한 장비 데이터 업데이트 (저장한다고 생각하면 편함)
        public void UpdateSelectEquipmentData()
        {
            InventoryManager.SetEquipment(selectEquipment.equipmentId, selectEquipment);
        }
    }
}