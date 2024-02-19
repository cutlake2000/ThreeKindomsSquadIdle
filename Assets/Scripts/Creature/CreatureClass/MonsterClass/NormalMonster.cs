using System.Collections;
using System.Collections.Generic;
using Creature.CreatureClass.MonsterFSM;
using Creature.CreatureClass.SquadClass;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers;
using Managers.BattleManager;
using Managers.GameManager;
using Module;
using Unity.VisualScripting;
using UnityEngine;

namespace Creature.CreatureClass.MonsterClass
{
    public class NormalMonster : Monster
    {
        [Header("Class")] [SerializeField] public Enums.MonsterClassType monsterClassType;

        [Header("Sprites")] [SerializeField] private List<Sprite> monsterSprites;

        [Header("HitEffects")] [SerializeField]
        private bool isEventHitRunning;

        public ParticleSystem effectHit;

        private readonly WaitForSeconds hitEffectDelay = new(0.2f);
        private Vector2 direction;

        [Header("StateMachine")] private MonsterStateMachine monsterStateMachine;

        [Header("Projectile")] private Vector2 projectileSpawnPosition;
        [Header("MultiplierValue")] public int multiplierValue;
        
        protected void Start()
        {
            monsterStateMachine = new MonsterStateMachine(this);
            monsterStateMachine.ChangeState(monsterStateMachine.MonsterIdleState);
        }

        protected override void Update()
        {
            base.Update();

            monsterStateMachine.Update();
            monsterStateMachine.LogicUpdate();

            // if (currentTarget != null && currentTarget.gameObject.activeInHierarchy && currentTarget.GetComponent<Squad>().isDead == false) return;
            FindNearbyEnemy();
        }

        private void FixedUpdate()
        {
            monsterStateMachine.PhysicsUpdate();
        }

        protected override void OnEnable()
        {
            animator = GetComponentInChildren<Animator>();
            animationEventReceiver = GetComponentInChildren<AnimationEventReceiver>();
            
            base.OnEnable();
            
            monsterStateMachine?.ChangeState(monsterStateMachine.MonsterIdleState);
        }

        protected override void AddEventListener()
        {
            animationEventReceiver.OnNormalAttack += OnNormalAttack;
        }

        public override void SetAllSpritesList()
        {
            foreach (var allSprite in allSprites) monsterSprites.Add(allSprite.sprite);
        }

        private void HideSpritesExceptBody()
        {
            for (var i = 1; i < allSprites.Count; i++) allSprites[i].gameObject.SetActive(false);
        }

        private void ResetAllSprites()
        {
            for (var i = 0; i < allSprites.Count; i++)
            {
                allSprites[i].sprite = monsterSprites[i];

                if (!allSprites[i].gameObject.activeInHierarchy) allSprites[i].gameObject.SetActive(true);
                allSprites[i].transform.rotation = Quaternion.identity;

                allSprites[i].color = Color.white;
            }
        }

        protected override void SetCreatureStats()
        {
            var increaseValue = MonsterManager.Instance.increaseMonsterStatValue;

            if (multiplierValue == 0) multiplierValue = 1;
            
            MaxHealth = MonsterManager.Instance.normalMonsterBaseStats.maxHealth * increaseValue / 100 * multiplierValue;
            CurrentHealth = MaxHealth;
            Defence = MonsterManager.Instance.normalMonsterBaseStats.defence * increaseValue / 100 * multiplierValue;
            Attack = MonsterManager.Instance.normalMonsterBaseStats.damage * increaseValue / 100 * multiplierValue;
            
            moveSpeed = MonsterManager.Instance.normalMonsterBaseStats.moveSpeed;
            followRange = MonsterManager.Instance.normalMonsterBaseStats.followRange;
            
            isDead = false;
            isEventHitRunning = false;
            currentTarget = null;
        }

        public override void TakeDamage(BigInteger inputDamage)
        {
            var randomDamage = Random.Range(-MonsterManager.Instance.totalAttackAdjustValue, MonsterManager.Instance.totalAttackAdjustValue + 1) + 100;
            var damageReductionPercentage = MonsterManager.Instance.damageReduction;
            var reduction = 100 * inputDamage / (inputDamage + Defence + damageReductionPercentage);
            var adjustDamage = inputDamage * (randomDamage + reduction) / 100;
            
            CurrentHealth -= adjustDamage;
            
            CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;
            SetUIHealthBar();

            effectHit.Play();

            var bounds = GetComponent<Collider2D>().bounds;
            var damageEffectSpawnPosition = bounds.center + new Vector3(0.0f, bounds.extents.y + 1f, 0.0f);

            EffectManager.Instance.CreateEffectsAtPosition(FunctionManager.Vector3ToVector2(damageEffectSpawnPosition),
                adjustDamage.ChangeMoney(), Enums.PoolType.EffectDamage);

            if (isEventHitRunning == false && !isDead) StartCoroutine(EventHit());

            if (CurrentHealth > 0 || isDead) return;
            isDead = true;
            hpBar.gameObject.SetActive(false);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            StageManager.CheckRemainedMonsterAction?.Invoke();
            HideSpritesExceptBody();
            monsterStateMachine.ChangeState(monsterStateMachine.MonsterDieState);
        }

        protected override void CreatureDeath()
        {
            ResetAllSprites();
            monsterStateMachine.ChangeState(monsterStateMachine.MonsterIdleState);
            MonsterManager.Instance.ReturnMonster(monsterClassType, this);
        }

        protected override void FindNearbyEnemy()
        {
            currentTarget = null;
            if (currentTarget != null && currentTarget.GetComponent<Squad>().isDead == false) return;

            currentTarget = TargetFinder.ScanNearestEnemy(followRange);
        }

        private void OnNormalAttack()
        {
            if (currentTarget == null) return;

            projectileSpawnPosition = FunctionManager.Vector3ToVector2(projectileSpawn.position);
            direction = (currentTarget.transform.position - projectileSpawn.transform.position).normalized;

            ProjectileManager.Instance.InstantiateBaseAttack(Attack, projectileSpawnPosition, direction,
                Enums.PoolType.ProjectileBaseAttackMonster, false);
        }

        private IEnumerator EventHit()
        {
            isEventHitRunning = true;

            foreach (var spriteRenderer in allSprites) spriteRenderer.color = new Color(0.83f, 0f, 0f);

            yield return hitEffectDelay;

            foreach (var spriteRenderer in allSprites) spriteRenderer.color = Color.white;

            isEventHitRunning = false;
        }
    }
}