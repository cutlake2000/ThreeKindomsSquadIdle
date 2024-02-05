using System;
using UnityEngine;

namespace Controller.Effects
{
    public class EnhanceEffect : MonoBehaviour
    {
        private Animator animator;
        private bool animationFinished;
        private static readonly int Effect = Animator.StringToHash("Effect");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = false;
        }

        public void StartEffect()
        {
            gameObject.SetActive(true);
            animator.SetTrigger(Effect);
        }

        public void InactiveEffect()
        {
            gameObject.SetActive(false);
        }
    }
}