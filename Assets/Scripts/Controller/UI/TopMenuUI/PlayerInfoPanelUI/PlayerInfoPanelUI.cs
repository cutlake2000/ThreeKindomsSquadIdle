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

        public void UpdateLevelPanelUI(int level)
        {
            accountLevel.text = $"Lv. {level}";
        }
    }
}