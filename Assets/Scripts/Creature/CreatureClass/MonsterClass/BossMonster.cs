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
        
        public void InitializeBossMonsterData(int dungeonLevel)
        {
            var increaseValue = DungeonManager.Instance.increaseBossMonsterStatValuePercent;

            var increaseValueToFloat = increaseValue / 100.0f;
            var multiplier = Mathf.Pow(dungeonLevel, increaseValueToFloat);
            var finalMultiplier = Mathf.FloorToInt(multiplier);
            
            if (finalMultiplier <= 0) Debug.Log("또잉?");
            
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            maxBossHealth = MonsterManager.Instance.bossMonsterBaseStats.maxHealth * finalMultiplier;
            currentBossHealth = MonsterManager.Instance.bossMonsterBaseStats.maxHealth * finalMultiplier;
            currentBossDefence = MonsterManager.Instance.bossMonsterBaseStats.defence * finalMultiplier;
        }

        public override void TakeDamage(BigInteger inputDamage, int criticalRate, int criticalDamage)
        {
            var criticalCheckDamage = inputDamage;
            
            var randomPickCriticalRate = Random.Range(1, 10001);
            var isCritical = false;
            
            if (criticalRate >= randomPickCriticalRate)
            {
                isCritical = true;
                criticalCheckDamage = criticalCheckDamage * criticalDamage / 10000;
            }
            
            var randomDamage = Random.Range(-MonsterManager.Instance.totalAttackAdjustValue, MonsterManager.Instance.totalAttackAdjustValue + 1) + 100;
            var adjustDamage = criticalCheckDamage * randomDamage / 100;
            
            currentBossHealth -= adjustDamage;
            currentBossHealth = currentBossHealth < 0 ? 0 : currentBossHealth;
            
            if (isEventHitRunning == false && !isDead)
            {
                StartCoroutine(EventHit());
            }
            
            var bounds = GetComponent<Collider2D>().bounds;
            var damageEffectSpawnPosition = bounds.center + new Vector3(0.0f, bounds.extents.y + 1f, 0.0f);

            var criticalCheck = isCritical switch
            {
                true => Enums.PoolType.EffectDamageCritical,
                false => Enums.PoolType.EffectDamageNormal
            };
            
            EffectManager.Instance.CreateEffectsAtPosition(FunctionManager.Vector3ToVector2(damageEffectSpawnPosition), adjustDamage.ChangeMoney(), criticalCheck);
            
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