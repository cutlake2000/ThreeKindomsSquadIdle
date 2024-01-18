using System;
using UnityEngine;

namespace Module
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        public event Action OnNormalAttackEffect;
        public event Action OnNormalAttack;
        public event Action OnSkill1Attack;
        public event Action OnSkill2Attack;
        public event Action OnDie;

        private void InvokeOnNormalAttackEffectEvent(int characterClass)
        {
            OnNormalAttackEffect?.Invoke();
        }

        private void InvokeNormalAttackEvent(int characterClass)
        {
            OnNormalAttack?.Invoke();
        }

        private void InvokeSkill1AttackEvent(int characterClass)
        {
            OnSkill1Attack?.Invoke();
        }

        private void InvokeSkill2AttackEvent(int characterClass)
        {
            OnSkill2Attack?.Invoke();
        }

        private void InvokeDieAttackEvent(int characterClass)
        {
            OnDie?.Invoke();
        }
    }
}