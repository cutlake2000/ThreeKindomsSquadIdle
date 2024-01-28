using Creature.CreatureClass.MonsterClass;
using Creature.CreatureClass.SquadFSM;
using Function;
using Managers;
using Module;
using UnityEngine;
using UnityEngine.Serialization;
using Enum = Data.Enum;

namespace Creature.CreatureClass.SquadClass
{
    public class Squad : Creature
    {
        [Header("Class")]
        [SerializeField] public Enum.CharacterType characterType;

        [Header("Sprite")]
        [SerializeField] public SPUM_SpriteList spumSprite;
        
        [Header("Projectile")]
        protected Vector2 ProjectileSpawnPosition;
        protected Vector2 Direction;

        [Header("StateMachine")]
        private SquadStateMachine squadStateMachine;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            squadStateMachine?.ChangeState(squadStateMachine.SquadIdleState);
        }
        
        protected virtual void Start()
        {
            squadStateMachine = new SquadStateMachine(this);
            squadStateMachine.ChangeState(squadStateMachine.SquadIdleState);
        }

        protected override void Update()
        {
            base.Update();
            
            squadStateMachine.Update();
            squadStateMachine.LogicUpdate();
            
            if (currentTarget != null && currentTarget.gameObject.activeInHierarchy && currentTarget.GetComponent<Monster>().isDead == false) return;
            FindNearbyEnemy();
        }

        protected virtual void FixedUpdate()
        {
            squadStateMachine.PhysicsUpdate();
        }
        
        protected override void SetEventListener()
        {
            animationEventReceiver.OnNormalAttackEffect += OnNormalAttackEffect;
            animationEventReceiver.OnNormalAttack += OnNormalAttack;
            animationEventReceiver.OnSkillAttack += OnSkillAttack;
        }

        public override void SetAllSpritesList()
        {
            allSprites.AddRange(spumSprite._itemList);
            allSprites.AddRange(spumSprite._eyeList);
            allSprites.AddRange(spumSprite._hairList);
            allSprites.AddRange(spumSprite._bodyList);
            allSprites.AddRange(spumSprite._clothList);
            allSprites.AddRange(spumSprite._armorList);
            allSprites.AddRange(spumSprite._pantList);
            allSprites.AddRange(spumSprite._weaponList);
            allSprites.AddRange(spumSprite._backList);
        }
        
        protected override void ResetAllSpritesList()
        {
            spumSprite.ResyncData();

            foreach (var sprite in allSprites)
            {
                var color = sprite.color;
                color.a = 1;
                sprite.color = color;
            }
        }

        protected override void SetCreatureStats()
        {
            maxHealth = SquadBattleManager.Instance.GetTotalSquadStat(Enum.SquadStatType.Health);
            defence = SquadBattleManager.Instance.GetTotalSquadStat(Enum.SquadStatType.Defence);
            moveSpeed = SquadBattleManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.MoveSpeed);
            followRange = SquadBattleManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.FollowRange);
            
            currentHealth = maxHealth;
            isDead = false;
            currentTarget = null;
        }

        public void TakeDamage(BigInteger damage)
        {
            currentHealth -= damage;
            currentHealth = currentHealth < 0 ? 0 : currentHealth;
            SetUIHealthBar();

            Debug.Log($"{gameObject.name} {damage} 데미지!");

            if (currentHealth > 0) return;
            isDead = true;
            hpBar.gameObject.SetActive(false);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            StageManager.CheckRemainedSquadAction?.Invoke();
            squadStateMachine.ChangeState(squadStateMachine.SquadDieState);
        }

        protected override void CreatureDeath()
        {
            squadStateMachine.ChangeState(squadStateMachine.SquadIdleState);
        }

        protected override void FindNearbyEnemy()
        {
            currentTarget = null;
            
            //TODO: 추후에 스테이지가 시작할 때 로직이 돌도록 수정하면 좋을 듯
            if (currentTarget != null && currentTarget.GetComponent<Monster>().isDead == false) return;

            currentTarget = TargetFinder.ScanNearestEnemy(followRange);
        }

        protected virtual void OnNormalAttack()
        {
            if (currentTarget == null) return;

            ProjectileSpawnPosition = FunctionManager.Vector3ToVector2(projectileSpawn.position);
            Direction = (currentTarget.transform.position - projectileSpawn.transform.position).normalized;
        }

        protected virtual void OnNormalAttackEffect() { }

        protected virtual void OnSkillAttack()
        {
            if (currentTarget == null) return;

            ProjectileSpawnPosition = FunctionManager.Vector3ToVector2(projectileSpawn.position);
            Direction = (currentTarget.transform.position - projectileSpawn.transform.position).normalized;
        }
    }
}