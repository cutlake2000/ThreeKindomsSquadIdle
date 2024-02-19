using System.Collections;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.GameManager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Creature.CreatureClass.MonsterClass
{
    
    public class BossMonster : Monster
    {
        [SerializeField] private bool isEventHitRunning;

        public BigInteger currentBossHealth;
        public BigInteger currentBossDefence;
        public BigInteger maxBossHealth;
        
        public SpriteRenderer spriteRenderer;
        
        private readonly WaitForSeconds hitEffectDelay = new(0.2f);

        protected override void Update(){}
        
        protected override void OnEnable(){}
        
        public void InitializeBossMonsterData(BigInteger increaseStatPercent)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            maxBossHealth = MonsterManager.Instance.bossMonsterBaseStats.maxHealth * increaseStatPercent;
            currentBossHealth = MonsterManager.Instance.bossMonsterBaseStats.maxHealth * increaseStatPercent;
            currentBossDefence = MonsterManager.Instance.bossMonsterBaseStats.defence * increaseStatPercent;
        }

        public override void TakeDamage(BigInteger inputDamage)
        {
            var randomDamage = Random.Range(-MonsterManager.Instance.totalAttackAdjustValue, MonsterManager.Instance.totalAttackAdjustValue + 1) + 100;
            var damageReductionPercentage = MonsterManager.Instance.damageReduction;
            var reduction = 100 * inputDamage / (inputDamage + currentBossDefence + damageReductionPercentage);
            var adjustDamage = inputDamage * (randomDamage + reduction) / 100;
            
            currentBossHealth -= adjustDamage;
            currentBossHealth = currentBossHealth < 0 ? 0 : currentBossHealth;
            
            if (isEventHitRunning == false && !isDead)
            {
                StartCoroutine(EventHit());
            }
            
            var bounds = GetComponent<Collider2D>().bounds;
            var damageEffectSpawnPosition = bounds.center + new Vector3(0.0f, bounds.extents.y + 1f, 0.0f);
            EffectManager.Instance.CreateEffectsAtPosition(FunctionManager.Vector3ToVector2(damageEffectSpawnPosition),
                inputDamage.ChangeMoney(), Enums.PoolType.EffectDamage);
            
            DungeonManager.CheckRemainedBossHealth?.Invoke(currentBossHealth, maxBossHealth);

            if (currentBossHealth > 0 || isDead) return;
            isDead = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            StageManager.CheckRemainedMonsterAction.Invoke();
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