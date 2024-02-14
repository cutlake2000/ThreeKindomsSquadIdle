using System.Collections;
using Data;
using Function;
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
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            maxBossHealth = MonsterManager.Instance.bossMonsterBaseStats.maxHealth * dungeonLevel;
            currentBossHealth = maxBossHealth;
            currentBossDefence = MonsterManager.Instance.bossMonsterBaseStats.defence * dungeonLevel;
        }

        public override void TakeDamage(BigInteger inputDamage)
        {
            currentBossHealth -= inputDamage * 100 / currentBossDefence + 100;
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