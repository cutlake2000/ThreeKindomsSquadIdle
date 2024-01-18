using TMPro;
using UnityEngine;

namespace Controller.Effects
{
    public class EffectDamageController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damage;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (!(normalizedTime >= 1.0f)) return;
            
            gameObject.SetActive(false);
        }

        public void InitializeEffectDamage(string damageText)
        {
            damage.text = damageText;
        }
    }
}