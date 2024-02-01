using System.Collections;
using System.Collections.Generic;
using Creature.CreatureClass.MonsterFSM;
using Creature.CreatureClass.SquadClass;
using Data;
using Function;
using Managers;
using Managers.BattleManager;
using UnityEngine;

namespace Creature.CreatureClass.MonsterClass
{
    public class Monster : Creature
    {
        [Header("Class")] [SerializeField] public Enums.MonsterClassType monsterClassType;

        [Header("Sprites")] [SerializeField] private List<Sprite> monsterSprites;

        [Header("HitEffects")] [SerializeField]
        private bool isEventHitRunning;

        public ParticleSystem effectHit;

        [Header("Detector")] public GameObject detector;

        private readonly WaitForSeconds hitEffectDelay = new(0.2f);
        private Vector2 direction;

        [Header("StateMachine")] private MonsterStateMachine monsterStateMachine;

        [Header("Projectile")] private Vector2 projectileSpawnPosition;

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
            base.OnEnable();

            monsterStateMachine?.ChangeState(monsterStateMachine.MonsterIdleState);
        }

        protected override void SetEventListener()
        {
            animationEventReceiver.OnNormalAttackEffect += OnNormalAttack;
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
            //TODO: 추후에 MonsterManager에서 가지고 오도록
            maxHealth = 500000;
            currentHealth = maxHealth;
            defence = 1000;
            moveSpeed = 3;
            followRange = 15;
            damage = 1000;

            currentHealth = maxHealth;
            isDead = false;
            isEventHitRunning = false;
            currentTarget = null;
        }

        public void TakeDamage(BigInteger damage)
        {
            currentHealth -= damage;
            currentHealth = currentHealth < 0 ? 0 : currentHealth;
            SetUIHealthBar();

            effectHit.Play();

            var bounds = GetComponent<Collider2D>().bounds;
            var damageEffectSpawnPosition = bounds.center + new Vector3(0.0f, bounds.extents.y + 1f, 0.0f);

            EffectManager.Instance.CreateEffectsAtPosition(FunctionManager.Vector3ToVector2(damageEffectSpawnPosition),
                damage.ToString(), Enums.PoolType.EffectDamage);

            if (isEventHitRunning == false && !isDead) StartCoroutine(EventHit());

            if (currentHealth > 0 || isDead) return;
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

            ProjectileManager.Instance.InstantiateBaseAttack(damage, projectileSpawnPosition, direction,
                Enums.PoolType.ProjectileBaseAttackMonster);
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