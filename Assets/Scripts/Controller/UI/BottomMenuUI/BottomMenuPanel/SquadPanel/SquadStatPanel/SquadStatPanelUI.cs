using System;
using Data;
using Managers.BattleManager;
using Managers.BottomMenuManager.SquadPanel;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadStatPanel
{
    public class SquadStatPanelUI : MonoBehaviour
    {
        [Header("=== 스쿼드 스탯 패널 ===")]
        [Header("--- 플레이어 정보 패널 UI ---")]
        public SquadStatPanelPlayerInfoUI squadStatPanelPlayerInfoUI;
        [Header("--- 스쿼드 스탯 패널 UI ---")]
        public Button[] levelUpMagnificationButton;

        public void InitializeEventListeners()
        {
            squadStatPanelPlayerInfoUI.InitializeEventListeners();
            
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

                    SquadStatManager.Instance.levelUpMagnification = (int)Mathf.Pow(10, i);

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

        public void CheckRequiredCurrencyOfMagnificationButton(int index)
        {
            var levelUpCost = SquadStatManager.Instance.squadStatItem[index].levelUpCost;

            if (AccountManager.Instance.statPoint < SquadStatManager.Instance.levelUpMagnification * levelUpCost)
                foreach (var squadStat in SquadStatManager.Instance.squadStatItem)
                {
                    if (squadStat.maxLevel - squadStat.currentLevel <= AccountManager.Instance.statPoint)
                        continue;

                    squadStat.upgradeButton.gameObject.SetActive(false);
                    squadStat.upgradeBlockButton.gameObject.SetActive(true);
                }
            else
                foreach (var squadStat in SquadStatManager.Instance.squadStatItem)
                {
                    squadStat.upgradeButton.gameObject.SetActive(true);
                    squadStat.upgradeBlockButton.gameObject.SetActive(false);
                }
        }
    }
}