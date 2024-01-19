using Creature.CreatureClass.MonsterClass;
using Creature.CreatureClass.SquadFSM;
using Function;
using Managers;
using UnityEngine;
using Enum = Data.Enum;

namespace Creature.CreatureClass.SquadClass
{
    public class Squad : CreatureClass.Creature
    {
        [Header("Class")]
        [SerializeField] public Enum.SquadClassType squadClassType;

        [Header("Sprite")]
        [SerializeField] private SPUM_SpriteList spumSprite;

        protected Vector2 Direction;

        [Header("Projectile")]
        protected Vector2 ProjectileSpawnPosition;

        [Header("StateMachine")]
        private SquadStateMachine squadStateMachine;

        [Header("Detector")]
        public GameObject detector;

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
            
            if (currentTarget != null && currentTarget.GetComponent<MonsterNew>().isDead == false) return;
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
            animationEventReceiver.OnSkill1Attack += OnSkillAttack1;
            animationEventReceiver.OnSkill2Attack += OnSkillAttack2;
        }

        protected override void SetAllSpritesList()
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

        protected override void SetCreatureStats()
        {
            maxHealth = SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.Hp);
            defence = SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.Def);
            moveSpeed = SquadManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.MoveSpeed);
            followRange = SquadManager.Instance.GetTotalSubSquadStat(Enum.SquadStatType.FollowRange);
            
            currentHealth = maxHealth;
            isDead = false;
            currentTarget = null;
        }

        public bool TakeDamage(BigInteger damage)
        {
            currentHealth -= damage;
            currentHealth = currentHealth < 0 ? 0 : currentHealth;
            SetUIHealthBar();

            if (currentHealth > 0) return true;
            isDead = true;
            squadStateMachine.ChangeState(squadStateMachine.SquadDieState);

            return false;
        }

        protected override void FindNearbyEnemy()
        {
            //TODO: 추후에 스테이지가 시작할 때 로직이 돌도록 수정하면 좋을 듯
            if (currentTarget != null && currentTarget.GetComponent<MonsterNew>().isDead == false) return;

            currentTarget = enemyFinder.ScanNearestEnemy(followRange);
        }

        protected virtual void OnNormalAttack()
        {
            if (currentTarget == null) return;

            ProjectileSpawnPosition = FunctionManager.Vector3ToVector2(projectileSpawn.position);
            Direction = (currentTarget.transform.position - projectileSpawn.transform.position).normalized;
        }

        protected virtual void OnNormalAttackEffect() { }

        protected virtual void OnSkillAttack1()
        {
            if (currentTarget == null) return;

            ProjectileSpawnPosition = FunctionManager.Vector3ToVector2(projectileSpawn.position);
            Direction = (currentTarget.transform.position - projectileSpawn.transform.position).normalized;
        }

        private void OnSkillAttack2()
        {
            Debug.Log("내려ㅓㅓㅓㅓㅓㅓㅓㅓ찍기!");
        }
    }
}