using System;
using Managers.BattleManager;
using Managers.BottomMenuManager.SquadPanel;
using Managers.BottomMenuManager.TalentPanel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.TalentPanel
{
    public class TalentPanelUI : MonoBehaviour
    {
        public Button[] levelUpMagnificationButton;

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

                    CheckRequiredCurrencyOfMagnificationButton(i);
                }
                else
                {
                    var color = Color.black;
                    color.a = 50.0f / 255.0f;
                    levelUpMagnificationButton[i].GetComponent<Image>().color = color;
                    levelUpMagnificationButton[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                }
        }

        public static void CheckRequiredCurrencyOfMagnificationButton(int index)
        {
            var levelUpCost = TalentManager.Instance.talentItem[index].levelUpCost;

            if (Convert.ToInt32(AccountManager.Instance.GetCurrencyAmount(Enum.CurrencyType.Gold)) <
                SquadStatManager.Instance.levelUpMagnification * levelUpCost)
                foreach (var talentItem in TalentManager.Instance.talentItem)
                {
                    if (talentItem.maxLevel - talentItem.currentLevel <=
                        Convert.ToInt32(AccountManager.Instance.GetCurrencyAmount(Enum.CurrencyType.Gold))) continue;

                    talentItem.upgradeButton.gameObject.SetActive(false);
                    talentItem.upgradeBlockButton.gameObject.SetActive(true);
                }
            else
                foreach (var talentItem in TalentManager.Instance.talentItem)
                {
                    talentItem.upgradeButton.gameObject.SetActive(true);
                    talentItem.upgradeBlockButton.gameObject.SetActive(false);
                }
        }
    }
}