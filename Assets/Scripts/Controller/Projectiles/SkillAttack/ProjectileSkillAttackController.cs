using System.Collections;
using Controller.Projectiles.BaseAttack;
using Function;
using Module;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack
{
    public class ProjectileSkillAttackController : ProjectileController
    {
        [SerializeField] private GameObject AttackCollider;

        private float particleCurrentTime = 0.0f;
        private float particleMaxTime = 0.5f;

        public void InitializeSkillAttack(BigInteger damage, Vector3 startPosition, Vector3 direction)
        {
            Direction = direction;
            FlipSprite(Direction.x);
            
            CurrentDuration = 0;
            
            transform.right = Direction * -1;
            
            AttackCollider.GetComponent<AttackCollider>().damage = damage * 2;
            gameObject.GetComponent<ParticleSystem>().Play(true);

            StartCoroutine(MoveCollider());
        }

        public IEnumerator MoveCollider()
         {
             AttackCollider.SetActive(true);
             
             while(true)
             {
                 particleCurrentTime += Time.deltaTime;
                 yield return null;

                 if (particleCurrentTime < particleMaxTime)
                 {
                     var newPositionX = -4 * particleCurrentTime / particleMaxTime;
                     var newPosition = new Vector3(newPositionX, 0, 0);
                     AttackCollider.transform.localPosition = newPosition;
                 }
                 else
                 {
                     particleCurrentTime = 0;
                     AttackCollider.SetActive(false);
                     yield break;
                 }
             }
         }
    }
}