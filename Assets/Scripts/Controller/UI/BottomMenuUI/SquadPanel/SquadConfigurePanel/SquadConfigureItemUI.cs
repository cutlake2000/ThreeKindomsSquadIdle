using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.SquadPanel.SquadConfigurePanel
{
    public class SquadConfigureItemUI : MonoBehaviour
    {
        [Header("캐릭터 레벨")]
        public TMP_Text characterLevel;
        [Header("캐릭터 이름")]
        public TMP_Text characterName;
        [Header("캐릭터 아이콘")]
        public Image characterIcon;

        public void UpdateSquadConfigureItemUI(int level, string name, Sprite icon)
        {
            characterLevel.text = $"Lv. {level}";
            characterName.text = $"Lv. {name}";
            characterIcon.sprite = icon;
        }
    }
}