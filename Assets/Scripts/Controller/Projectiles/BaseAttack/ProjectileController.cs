using Function;
using Keiwando.BigInteger;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileController : MonoBehaviour
    {
        protected BigInteger Damage;
        [SerializeField] protected Vector3 direction;

        protected virtual void AttackEnemy()
        {
        }
        
        protected void FlipLocalScaleXY(float directionX)
        {
            var scale = transform.localScale;
            var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));

            switch (directionX)
            {
                case > 0f:
                    localScale = new Vector3(-localScale.x, -localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
                case < 0f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
            }
        }

        protected void FlipLocalScaleY(float directionX)
        {
            var scale = transform.localScale;
            var localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));

            switch (directionX)
            {
                case > 0f:
                    localScale = new Vector3(localScale.x, -localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
                case < 0f:
                    localScale = new Vector3(localScale.x, localScale.y, localScale.z);
                    transform.localScale = localScale;
                    break;
            }
        }
    }
}