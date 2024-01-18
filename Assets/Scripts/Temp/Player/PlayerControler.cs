// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.TextCore.Text;
// using static UnityEngine.GraphicsBuffer;
//
// public class PlayerControler : MonoBehaviour
// {
//     public static Action isKilled;
//
//     Player player;
//
//     [SerializeField] Rigidbody2D rb;
//     [SerializeField] Transform closestMonsterTransform;
//
//     float moveSpeed = 2;
//
//     float radius = 0.45f;
//     [SerializeField] float angle = 0f;
//     Vector3 centerPosition;
//
//     [SerializeField] GameObject attackCol;
//     float tempSpeed = 1;
//
//     private void Awake()
//     {
//         player = GetComponent<Player>();
//         rb = GetComponent<Rigidbody2D>();
//     }
//     private void Start()
//     {
//         isKilled += ResetClosestMonster;
//         centerPosition = transform.position;
//         tempSpeed = player.GetAnimationLength(Strings.ANIMATION_MELEEATTACK) /2;
//     }
//
//     public bool Move()
//     {
//         if (closestMonsterTransform == null) return false;
//
//         Vector3 position = rb.position;
//         var direction = (closestMonsterTransform.position - position).normalized;
//
//         var newPosition = position + direction * (moveSpeed * Time.fixedDeltaTime);
//         rb.MovePosition(newPosition);
//
//         FlipSprite(direction.x);
//
//         return true;
//     }
//
//     private float elapsedTime = 0f; // 경과 시간
//     public float totalTime; // 반원을 이동하는 데 걸리는 총 시간
//
//     public IEnumerator MeleeAttack()
//     {
//         totalTime = tempSpeed;
//         while(true)
//         {
//             elapsedTime += Time.deltaTime;
//             yield return null;
//
//             if (elapsedTime < totalTime)
//             {
//                 float angle = (elapsedTime / totalTime) * 120f;
//                 float radian = angle * Mathf.Deg2Rad;
//
//                 // 새 위치 계산 (x축과 y축을 사용)
//                 Vector3 newPosition = centerPosition + new Vector3(-Mathf.Sin(radian), Mathf.Cos(radian), 0) * radius;
//                 attackCol.transform.localPosition = newPosition;
//             }
//             else
//             {
//                 Debug.Log("도착!");
//                 // 이동 완료 후 초기 위치로 설정
//                 //attackCol.transform.localPosition = centerPosition - new Vector3(0, radius, 0);
//                 elapsedTime = 0; // 경과 시간 초기화
//                 yield break;
//             }
//             
//         }
//     }
//
//     public IEnumerator RangedAttack()
//     {
//         Rigidbody2D projectile = PlayerManager.instance.Getprojectile().GetComponent<Rigidbody2D>();
//         projectile.transform.position = transform.position;
//
//         projectile.gameObject.SetActive(true);
//         bool hasReachedTarget = false;
//
//         while (!hasReachedTarget || (closestMonsterTransform != null && Vector3.Distance(projectile.position, closestMonsterTransform.position) <= 1.0f))
//         {
//             if (closestMonsterTransform == null)
//             {
//                 // 목표가 사라졌을 경우, 코루틴을 종료합니다.
//                 break;
//             }
//
//             Vector2 direction = (closestMonsterTransform.position - (Vector3)projectile.position).normalized;
//             projectile.position += direction * 10 * Time.deltaTime;
//
//             if (!hasReachedTarget && Vector3.Distance(projectile.position, closestMonsterTransform.position) <= 0.1f)
//             {
//                 hasReachedTarget = true;
//             }
//
//             yield return null;
//         }
//
//         // 목표에 도달한 후 추가 이동 완료
//         PlayerManager.instance.ReturnProjectile(projectile.gameObject);
//         projectile.gameObject.SetActive(false);
//         Debug.Log("Target reached and passed!");
//     }
//
//
//     public void MeleeAttackEvent()
//     {
//         StartCoroutine(MeleeAttack());
//     }
//
//     public void RangedAttackEvent()
//     {
//         StartCoroutine(RangedAttack());
//     }
//
//     public void FindClosestMonster()
//     {
//         Collider2D[] hitColliders = BattleSystem.GetColliderInCircle(transform.position, 10, 1<<12);
//
//         float closestDistance = Mathf.Infinity;
//         Transform closestPlayer = null;
//         foreach (var hitCollider in hitColliders)
//         {
//             float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
//             if (distance < closestDistance)
//             {
//                 closestDistance = distance;
//                 closestPlayer = hitCollider.transform;
//             }
//         }
//
//         closestMonsterTransform = closestPlayer;
//     }
//
//     public bool CheckClosestMonster()
//     {
//         return closestMonsterTransform != null;
//     }
//
//     public void ResetClosestMonster()
//     {
//         closestMonsterTransform = null;
//     }
//
//     public bool CheckClosestMonsterActive()
//     {
//         return closestMonsterTransform.gameObject.activeSelf;
//     }
//
//     private void FlipSprite(float directionX)
//     {
//         if (closestMonsterTransform == null) return;
//
//         var transform = this.transform;
//         var scale = transform.localScale;
//         var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));
//
//         switch (directionX)
//         {
//             case > 0.1f:
//                 localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
//                 transform.localScale = localScale;
//                 break;
//             case < -0.1f:
//                 localScale = new Vector3(localScale.x, localScale.y, localScale.z);
//                 transform.localScale = localScale;
//                 break;
//         }
//     }
//
//     private void FlipSprite()
//     {
//         if (closestMonsterTransform == null) return;
//
//         var scale = transform.localScale;
//         var direction = (closestMonsterTransform.position - transform.position).normalized;
//         var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));
//
//         switch (direction.x)
//         {
//             case > 0.1f:
//                 localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
//                 transform.localScale = localScale;
//                 break;
//             case < -0.1f:
//                 localScale = new Vector3(localScale.x, localScale.y, localScale.z);
//                 transform.localScale = localScale;
//                 break;
//         }
//     }
//
//     private int CheckFlip()
//     {
//         if (closestMonsterTransform == null) return 0;
//
//         var scale = transform.localScale;
//         var direction = (closestMonsterTransform.position - transform.position).normalized;
//         var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));
//
//         switch (direction.x)
//         {
//             case > 0.1f:
//                 localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
//                 transform.localScale = localScale;
//                 Debug.Log("보자잉 : "+ 1);
//                 return 1;
//             case < -0.1f:
//                 localScale = new Vector3(localScale.x, localScale.y, localScale.z);
//                 transform.localScale = localScale;
//                 Debug.Log("보자잉 : " + -1);
//                 return -1;
//         }
//
//         return 0;
//     }
// }
