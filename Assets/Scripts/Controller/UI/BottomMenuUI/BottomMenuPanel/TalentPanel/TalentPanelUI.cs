using System;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.BottomMenuManager.SquadPanel;
using Managers.BottomMenuManager.TalentPanel;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.TalentPanel
{
    public class TalentPanelUI : MonoBehaviour
    {
        public Button[] levelUpMagnificationButton;

        public void OnEnable()
        {
            CheckRequiredCurrencyOfMagnificationAllButton(TalentManager.Instance.levelUpMagnification);
        }

        public void InitializeEventListeners()
        {
            for (var i = 0; i < levelUpMagnificationButton.Length; i++)
            {
                var index = i;
                levelUpMagnificationButton[i].GetComponent<Button>().onClick
                    .AddListener(() => InitializeLevelUpMagnificationButton(index));
            }
        }

        public void InitializeLevelUpMagnificationButton(int index)
        {
            for (var i = 0; i < levelUpMagnificationButton.Length; i++)
                if (i == index)
                {
                    var color = Color.white;
                    color.a = 1;
                    levelUpMagnificationButton[i].GetComponent<Image>().color = color;
                    levelUpMagnificationButton[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

                    TalentManager.Instance.levelUpMagnification = (int)Mathf.Pow(10, i);

                    Debug.Log($"{TalentManager.Instance.levelUpMagnification}");

                    CheckRequiredCurrencyOfMagnificationAllButton((int) Mathf.Pow(10, i));

                    foreach (var talentItem in TalentManager.Instance.talentItem)
                    {
                        talentItem.GetComponent<TalentItemUI>().UpdateSquadTalentUI((int) Mathf.Pow(10, i));
                    }
                }
                else
                {
                    var color = Color.black;
                    color.a = 50.0f / 255.0f;
                    levelUpMagnificationButton[i].GetComponent<Image>().color = color;
                    levelUpMagnificationButton[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                }
        }

        public void CheckRequiredCurrencyOfMagnificationAllButton(int index)
        {
            foreach (var talentItem in TalentManager.Instance.talentItem)
            {
                var currentAmount = new BigInteger(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.Gold));
                var requiredAmount = talentItem.currentLevelUpCost[(int)Mathf.Log10(index)];
                
                if (currentAmount < requiredAmount)
                {
                    talentItem.upgradeButton.gameObject.SetActive(false);
                    talentItem.upgradeBlockButton.gameObject.SetActive(true);
                }
                else
                {
                    talentItem.upgradeButton.gameObject.SetActive(true);
                    talentItem.upgradeBlockButton.gameObject.SetActive(false);
                }
            }
        }

        public static void CheckRequiredCurrencyOfMagnificationButton(int index, int levelMagnification)
        {
            var talent = TalentManager.Instance.talentItem[index];

            if (new BigInteger(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.Gold)) < talent.currentLevelUpCost[(int) Mathf.Log10(levelMagnification)])
            {
                talent.upgradeButton.gameObject.SetActive(false);
                talent.upgradeBlockButton.gameObject.SetActive(true);
            }
            else
            {
                talent.upgradeButton.gameObject.SetActive(true);
                talent.upgradeBlockButton.gameObject.SetActive(false);
            }
        }
    }
}