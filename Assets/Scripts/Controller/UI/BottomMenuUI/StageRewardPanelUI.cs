using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI
{
    public class StageRewardPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject StageClearMessage;
        [SerializeField] private GameObject StageFailMessage;
        
        [SerializeField] private Image reward1Sprite;
        [SerializeField] private TMP_Text reward1Text;
        [SerializeField] private Image reward2Sprite;
        [SerializeField] private TMP_Text reward2Text;

        public void PopUpStageClearMessage(bool isClear)
        {
            switch (isClear)
            {
                case true:
                    StageClearMessage.SetActive(true);
                    StageFailMessage.SetActive(false);
                    break;
                case false:
                    StageClearMessage.SetActive(false);
                    StageFailMessage.SetActive(true);
                    break;
            }
        }

        public void UpdateRewardUI(Sprite icon1, string reward1, Sprite icon2, string reward2)
        {
            reward1Sprite.sprite = icon1;
            reward1Text.text = reward1;
            reward2Sprite.sprite = icon2;
            reward2Text.text = reward2;
        }

        public void PopUnderStageClearMessage()
        {
            StageClearMessage.SetActive(false);
            StageFailMessage.SetActive(false);
        }
    }
}