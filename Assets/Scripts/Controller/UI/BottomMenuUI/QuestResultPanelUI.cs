using System;
using Function;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI
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
        
        public void UpdateQuestResultPanelUI(Sprite image, String value)
        {
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
            gameObject.SetActive(false);
            exitButton.interactable = false;
        }
    }
}