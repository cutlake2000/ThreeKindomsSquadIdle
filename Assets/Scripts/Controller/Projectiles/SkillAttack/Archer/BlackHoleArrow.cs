using System;
using System.Collections;
using Creature.CreatureClass.MonsterClass;
using Module;
using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Archer
{
    public class BlackHoleArrow : SkillAttackController
    {
        private readonly WaitForSeconds particleStandByTime = new(1.0f);
        
        [SerializeField] private float maxDuration;
        [SerializeField] private float currentDuration;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private bool readyToLaunch;
        [SerializeField] private bool readyToActiveSkillEffect;
        
        private void Update()
        {
            if (!readyToLaunch) return;

            currentDuration += Time.deltaTime;

            if (currentDuration < maxDuration)
            {
                if (!readyToActiveSkillEffect == false) readyToActiveSkillEffect = true;
            }
            else
            {
                readyToActiveSkillEffect = false;
                readyToLaunch = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (readyToActiveSkillEffect == false && !collision.CompareTag(Strings.TAG_ENEMY)) return;

            var enemyRigid = collision.GetComponent<Rigidbody2D>(); // 화이팅 거의 다왔다!
            if (enemyRigid != null)
            {
                StartCoroutine(PullEnemy(enemyRigid));
            }
        }

        protected override void ActivateSkill()
        {
            particleCurrentTime = 0f;
            projectileTransform.position = targetPosition;

            readyToLaunch = true;
            StartCoroutine(MultiHitCollider());
        }
        
        private IEnumerator MultiHitCollider()
        {
            foreach (var t in attackColliders)
            {
                t.SetActive(true);
                
                yield return particleStandByTime;
            
                t.SetActive(false);
            }
        }
        
        private IEnumerator PullEnemy(Rigidbody2D rigid)
        {
            var duration = 0.5f; // 적을 당겨오는 시간
            var elapsedTime = 0f;

            var initialPosition = rigid.position; // 적의 초기 위치
            Vector2 target = transform.position; // 콜라이더의 중심점 위치

            // 몬스터를 더 세게 당기기 위해 힘을 계산합니다.
            var direction = (target - initialPosition).normalized;
            var forceMagnitude = 10f; // 적을 당겨오는 힘의 크기
            var force = direction * forceMagnitude;

            while (elapsedTime < duration)
            {
                // 보정된 적의 위치 계산
                var newPosition = Vector2.Lerp(initialPosition, target, elapsedTime / duration);

                // 적 오브젝트를 새로 계산된 위치로 이동시킴
                rigid.MovePosition(newPosition);

                // 적 오브젝트에 힘을 가해 더욱 세게 당겨옵니다.
                rigid.AddForce(force, ForceMode2D.Force);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}