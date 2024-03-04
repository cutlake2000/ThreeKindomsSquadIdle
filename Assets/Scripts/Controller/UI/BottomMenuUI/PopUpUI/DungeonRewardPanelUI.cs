using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.PopUpUI
{
    public class DungeonRewardPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject DungeonClearMessage;
        [SerializeField] private GameObject DungeonFailMessage;
        
        [SerializeField] private Image reward1Sprite;
        [SerializeField] private TMP_Text reward1Text;

        public void PopUpDungeonClearMessage(bool isClear)
        {
            switch (isClear)
            {
                case true:
                    DungeonClearMessage.SetActive(true);
                    DungeonFailMessage.SetActive(false);
                    break;
                case false:
                    DungeonClearMessage.SetActive(false);
                    DungeonFailMessage.SetActive(true);
                    break;
            }
        }

        public void UpdateRewardUI(Sprite icon1, string reward1)
        {
            reward1Sprite.sprite = icon1;
            reward1Text.text = reward1;
        }

        public void PopUnderDungeonClearMessage()
        {
            DungeonClearMessage.SetActive(false);
            DungeonFailMessage.SetActive(false);
        }
    }
}