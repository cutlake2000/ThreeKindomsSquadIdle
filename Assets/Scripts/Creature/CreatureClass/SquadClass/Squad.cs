using System;
using Creature.CreatureClass.MonsterClass;
using Creature.CreatureClass.SquadFSM;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers;
using Managers.BattleManager;
using Managers.GameManager;
using Module;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creature.CreatureClass.SquadClass
{
    public class Squad : Creature
    {
        [Header("Class")] [SerializeField] public Enums.CharacterType characterType;
        [Header("Sprite")] [SerializeField] public SPUM_SpriteList spumSprite;
        [Header("Model")] public GameObject characterModel;
        [Header("Projectile")] protected Vector2 ProjectileSpawnPosition;
        [Header("StateMachine")] private SquadStateMachine squadStateMachine;
        
        protected Vector2 Direction;

        protected BigInteger attack;
        protected BigInteger criticalRate;
        protected BigInteger criticalDamage;
        protected BigInteger penetration;
        protected BigInteger accuracy;

        protected bool isCriticalAttack;

        protected override void OnEnable()
        {
            animator = characterModel.GetComponentInChildren<Animator>();
            animationEventReceiver = characterModel.GetComponentInChildren<AnimationEventReceiver>();
            
            base.OnEnable();
            
            squadStateMachine = new SquadStateMachine(this);
            squadStateMachine.ChangeState(squadStateMachine.SquadIdleState);
        }

        protected override void Update()
        {
            base.Update();

            squadStateMachine.Update();
            squadStateMachine.LogicUpdate();

            if (currentTarget != null && currentTarget.gameObject.activeInHierarchy &&
                currentTarget.GetComponent<Monster>().isDead == false) return;
            FindNearbyEnemy();
        }

        protected virtual void FixedUpdate()
        {
            squadStateMachine.PhysicsUpdate();
        }
        
        protected override void AddEventListener()
        {
            animationEventReceiver.OnNormalAttackEffect += OnNormalAttackEffect;
            animationEventReceiver.OnNormalAttack += OnNormalAttack;
            animationEventReceiver.OnSkillAttack += OnSkillAttack;
        }

        protected override void SubtractEventListener()
        {
            animationEventReceiver.OnNormalAttackEffect -= OnNormalAttackEffect;
            animationEventReceiver.OnNormalAttack -= OnNormalAttack;
            animationEventReceiver.OnSkillAttack -= OnSkillAttack;
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
            attack = characterType switch
            {
                Enums.CharacterType.Warrior => SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.WarriorAtk),
                Enums.CharacterType.Archer => SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.ArcherAtk),
                Enums.CharacterType.Wizard => SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.WizardAtk),
                _ => throw new ArgumentOutOfRangeException()
            };
            MaxHealth = SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.Health);
            Defence = SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.Defence);
            criticalRate = SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.CriticalRate);
            criticalDamage = SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.CriticalDamage);
            accuracy = SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.Accuracy);
            penetration = SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.Penetration);
            
            moveSpeed = SquadBattleManager.Instance.GetTotalSubSquadStat(Enums.SquadStatType.MoveSpeed);
            followRange = SquadBattleManager.Instance.GetTotalSubSquadStat(Enums.SquadStatType.FollowRange);

            CurrentHealth = MaxHealth;
            isDead = false;
            currentTarget = null;
        }

        public void TakeDamage(BigInteger inputDamage)
        {
            var damageReduction = characterType switch
            {
                Enums.CharacterType.Warrior => SquadBattleManager.Instance.warriorDamageReduction,
                Enums.CharacterType.Archer => SquadBattleManager.Instance.archerDamageReduction,
                Enums.CharacterType.Wizard => SquadBattleManager.Instance.wizardDamageReduction,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var randomDamage = Random.Range(-SquadBattleManager.Instance.totalAttackAdjustValue, SquadBattleManager.Instance.totalAttackAdjustValue + 1) + 100;
            var reduction = 100 * inputDamage / (inputDamage + Defence + damageReduction);
            var adjustDamage = inputDamage * (randomDamage + reduction) / 100;
            
            CurrentHealth -= adjustDamage;
            
            CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;
            SetUIHealthBar();

            // Debug.Log($"{gameObject.name} {damage} 데미지!");

            if (CurrentHealth > 0 && !isDead) return;
            isDead = true;
            hpBar.gameObject.SetActive(false);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            StageManager.CheckRemainedSquadAction?.Invoke();
            squadStateMachine.ChangeState(squadStateMachine.SquadDieState);

            if (SquadBattleManager.Instance.cameraController.currentCameraTarget != gameObject.transform) return;
            foreach (var squad in SquadBattleManager.Instance.squads)
            {
                if (!squad.GetComponent<Squad>().isDead)
                    SquadBattleManager.Instance.cameraController.SetCameraTarget(squad.transform);
            }
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
            
            var criticalRateCheck = Random.Range(0, 101);

            attack += accuracy + penetration;
            
            if (criticalRate >= criticalRateCheck)
            {
                attack = attack * criticalDamage / 100;
                isCriticalAttack = true;
            }
            else
            {
                isCriticalAttack = false;
            }
        }

        protected virtual void OnNormalAttackEffect()
        {
        }

        protected virtual void OnSkillAttack()
        {
            if (currentTarget == null) return;

            ProjectileSpawnPosition = FunctionManager.Vector3ToVector2(projectileSpawn.position);
            Direction = (currentTarget.transform.position - projectileSpawn.transform.position).normalized;
        }
    }
}