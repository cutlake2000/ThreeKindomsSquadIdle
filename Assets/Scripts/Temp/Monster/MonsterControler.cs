// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class MonsterControler : MonoBehaviour
// {
//     Transform closestPlayerTransform;
//
//     [SerializeField] Rigidbody2D rb;
//
//     int moveSpeed = 1;
//
//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }
//
//     private void Start()
//     {
//         closestPlayerTransform = GameObject.FindWithTag("Player").transform;
//     }
//
//     public void SetClosestPlayer(Transform transform)
//     {
//         closestPlayerTransform = transform;
//     }
//
//     public bool Move()
//     {
//         if (closestPlayerTransform == null) return false;
//
//         Vector3 position = rb.position;
//         var direction = (closestPlayerTransform.position - position).normalized;
//
//         var newPosition = position + direction * (moveSpeed * Time.fixedDeltaTime);
//         rb.MovePosition(newPosition);
//
//         FlipSprite(direction.x);
//
//         return true;
//     }
//
//     
//     private void FlipSprite(float directionX)
//     {
//         if (closestPlayerTransform == null) return;
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
//
//     //public void Takeknockback()
//     //{
//     //    float knockbackForce = 100f;
//
//     //    Vector2 knockbackDirection = (rb.position - (Vector2)closestPlayerTransform.position).normalized;
//
//     //    rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
//     //}
// }
