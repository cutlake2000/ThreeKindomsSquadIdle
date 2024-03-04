using System;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.PopUpUI
{
    public class QuestResultPanelUI : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private Animator questResultAnimation;
        [SerializeField] private Image questResultImage;
        [SerializeField] private TMP_Text questResultValue;
        [SerializeField] private Button exitButton;
        
        private static readonly int Effect = Animator.StringToHash("Effect");

        private void OnEnable()
        {
            questResultAnimation.SetTrigger(Effect);
        }

        public void InitializeEventListeners()
        {
            exitButton.onClick.AddListener(InactivePanel);
        }
        
        public void UpdateQuestResultPanelUI(Sprite image, String value, bool isClear)
        {
            if (isClear) gameObject.SetActive(true);
            questResultImage.sprite = image;
            questResultValue.text = value;
        }

        public void RunParticle()
        {
            exitButton.interactable = true;
            particleSystem.Play();
        }
        
        private void InactivePanel()
        {
            QuestManager.Instance.TargetQuestClear();
            
            gameObject.SetActive(false);
            exitButton.interactable = false;
        }
    }
}