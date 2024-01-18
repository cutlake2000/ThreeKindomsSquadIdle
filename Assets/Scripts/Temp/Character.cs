// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Character : MonoBehaviour
// {
//     [SerializeField] private Animator animator;
//
//     private Dictionary<string, float> animationLengths = new Dictionary<string, float>();
//
//     public bool isAttacking = false;
//     
//     public string EnemyTag;
//
//     protected float maxHealth = 100;
//     protected float currentHealth = 0;
//
//
//     private void Awake()
//     {
//         InitializeAnimationLengths();
//         currentHealth = maxHealth;
//     }
//
//
//     public virtual bool TakeDamage(float value)
//     {
//         currentHealth = (currentHealth - value) <= 0 ? 0 : (currentHealth - value);
//         Debug.Log("아야!" + currentHealth);
//         if (currentHealth == 0) return true;
//         return false;
//     }
//
//     public bool CheckHealth()
//     {
//         return currentHealth == 0;
//     }
//
//
//     private void InitializeAnimationLengths()
//     {
//         AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
//         foreach (AnimationClip clip in clips)
//         {
//             animationLengths[clip.name] = clip.length;
//         }
//     }
//
//     public float GetAnimationLength(string animationName)
//     {
//         if (animationLengths.TryGetValue(animationName, out float length))
//         {
//             return length;
//         }
//         else
//         {
//             Debug.LogWarning("Animation not found: " + animationName);
//             return 0f;
//         }
//     }
//
//     // 애니메이션을 키거나 끔
//     public void StartAnimation(string animationName)
//     {
//         animator.SetBool(animationName, true);
//     }
//
//     public void StopAnimation(string animationName)
//     {
//         animator.SetBool(animationName, false);
//     }
//     public void SetTriggerAnimation(string animationName)
//     {
//         animator.SetTrigger(animationName);
//     }
// }
