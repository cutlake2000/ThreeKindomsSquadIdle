using System;
using Creature.Data;
using Function;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controller.UI
{
    public class EquipmentUI : MonoBehaviour
    {
        public static event Action<Equipment> OnClickSelectEquipment;
        public static Action<bool> UpdateEquipmentUIAction;

        public static EquipmentUI Instance;

        [Header("=== 인벤토리 패널 ===")]
        public GameObject[] equipmentButton;
        [SerializeField] private GameObject[] scrollViewEquipmentPanel;
        [SerializeField] private GameObject squadEquipmentStatusPanel;
        [SerializeField] private GameObject selectedEquipmentPanel;
        
        [Space(5)]
        [Header("=== 장비 선택 팝업 창 ===")]
        [Header("--- 팝업 창 버튼 ---")]
        [SerializeField] private Button allCompositeButton;
        [SerializeField] private Button autoEquipButton;
        [SerializeField] private Button levelUpButton;
        [SerializeField] private Button exitButton;
        
        [FormerlySerializedAs("selectBaseEquipment")]
        [FormerlySerializedAs("selectEquipmentInfo")]
        [Header("=== 선택한 장비 정보 ===")]
        [Header("--- 장비 정보 ---")]
        [SerializeField] private Equipment selectEquipment;
        [SerializeField] private TMP_Text selectEquipmentName;
        [SerializeField] private TMP_Text selectEquipmentEquippedEffect;
        [SerializeField] private TMP_Text selectEquipmentOwnedEffect;
        [SerializeField] private TMP_Text selectEquipmentLevel;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializeButtonListeners();
        }

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
        private void InitializeButtonListeners()
        {
            for (var i = 0; i < equipmentButton.Length; i++)
            {
                var index = i;
                equipmentButton[i].GetComponent<Button>().onClick.AddListener(() => OnClickEquipment(index));
            }
            
            allCompositeButton.onClick.AddListener(OnClickAllComposite);
            autoEquipButton.onClick.AddListener(OnClickAutoEquip);
            levelUpButton.onClick.AddListener(OnClickLevelUp);
            exitButton.onClick.AddListener(OnClickExit);
        }

        private void OnClickEquipment(int index)
        {
            for (var i = 0; i < scrollViewEquipmentPanel.Length; i++)
            {
                scrollViewEquipmentPanel[i].SetActive(i == index);
            }
        }

        // 장비 선택 이벤트 트리거 하는 메서드 
        public static void SelectEquipmentAction(Equipment equipment)
        {
            OnClickSelectEquipment?.Invoke(equipment);
        }

        // 장비 클릭 했을 때 불리는 메서드
        public void SelectEquipment(Equipment equipment)
        {
            if (squadEquipmentStatusPanel.activeInHierarchy) squadEquipmentStatusPanel.SetActive(false);
            if (!selectedEquipmentPanel.activeInHierarchy) selectedEquipmentPanel.SetActive(true);
            
            selectEquipment.GetComponent<Equipment>().SetEquipmentInfo(equipment.GetComponent<Equipment>());
            UpdateSelectedEquipmentUI(selectEquipment);
        }
    
        private void UpdateSelectedEquipmentUI(Equipment equipment)
        {
            equipment.SetQuantityText();
            selectEquipment.GetComponent<Equipment>().SetUI();
       
            SetOnEquippedBtnUI(selectEquipment.isEquipped);
            SetSelectEquipmentTextUI(equipment);
        }


        // 선택 장비 데이터 UI로 보여주는 메서드
        private void SetSelectEquipmentTextUI(Equipment equipment)
        {
            //TODO: 추후에 해당 무기 보유 / 장착 효과에 맞게 공격력 / 방어력 증가 등 옵션 추가해야함
            selectEquipmentName.text = equipment.id;
            selectEquipmentLevel.text = $"{equipment.tier / equipment.maxLevel}";
            selectEquipmentEquippedEffect.text = $"공격력 {BigInteger.ChangeMoney(equipment.equippedEffect.ToString())}% 증가";
            selectEquipmentOwnedEffect.text = $"공격력 {equipment.ownedEffect}% 증가";
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
            var enhanceEquipmentTemp = EquipmentManager.GetEquipment(selectEquipment.id);
            selectEquipment.GetComponent<Equipment>().SetEquipmentInfo(enhanceEquipmentTemp.GetComponent<Equipment>());
            
            if (enhanceEquipmentTemp == null) return;
            
            selectEquipmentLevel.text =
                $"Lv.{enhanceEquipmentTemp.level} / {enhanceEquipmentTemp.maxLevel}"; //장비 강화(0 / 0)
            selectEquipmentEquippedEffect.text =
                $"장착 효과 : {enhanceEquipmentTemp.equippedEffect + enhanceEquipmentTemp.basicEquippedEffect}"; // 장착 효과 0 → 0
            selectEquipmentOwnedEffect.text =
                $"보유 효과  : {enhanceEquipmentTemp.ownedEffect + enhanceEquipmentTemp.basicOwnedEffect}";
            // enhanceCurrencyText.text = AccountManager.Instance.GetCurrencyAmount(Data.Enum.CurrencyType.EnhanceStone);
            // requiredCurrencyText.text = enhanceEquipmentTemp.GetEnhanceStone().ToString();
            selectEquipment.SetUI();
        }
    
        private void OnClickAllComposite()
        {
            EquipmentManager.Instance.AllComposite(selectEquipment.type);
        }

        // 강화 버튼 눌렸을 때 불리는 메서드
        public void OnClickLevelUp()
        {
            if (selectEquipment.level >= selectEquipment.maxLevel) return;
            if (selectEquipment.GetEnhanceStone() > new BigInteger(AccountManager.Instance.GetCurrencyAmount(Data.Enum.CurrencyType.EnhanceStone))) return; 
            
            AccountManager.Instance.SubtractCurrency(Data.Enum.CurrencyType.EnhanceStone,selectEquipment.GetEnhanceStone());
            selectEquipment.Enhance();
            SetSelectEquipmentTextUI(selectEquipment);


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
            SquadManager.EquipAction?.Invoke(EquipmentManager.GetEquipment(selectEquipment.id));
        }
    
        private void OnClickAutoEquip()
        {
            EquipmentManager.Instance.AutoEquip(selectEquipment.type);
        }

        // 선택한 장비 데이터 업데이트 (저장한다고 생각하면 편함)
        public void UpdateSelectEquipmentData()
        {
            EquipmentManager.SetEquipment(selectEquipment.id, selectEquipment);
        }
    }
}