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
    [Serializable]
    public struct SquadPanelUI
    {
        [Header("=== 스쿼드 스탯 패널 ===")]
        [Header("--- 스쿼드 스탯 패널 UI ---")]
        public TMP_Text squadLevelText;
        public TMP_Text squadStatPointText;
        public Slider squadExpSlider;
        public TMP_Text squadExpText;
        public Button levelUpButton;
        public Button[] levelUpMagnificationButton;
        
        [Space(5)]
        public int levelUpMagnification;
        
        public void InitializeEventListeners()
        {
            for (var i = 0; i < levelUpMagnificationButton.Length; i++)
            {
                var index = i;
                var squadPanel = this;
                levelUpMagnificationButton[i].GetComponent<Button>().onClick.AddListener(() => squadPanel.InitializeLevelUpMagnificationButton(index));
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
            Debug.Log($"계십니까~ {Convert.ToInt32(AccountManager.Instance.GetCurrencyAmount(Enum.CurrencyType.StatPoint))} | {levelUpMagnification * SquadStatManager.Instance.squadStatItem[index].levelUpCost}");
            var levelUpCost = SquadStatManager.Instance.squadStatItem[index].levelUpCost;
            
            if (Convert.ToInt32(AccountManager.Instance.GetCurrencyAmount(Enum.CurrencyType.StatPoint)) < levelUpMagnification * levelUpCost)
            {
                foreach (var squadStat in SquadStatManager.Instance.squadStatItem)
                {
                    if (squadStat.maxLevel - squadStat.currentLevel <= Convert.ToInt32(AccountManager.Instance.GetCurrencyAmount(Enum.CurrencyType.StatPoint))) continue;
                    
                    squadStat.upgradeButton.gameObject.SetActive(false);
                    squadStat.upgradeBlockButton.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var squadStat in SquadStatManager.Instance.squadStatItem)
                {
                    squadStat.upgradeButton.gameObject.SetActive(true);
                    squadStat.upgradeBlockButton.gameObject.SetActive(false);
                }
            }
        }
    }
}