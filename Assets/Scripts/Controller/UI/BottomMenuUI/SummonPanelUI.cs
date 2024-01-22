using System;
using System.Collections;
using System.Collections.Generic;
using Creature.Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Controller.UI.BottomMenuUI
{
    public class SummonPanelUI : MonoBehaviour
    {
        public static SummonPanelUI Instance;

        [SerializeField] private List<Equipment> summonLists = new();

        public static Action<Enum.SummonEquipmentType, int> OnSummon;
        
        [Header("무기 소환 레벨 / 무기 소환 경험치 /  장비 소환 레벨 / 장비 소환 경험치")]
        [SerializeField] private TMP_Text currentSquadSummonLevelText;
        [SerializeField] private Slider currentSquadSummonExpSlider;
        [SerializeField] private TMP_Text currentWeaponSummonLevelText;
        [SerializeField] private Slider currentWeaponSummonExpSlider;
        [SerializeField] private TMP_Text currentGearSummonLevelText;
        [SerializeField] private Slider currentGearSummonExpSlider;
        
        [Header("소환 패널 - 소환 레벨 / 소환 경험치 / 소환 슬라이더")]
        [SerializeField] private TMP_Text summonPanelSummonLevelText;
        [SerializeField] private TMP_Text summonPanelSummonExpText;
        [SerializeField] private Slider summonPanelSummonExpSlider;
            
        [Header("무기 1회 / 10회 / 100회 소환 버튼")]
        [SerializeField] private Button weapon1SummonBtn;
        [SerializeField] private Button weapon30SummonBtn;
        [SerializeField] private Button weapon100SummonBtn;
            
        [Header("장비 1회 / 10회 / 100회 소환 버튼")]
        [SerializeField] private Button armor1SummonBtn;
        [SerializeField] private Button armor30SummonBtn;
        [SerializeField] private Button armor100SummonBtn;
        
        [Header("추가 30회 / 100회 / 250회 소환 버튼")]
        [SerializeField] private Button extra30SummonBtn;
        [SerializeField] private Button extra100SummonBtn;
        [SerializeField] private Button extra250SummonBtn;

        [Header("오브젝트 풀링")]
        [SerializeField] private int defaultCapacity = 20;
        [SerializeField] private int maxPoolSize = 30;
        [SerializeField] private GameObject equipmentPrefab;
        [SerializeField] private GameObject spawnTarget;

        private IObjectPool<Equipment> SummonPool { get; set; }
        
        [Header("소환 결과창")]
        [SerializeField] private GameObject summonResultPanel;
        [SerializeField] private Button summonResultExitButton;
        [SerializeField] private Enum.SummonEquipmentType currentSummonEquipmentType;
        [SerializeField] private WaitForSeconds summonWaitForSeconds = new WaitForSeconds(0.1f);
        private readonly Vector3 initScale = new(1, 1, 1);

        private void Awake()
        {
            Instance = this;
            summonLists = new List<Equipment>();
            SummonPool = new ObjectPool<Equipment>(CreateSummon, OnGetSummon, OnReleaseSummon, OnDestroySummon, false, defaultCapacity, maxPoolSize);
            
            for (var i = 0; i < defaultCapacity; i++)
            {
                var equipment = CreateSummon().GetComponent<Equipment>();
                equipment.ManagedPool.Release(equipment);
            }
        }

        private void Start()
        {
            InitializeButtonListeners();
            SetupEventListeners();
        }

        private void SetupEventListeners()
        {
            OnSummon += IncreaseExp;
            OnSummon += GetObjectFromPool;
        }
        
        private void InitializeButtonListeners()
        {
            weapon1SummonBtn.onClick.AddListener(() => OnSummonEquipment(Enum.SummonEquipmentType.Weapon, 10));
            weapon30SummonBtn.onClick.AddListener(() => OnSummonEquipment(Enum.SummonEquipmentType.Weapon,30));
            weapon100SummonBtn.onClick.AddListener(() => OnSummonEquipment(Enum.SummonEquipmentType.Weapon,100));
            armor1SummonBtn.onClick.AddListener(() => OnSummonEquipment(Enum.SummonEquipmentType.Gear,10));
            armor30SummonBtn.onClick.AddListener(() => OnSummonEquipment(Enum.SummonEquipmentType.Gear,30));
            armor100SummonBtn.onClick.AddListener(() => OnSummonEquipment(Enum.SummonEquipmentType.Gear,100));
            summonResultExitButton.onClick.AddListener(OnClickExit);
            
            extra30SummonBtn.onClick.AddListener(() => OnSummonEquipment(currentSummonEquipmentType, 30));
            extra100SummonBtn.onClick.AddListener(() => OnSummonEquipment(currentSummonEquipmentType, 100));
            extra250SummonBtn.onClick.AddListener(() => OnSummonEquipment(currentSummonEquipmentType, 250));
        }
        
        private void OnClickExit()
        {
            summonResultPanel.SetActive(false);
            
            ResetSummonPool();
        }

        private void OnSummonEquipment(Enum.SummonEquipmentType type, int count)
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.Dia, count * 10)) return;
            
            ResetSummonPool();
            SetSummonResultPanel();

            currentSummonEquipmentType = type;
                
            SummonManager.OnSummonEquipment?.Invoke(type, count);
        }
            
        private static void IncreaseExp(Enum.SummonEquipmentType type, int count)
        {
            switch (type)
            {
                case Enum.SummonEquipmentType.Weapon:
                    SquadManager.Instance.SummonLevel.IncreaseExp(type, count);
                    break;
                case Enum.SummonEquipmentType.Gear:
                    SquadManager.Instance.SummonLevel.IncreaseExp(type, count);
                    break;
            }
        }

        private void SetSummonResultPanel()
        {
            summonResultPanel.SetActive(true);
            SetSummonUIOnSummonResult();
        }

        private void SetSummonUIOnSummonResult()
        {
            switch (currentSummonEquipmentType)
            {
                case Enum.SummonEquipmentType.Weapon:
                    summonPanelSummonLevelText.text = $"소환 레벨 {SquadManager.Instance.SummonLevel.CurrentWeaponLevel}";
                    summonPanelSummonExpText.text = $"{SquadManager.Instance.SummonLevel.CurrentWeaponExp} / {SquadManager.Instance.SummonLevel.MaxWeaponExp}";
                    summonPanelSummonExpSlider.value = SquadManager.Instance.SummonLevel.CurrentWeaponExp / SquadManager.Instance.SummonLevel.MaxWeaponExp;
                    break;
                case Enum.SummonEquipmentType.Gear:
                    summonPanelSummonLevelText.text = $"소환 레벨 {SquadManager.Instance.SummonLevel.CurrentGearLevel}";
                    summonPanelSummonExpText.text = $"{SquadManager.Instance.SummonLevel.CurrentGearExp} / {SquadManager.Instance.SummonLevel.MaxGearExp}";
                    summonPanelSummonExpSlider.value = SquadManager.Instance.SummonLevel.CurrentGearExp / SquadManager.Instance.SummonLevel.MaxGearExp;
                    break;
            }
        }

        private void GetObjectFromPool(Enum.SummonEquipmentType type, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var equipment = SummonPool.Get();
                summonLists.Add(equipment);
            }

            SetSummonedEquipmentInfo();
            SetSummonUIOnSummonResult();
            StartCoroutine(SetSummonedEquipmentOnPanel());
        }

        private void SetSummonedEquipmentInfo()
        {
            var index = 0;
            
            foreach (var equipment in SummonManager.Instance.summonedEquipmentList)
            {
                summonLists[index].equipmentRarity = equipment.equipmentRarity;
                summonLists[index].tier = equipment.tier;
                summonLists[index].summonCount = equipment.summonCount;
                summonLists[index].equipmentImage = equipment.equipmentImage;
                summonLists[index].equipmentBackground = equipment.equipmentBackground;
                
                index++;
            }
        }

        private IEnumerator SetSummonedEquipmentOnPanel()
        {
            for (var i = summonLists.Count; i > 0 ; i--)
            {
                summonLists[i - 1].gameObject.SetActive(true);
                summonLists[i - 1].SetSummonUI();
                yield return summonWaitForSeconds;
            }
        }

        public void SetSummonUI()
        {
            currentSquadSummonLevelText.text = $"소환 레벨 : {SquadManager.Instance.SummonLevel.CurrentSquadLevel} ({SquadManager.Instance.SummonLevel.CurrentSquadExp} / {SquadManager.Instance.SummonLevel.MaxSquadExp})";
            currentSquadSummonExpSlider.value = SquadManager.Instance.SummonLevel.CurrentSquadExp / SquadManager.Instance.SummonLevel.MaxSquadExp;
            
            currentWeaponSummonLevelText.text = $"소환 레벨 : {SquadManager.Instance.SummonLevel.CurrentWeaponLevel} ({SquadManager.Instance.SummonLevel.CurrentWeaponExp} / {SquadManager.Instance.SummonLevel.MaxWeaponExp})";
            currentWeaponSummonExpSlider.value = SquadManager.Instance.SummonLevel.CurrentWeaponExp / SquadManager.Instance.SummonLevel.MaxWeaponExp;
                
            currentGearSummonLevelText.text = $"소환 레벨 : {SquadManager.Instance.SummonLevel.CurrentGearLevel} ({SquadManager.Instance.SummonLevel.CurrentGearExp} / {SquadManager.Instance.SummonLevel.MaxGearExp})";
            currentGearSummonExpSlider.value = SquadManager.Instance.SummonLevel.CurrentGearExp / SquadManager.Instance.SummonLevel.MaxWeaponExp;
        }
        
        private Equipment CreateSummon()
        {
            var equipment = Instantiate(equipmentPrefab).GetComponent<Equipment>();
            var equipmentTransform = equipment.transform;
            equipmentTransform.SetParent(spawnTarget.transform);
            equipmentTransform.localScale = initScale;
            equipment.SetManagedPool(SummonPool);
            
            return equipment;
        }
        
        private static void OnGetSummon(Equipment obj)
        {
            // obj.gameObject.SetActive(true);
        }

        private static void OnReleaseSummon(Equipment obj)
        {
            obj.gameObject.SetActive(false);
        }

        private static void OnDestroySummon(Equipment obj)
        {
            Destroy(obj.gameObject);
        }

        public void ResetSummonPool()
        { 
            foreach (var equipment in summonLists)
            {
                SummonPool.Release(equipment);
            }
            
            summonLists.Clear();
            SummonManager.Instance.SummonedItemDictionary.Clear();
            SummonManager.Instance.summonedEquipmentList.Clear();
        }
    }
}