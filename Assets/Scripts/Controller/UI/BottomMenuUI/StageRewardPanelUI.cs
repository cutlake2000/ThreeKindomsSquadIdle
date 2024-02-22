using System;
using System.Collections.Generic;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI
{
    [Serializable]
    public class TargetObject
    {
        public List<GameObject> activeObjects;
    }
    
    public class StageRewardPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject stageClearMessage;
        [SerializeField] private GameObject stageFailMessage;
        [SerializeField] private GameObject stageFailExtraDescriptionMessage;
        
        [Header("StageClear 패널")]
        [SerializeField] private Image reward1Sprite;
        [SerializeField] private TMP_Text reward1Text;
        [SerializeField] private Image reward2Sprite;
        [SerializeField] private TMP_Text reward2Text;

        [Header("StageFail 패널")]
        [SerializeField] private List<Button> goToPanelButtons;
        [SerializeField] private List<TargetObject> targetObjects;
        [SerializeField] private Button exitButton;

        public void InitializeEventListener()
        {
            for (var i = 0 ; i < goToPanelButtons.Count ; i++)
            {
                var index = i;
                goToPanelButtons[i].onClick.AddListener(() => OpenTargetPanelUI(index));
            }
            
            exitButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void OpenTargetPanelUI(int index)
        {
            foreach (var target in targetObjects[index].activeObjects)
            {
                target.SetActive(true);
            }
            
            gameObject.SetActive(false);
            QuestManager.Instance.backboardPanel.SetActive(true);
        }
        
        public void PopUpStageClearMessage(bool isClear)
        {
            switch (isClear)
            {
                case true:
                    stageClearMessage.SetActive(true);
                    stageFailMessage.SetActive(false);
                    stageFailExtraDescriptionMessage.SetActive(false);
                    break;
                case false:
                    stageClearMessage.SetActive(false);

                    var targetFailMessage = QuestManager.Instance.questLevel >= 18;
                    stageFailMessage.SetActive(!targetFailMessage);
                    stageFailExtraDescriptionMessage.SetActive(targetFailMessage);
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
            stageClearMessage.SetActive(false);
            stageFailMessage.SetActive(false);
            stageFailExtraDescriptionMessage.SetActive(false);
        }
    }
}