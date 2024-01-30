using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadConfigurePanel
{
    public class SquadConfigureItemUI : MonoBehaviour
    {
        [Header("캐릭터 레벨")] public TMP_Text characterLevel;

        [Header("캐릭터 이름")] public TMP_Text characterName;

        [Header("캐릭터 아이콘")] public Image characterIcon;

        [Header("장착 여부")] public GameObject equipMark;

        [Header("보유 여부")] public GameObject possessMark;

        public void UpdateSquadConfigureItemUI(int level, bool isEquipped, bool isPossessed, string name, Sprite icon)
        {
            characterLevel.text = $"Lv. {level}";
            characterName.text = $"{name}";
            characterIcon.sprite = icon;
            equipMark.SetActive(isEquipped);
            possessMark.SetActive(!isPossessed);
        }
    }
}