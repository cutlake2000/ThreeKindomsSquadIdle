using System;
using Function;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Controller.UI.BottomMenuUI.SquadMenu
{
    public class SquadUI : MonoBehaviour
    {
        public static SquadUI Instance;

        [Header("=== 스쿼드 스탯 패널 ===")]
        [Header("--- 스쿼드 스탯 패널 UI ---")]
        [SerializeField] private TMP_Text squadLevelText;
        [SerializeField] private TMP_Text squadStatPointText;
        [SerializeField] private Slider squadExpSlider;
        [SerializeField] private TMP_Text squadExpText;
        [SerializeField] private Button levelUpButton;
        [SerializeField] private Button[] levelUpMagnificationButton;
        
        [Space(5)]
        [SerializeField]
        public int levelUpMagnification;
        
        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            InitializeButtonListeners();
        }

        private void InitializeButtonListeners()
        {
            for (var i = 0; i < levelUpMagnificationButton.Length; i++)
            {
                var index = i;
                levelUpMagnificationButton[i].GetComponent<Button>().onClick.AddListener(() => InitializeLevelUpMagnificationButton(index));
            }
        }

        public void InitializeLevelUpMagnificationButton(int index)
        {
            for (var i = 0; i < levelUpMagnificationButton.Length; i++)
            {
                if (i == index)
                {
                    var color = Color.white;
                    color.a = 1;
                    levelUpMagnificationButton[i].GetComponent<Image>().color = color;
                    levelUpMagnificationButton[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
                    
                    levelUpMagnification = (int) Mathf.Pow(10, i);
                    
                    CheckRequiredStatPointOfMagnificationButton(i);
                }
                else
                {
                    var color = Color.black;
                    color.a = 50.0f / 255.0f;
                    levelUpMagnificationButton[i].GetComponent<Image>().color = color;
                    levelUpMagnificationButton[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                }
            }
        }

        public void CheckRequiredStatPointOfMagnificationButton(int index)
        {
            Debug.Log($"계십니까~ {Convert.ToInt32(AccountManager.Instance.GetCurrencyAmount(Enum.CurrencyType.StatPoint))} | {levelUpMagnification * SquadStatManager.Instance.squadStats[index].levelUpCost}");
            var levelUpCost = SquadStatManager.Instance.squadStats[index].levelUpCost;
            
            if (Convert.ToInt32(AccountManager.Instance.GetCurrencyAmount(Enum.CurrencyType.StatPoint)) < levelUpMagnification * levelUpCost)
            {
                foreach (var squadStat in SquadStatManager.Instance.squadStats)
                {
                    if (squadStat.maxLevel - squadStat.currentLevel <= Convert.ToInt32(AccountManager.Instance.GetCurrencyAmount(Enum.CurrencyType.StatPoint))) continue;
                    
                    squadStat.upgradeButton.gameObject.SetActive(false);
                    squadStat.upgradeBlockButton.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var squadStat in SquadStatManager.Instance.squadStats)
                {
                    squadStat.upgradeButton.gameObject.SetActive(true);
                    squadStat.upgradeBlockButton.gameObject.SetActive(false);
                }
            }
        }
    }
}