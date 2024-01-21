using System.Collections;
using System.Collections.Generic;
using Creature.CreatureClass.MonsterFSM;
using Creature.CreatureClass.SquadClass;
using Function;
using Managers;
using UnityEngine;
using Enum = Data.Enum;

namespace Creature.CreatureClass.MonsterClass
{
    public class Monster : Creature
    {
        [Header("Class")] [SerializeField] public Enum.MonsterClassType monsterClassType;

        [SerializeField] private Vector2 direction;

        [Header("Projectile")] [SerializeField]
        private Vector2 projectileSpawnPosition;

        [Header("StateMachine")] private MonsterStateMachine monsterStateMachine;

        [Header("Sprites")] [SerializeField] private List<Sprite> monsterSprites;

        [Header("HitEffects")] [SerializeField]
        private bool isEventHitRunning;
        
        [Header("Detector")]
        public GameObject detector;

        public ParticleSystem effectHit;
        private readonly WaitForSeconds hitEffectDelay = new(0.2f);
        private float currentExplorationTime = 0.0f; // 위치 업데이트 간격
        private float maxExplorationTime = 2.0f; // 위치 업데이트 간격

        protected override void OnEnable()
        {
            base.OnEnable();

            monsterStateMachine?.ChangeState(monsterStateMachine.MonsterIdleState);
        }

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

            if (currentTarget != null && currentTarget.GetComponent<Squad>().isDead == false) return;
            FindNearbyEnemy();
        }

        private void FixedUpdate()
        {
            monsterStateMachine.PhysicsUpdate();
        }

        protected override void SetEventListener()
        {
            animationEventReceiver.OnNormalAttackEffect += OnNormalAttack;
        }

        protected override void SetAllSpritesList()
        {
            foreach (var allSprite in allSprites) monsterSprites.Add(allSprite.sprite);
        }

        private void ResetAllSprites()
        {
            for (var i = 0; i < allSprites.Count; i++)
            {
                allSprites[i].sprite = monsterSprites[i];

                if (!allSprites[i].gameObject.activeSelf) allSprites[i].gameObject.SetActive(true);
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

            isDead = false;
            isEventHitRunning = false;
            currentTarget = null;
        }

        public bool TakeDamage(BigInteger damage)
        {
            currentHealth -= damage;
            currentHealth = currentHealth < 0 ? 0 : currentHealth;
            SetUIHealthBar();

            effectHit.Play();

            var bounds = GetComponent<Collider2D>().bounds;
            var damageEffectSpawnPosition = bounds.center + new Vector3(0.0f, bounds.extents.y + 1f, 0.0f);

            EffectManager.Instance.CreateEffectsAtPosition(FunctionManager.Vector3ToVector2(damageEffectSpawnPosition),
                damage.ToString(), Enum.PoolType.EffectDamage);

            if (isEventHitRunning == false && !isDead) StartCoroutine(EventHit());

            if (currentHealth > 0 || isDead) return true;
            isDead = true;
            monsterStateMachine.ChangeState(monsterStateMachine.MonsterDieState);

            ResetAllSprites();
            StageManager.CheckRemainedMonster?.Invoke();
            MonsterManager.Instance.ReturnMonster(monsterClassType, this);
            return false;
        }

        protected override void FindNearbyEnemy()
        {
            //TODO: 추후에 스테이지가 시작할 때 로직이 돌도록 수정하면 좋을 듯
            if (currentTarget != null && currentTarget.GetComponent<Squad>().isDead == false) return;

            currentTarget = enemyFinder.ScanNearestEnemy(followRange);
        }

        private void OnNormalAttack()
        {
            if (currentTarget == null) return;

            projectileSpawnPosition = FunctionManager.Vector3ToVector2(projectileSpawn.position);
            direction = (currentTarget.transform.position - projectileSpawn.transform.position).normalized;

            ProjectileManager.Instance.InstantiateBaseAttack(attack, projectileSpawnPosition, direction,
                Enum.PoolType.ProjectileBaseAttackWarrior);
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