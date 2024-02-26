using Creature.CreatureClass.MonsterClass;
using Function;
using Keiwando.BigInteger;
using Module;
using UnityEngine;

namespace Controller.Projectiles.BaseAttack
{
    public class ProjectileBaseAttackWarriorController : ProjectileController
    {
        private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        
        public AttackCollider attackCollider;
        public Animator animator;

        public void InitializeWarriorBaseAttack(BigInteger damage, Vector3 inputDirection, int criticalRatePercent, int criticalDamagePercent)
        {
            direction = inputDirection;
            FlipLocalScaleXY(direction.x);
            
            animator.SetTrigger(AttackTrigger);
            transform.right = direction * -1;

            attackCollider.InitializeAttackColliderData(damage, criticalRatePercent, criticalDamagePercent);
        }
    }
}