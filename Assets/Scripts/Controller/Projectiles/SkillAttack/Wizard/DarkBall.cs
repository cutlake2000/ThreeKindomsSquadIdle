using System.Collections;
using Module;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Wizard
{
    public class DarkBall : SkillAttackController
    {
        private readonly WaitForSeconds particleStandByTime = new(0.5f);
        
        protected override void ActivateSkill()
        {
            foreach (var ac in attackColliders)
            {
                ac.GetComponent<AttackCollider>().Damage = skillDamage * 2;
            }
            
            projectileTransform.position = targetPosition;
            
            StartCoroutine(MultiHitCollider());
        }
        
        private IEnumerator MultiHitCollider()
        {
            foreach (var t in attackColliders)
            {
                t.SetActive(false);
                
                yield return particleStandByTime;
                
                t.SetActive(true);
            }
        }
    }
}