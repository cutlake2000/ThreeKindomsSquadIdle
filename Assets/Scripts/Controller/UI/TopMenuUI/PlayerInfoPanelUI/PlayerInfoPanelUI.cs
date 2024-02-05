using Data;
using Managers.BattleManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.TopMenuUI.PlayerInfoPanelUI
{
    public class PlayerInfoPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text accountName;
        [SerializeField] private TMP_Text accountLevel;

        public void UpdatePlayerInfoPanelNickNameUI(string nickName)
        {
            accountName.text = $"{nickName}";
        }
        public void UpdatePlayerInfoPanelLevelUI(int level)
        {
            accountLevel.text = $"Lv. {level}";
        }
    }
}