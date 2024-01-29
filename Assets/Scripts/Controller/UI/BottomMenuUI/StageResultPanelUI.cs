using UnityEngine;

namespace Controller.UI.BottomMenuUI
{
    public class StageResultPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject StageClearMessage;
        [SerializeField] private GameObject StageFailMessage;

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

        public void PopUnderStageClearMessage()
        {
            StageClearMessage.SetActive(false);
            StageFailMessage.SetActive(false);
        }
    }
}