using Managers.BottomMenuManager.InventoryPanel;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.InventoryPanel
{
    public class InventoryPanelItemUI : MonoBehaviour
    {
        [Header("장비 티어")] public TMP_Text equipmentTier;
        [Header("장비 레벨")] public TMP_Text equipmentLevel;
        [Header("장비 수량")] public TMP_Text equipmentQuantity;
        [Header("장비 수량 슬라이더")] public Slider equipmentQuantitySlider;
        [Header("장비 이미지")] public Image equipmentIcon;
        [Header("장비 배경 효과")] public Image equipmentBackgroundEffect;
        [Header("장비 배경")] public Image equipmentBackground;
        [Header("장비 장착 여부")] public GameObject equipMark;
        [Header("장비 보유 여부")] public GameObject possessMark;

        public void UpdateInventoryPanelItemUI(int level, int maxLevel, int quantity, int maxQuantity, bool isEquipped, bool isPossessed, int tier, string name, Sprite icon,
            Sprite backgroundEffect, Sprite background)
        {
            equipmentTier.text = $"{tier}티어";
            equipmentLevel.text = $"Lv.{level} / {maxLevel}";
            equipmentTier.text = $"{tier}티어";
            equipmentQuantity.text = $"{quantity} / {maxQuantity}";
            equipmentQuantitySlider.value = quantity;
            equipmentQuantitySlider.maxValue = maxQuantity;
            equipmentIcon.sprite = icon;
            equipmentBackgroundEffect.sprite = backgroundEffect;
            equipmentBackground.sprite = background;
            equipMark.SetActive(isEquipped);
            possessMark.SetActive(!isPossessed);
        }

        public void UpdateInventoryPanelItemQuantityUI(int quantity)
        {
            equipmentQuantity.text = $"{quantity} / {InventoryManager.MaxQuantity}";
            equipmentQuantitySlider.value = quantity;
            equipmentQuantitySlider.maxValue = InventoryManager.MaxQuantity;
        }

        public void UpdateInventoryPanelItemPossessMark()
        {
            possessMark.SetActive(true);
        }
    }
}