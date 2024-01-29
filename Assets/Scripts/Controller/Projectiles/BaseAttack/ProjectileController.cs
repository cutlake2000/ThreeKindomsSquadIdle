using Function;
using UnityEngine;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileController : MonoBehaviour
    {
        protected BigInteger Damage;
        protected Vector3 Direction;

        protected virtual void AttackEnemy()
        {
        }

        protected virtual void AttackEnemy(Collider2D collision)
        {
        }

        protected void FlipSprite(float directionX)
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