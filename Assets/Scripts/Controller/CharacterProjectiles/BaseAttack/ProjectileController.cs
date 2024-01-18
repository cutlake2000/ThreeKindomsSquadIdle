using Function;
using Module;
using UnityEngine;

namespace Controller.CharacterProjectiles.BaseAttack
{
    public class ProjectileController : MonoBehaviour
    {        
        [SerializeField] protected LayerMask levelCollisionLayer;
        [SerializeField] protected float duration;
        [SerializeField] protected float attackRange;
        
        protected EnemyFinder EnemyFinder;
        protected Vector3 Direction;
        protected Collider2D[] NearbyTargets;
        protected float CurrentDuration;
        protected BigInteger Damage;
        protected bool ReadyToLaunch;
        
        protected virtual void Awake()
        {
            EnemyFinder = GetComponent<EnemyFinder>();
        }

        protected virtual void OnEnable()
        {
            NearbyTargets = null;
        }
        
        protected virtual void OnTriggerEnter2D(Collider2D collision) { }
        
        protected void FindNearbyEnemy()
        {
            if (NearbyTargets != null) return;
            
            NearbyTargets = EnemyFinder.ScanNearby(attackRange);
                
            if (NearbyTargets != null) AttackEnemy();
        }

        protected virtual void AttackEnemy() { }

        protected virtual void AttackEnemy(Collider2D collision) { }
        
        protected void FlipSprite(float directionX)
        {
            var scale = transform.localScale;
            var localScale = new Vector3(Mathf.Abs(scale.x),Mathf.Abs(scale.y), Mathf.Abs(scale.z));
            
            switch (directionX)
            {
                case > 0.1f:
                    localScale = new Vector3(localScale.x, -localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
                case < -0.1f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
            }
        }
    }
}