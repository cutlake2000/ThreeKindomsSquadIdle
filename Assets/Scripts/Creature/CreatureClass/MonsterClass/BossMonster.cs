using System.Collections;
using Function;
using Managers.BattleManager;
using UnityEngine;

namespace Creature.CreatureClass.MonsterClass
{
    
    public class BossMonster : Monster
    {
        [Header("HitEffects")] [SerializeField] private bool isEventHitRunning;

        public int initialBossMonsterHealth;
        public int initialBossMonsterDefence;
        
        public SpriteRenderer spriteRenderer;
        
        private readonly WaitForSeconds hitEffectDelay = new(0.2f);

        protected override void Update(){}
        
        protected override void OnEnable(){}
        
        public void InitializeBossMonsterData(int dungeonLevel)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            currentHealth = initialBossMonsterHealth * dungeonLevel;
            maxHealth = currentHealth;
            defence = initialBossMonsterDefence * dungeonLevel;
        }
        
        public override void TakeDamage(BigInteger damage)
        {
            currentHealth -= (damage - defence);
            currentHealth = currentHealth < 0 ? 0 : currentHealth;
            
            if (isEventHitRunning == false && !isDead) StartCoroutine(EventHit());
            
            StageManager.CheckRemainedBossHealth?.Invoke(BigInteger.ToInt32(currentHealth * 100 / maxHealth));

            if (currentHealth > 0 || isDead) return;
            isDead = true;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }

        private IEnumerator EventHit()
        {
            isEventHitRunning = true;

            spriteRenderer.color = new Color(0.83f, 0f, 0f);

            yield return hitEffectDelay;

            spriteRenderer.color = Color.white;

            isEventHitRunning = false;
        }
    }
}