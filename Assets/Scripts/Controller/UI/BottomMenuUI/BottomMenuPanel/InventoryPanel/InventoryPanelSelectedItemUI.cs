using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.InventoryPanel
{
    public class InventoryPanelSelectedItemUI : MonoBehaviour
    {
        [Header("장비 티어")] public TMP_Text equipmentTier;
        [Header("장비 이미지")] public Image equipmentIcon;
        [Header("장비 배경 효과")] public Image equipmentBackgroundEffect;
        [Header("장비 배경")] public Image equipmentBackground;

        public void UpdateInventoryPanelSelectedItem(int tier, Sprite icon, Sprite backgroundEffect, Sprite background)
        {
            equipmentTier.text = $"{tier}티어";
            equipmentIcon.sprite = icon;
            equipmentBackgroundEffect.sprite = background;
            equipmentBackground.sprite = backgroundEffect;
        }
    }
}