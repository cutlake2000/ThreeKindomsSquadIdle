// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Creature.SquadScripts.SquadClass;
// using Function;
// using Managers;
// using MonsterLove.StateMachine;
// using UnityEngine;
// using UnityEngine.Serialization;
// using UnityEngine.UI;
// using Enum = Data.Enum;
//
// namespace Enemy
// {
//     public enum MonsterState
//     {
//         Spawn,
//         Idle,
//         Run,
//         Attack,
//         Death
//     }
//
//     public class Monster : MonoBehaviour
//     {
//         private const int playerLayerMask = 1 << 6; // 플레이어 레이어 마스크
//
//         private static readonly int Hit = Animator.StringToHash("Hit");
//
//         public bool isDead;
//
//
//         [FormerlySerializedAs("myType")] [SerializeField] public Enum.MonsterClassType myClassType;
//
//         public BigInteger maxHealth = 30000;
//         public BigInteger currentHealth;
//
//         public float speed = 2.0f;
//         public float detectionRadius = 50.0f;
//         public float updateInterval = 2.0f; // 위치 업데이트 간격
//
//
//         [SerializeField] private Transform closestPlayerTransform;
//         [SerializeField] private SpriteRenderer[] monsterSpriteRenderers;
//         [SerializeField] private Sprite[] monsterSprites;
//
//         public ParticleSystem EffectHit;
//
//         public Slider healthBar;
//
//         [SerializeField] private Squad currentChar;
//         private readonly WaitForSeconds attackDelay = new(0.2f);
//
//         private Dictionary<string, float> animationLengths = new();
//
//         // ---
//         private IEnumerator attackCoroutine;
//
//         public StateMachine<MonsterState> FSM;
//         public Action<Collider2D> InAttackRangeEvent;
//
//         private bool isEventHitRunning;
//         private Animator monsterAnimator;
//         private Rigidbody2D monsterRigidbody; // 몬스터의 Rigidbody2D
//         private float updateTimer;
//
//         public Vector3 Position => transform.position;
//
//
//         private void Awake()
//         {
//             monsterRigidbody = GetComponent<Rigidbody2D>();
//             monsterAnimator = GetComponent<Animator>();
//             FSM = new StateMachine<MonsterState>(this);
//
//             InAttackRangeEvent += InAttackRange;
//         }
//
//         private void Update()
//         {
//             FSM.Driver.Update.Invoke();
//             FlipHpBar();
//         }
//
//         private void OnEnable()
//         {
//             Init();
//         }
//         
//         // StageManager.CheckRemainedMonster?.Invoke();
//         // MonsterNew.InitSprite();
//         // MonsterManager.Instance.ReturnMonster(MonsterNew.MonsterClassType, MonsterNew);
//
//         private float GetAnimationLength(string animationName)
//         {
//             return MonsterManager.Instance.GetAnimationLength(animationName);
//         }
//
//         private void Init()
//         {
//             //InitSprite();
//             isDead = false;
//             isEventHitRunning = false;
//             closestPlayerTransform = null;
//
//
//             currentHealth = maxHealth;
//             healthBar.maxValue = 100;
//             FSM.ChangeState(MonsterState.Spawn);
//
//             SetUIHealthBar();
//         }
//
//         private void InitSprite()
//         {
//             for (var i = 0; i < monsterSpriteRenderers.Length; i++)
//             {
//                 var monsterSpriteRenderer = monsterSpriteRenderers[i];
//                 if (!monsterSpriteRenderer.gameObject.activeSelf) monsterSpriteRenderer.gameObject.SetActive(true);
//                 monsterSpriteRenderer.sprite = monsterSprites[i];
//                 monsterSpriteRenderer.transform.rotation = Quaternion.identity;
//                 monsterSpriteRenderer.color = Color.white;
//             }
//         }
//
//         // ---
//         private void Spawn_Enter()
//         {
//             FindClosestPlayer();
//             FSM.ChangeState(MonsterState.Idle);
//         }
//
//         // --
//         private void Idle_Enter()
//         {
//             PlayAnimation(MonsterState.Idle);
//             closestPlayerTransform = null;
//             currentChar = null;
//             if (closestPlayerTransform != null) FSM.ChangeState(MonsterState.Run);
//         }
//
//         private void Idle_Update()
//         {
//             if (closestPlayerTransform == null)
//             {
//                 updateTimer += Time.deltaTime;
//                 if (updateTimer >= updateInterval)
//                 {
//                     updateTimer = 0.0f;
//                     FindClosestPlayer();
//                 }
//             }
//             else
//             {
//                 FSM.ChangeState(MonsterState.Run);
//             }
//         }
//
//         // ---
//
//         private void Run_Enter()
//         {
//             PlayAnimation(MonsterState.Run);
//         }
//
//         private void Run_Update()
//         {
//             MoveTowardsPlayer();
//         }
//
//         private void Attack_Enter()
//         {
//             //TODO: 임시 반창고
//             if (isDead) return;
//             //attackCoroutine = Attack();
//             StartCoroutine(Attack());
//         }
//
//         private void Attack_Exit()
//         {
//             if (isDead) return;
//             StopCoroutine(Attack());
//             PlayAnimation(MonsterState.Run);
//         }
//         // ---
//
//         private void Death_Enter()
//         {
//             // 쓰러지는 애니메이션
//
//             StageManager.CheckRemainedMonster?.Invoke();
//             InitSprite();
//             // MonsterManager.Instance.ReturnMonster(myClassType, this);
//         }
//
//
//         private void MoveTowardsPlayer()
//         {
//             // 플레이어와 몬스터 사이의 방향 계산
//             var direction = (closestPlayerTransform.position - transform.position).normalized;
//
//             // 몬스터 이동
//             monsterRigidbody.MovePosition(transform.position + direction * speed * Time.deltaTime);
//
//             // 플레이어가 몬스터의 왼쪽에 있는지 오른쪽에 있는지 판단
//             if (direction.x > 0)
//                 transform.localScale = new Vector3(-Math.Abs(transform.localScale.x), transform.localScale.y,
//                     transform.localScale.z);
//             else if (direction.x < 0)
//                 transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y,
//                     transform.localScale.z);
//         }
//
//         private void FlipHpBar()
//         {
//             Transform healthBarTransform;
//             Vector3 localScale;
//
//             switch (transform.localScale.x)
//             {
//                 case < 0:
//                     healthBarTransform = healthBar.transform;
//                     localScale = healthBarTransform.localScale;
//                     localScale = new Vector3(-Mathf.Abs(localScale.x), Mathf.Abs(localScale.y),
//                         Mathf.Abs(localScale.z));
//
//                     healthBarTransform.localScale = localScale;
//                     break;
//                 case > 0:
//                     healthBarTransform = healthBar.transform;
//                     localScale = healthBarTransform.localScale;
//                     localScale = new Vector3(Mathf.Abs(localScale.x), Mathf.Abs(localScale.y), Mathf.Abs(localScale.z));
//
//                     healthBarTransform.localScale = localScale;
//                     break;
//             }
//         }
//
//
//         private void FindClosestPlayer()
//         {
//             var hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayerMask);
//
//             var closestDistance = Mathf.Infinity;
//             Transform closestPlayer = null;
//             foreach (var hitCollider in hitColliders)
//             {
//                 if (hitCollider.GetComponent<Squad>().isDead) continue;
//                 
//                 var distance = Vector3.Distance(transform.position, hitCollider.transform.position);
//                 if (distance < closestDistance)
//                 {
//                     closestDistance = distance;
//                     closestPlayer = hitCollider.transform;
//                 }
//             }
//
//             closestPlayerTransform = closestPlayer;
//         }
//
//
//         private void InAttackRange(Collider2D target)
//         {
//             currentChar = target.GetComponent<Squad>();
//             FSM.ChangeState(MonsterState.Attack);
//         }
//
//         private IEnumerator Attack()
//         {
//             var attackDelay = new WaitForSeconds(GetAnimationLength("Attack"));
//             while (true)
//             {
//                 yield return attackDelay; // 공격 사이의 대기 시간
//                 PlayAnimation(MonsterState.Attack);
//                 // if (currentChar != null && currentChar.TakeDamage(10)) FSM.ChangeState(MonsterState.Idle);
//                 yield return attackDelay; // 공격 애니메이션 길이
//             }
//         }
//         
//         public bool TakeDamage(BigInteger damage)
//         {
//             currentHealth -= damage;
//
//             if (currentHealth < 0) currentHealth = 0;
//
//             SetUIHealthBar();
//
//             EffectHit.Play();
//             
//             var bounds = GetComponent<Collider2D>().bounds;
//             var damageEffectSpawnPosition = bounds.center + new Vector3(0.0f, bounds.extents.y + 1f, 0.0f);
//             
//             EffectManager.Instance.CreateEffectsAtPosition(FunctionManager.Vector3ToVector2(damageEffectSpawnPosition),
//                 damage.ToString(), Enum.PoolType.EffectDamage);
//             
//             if (isEventHitRunning == false && !isDead) StartCoroutine(EventHit());
//
//             if (currentHealth > 0 || isDead) return false;
//             
//             isDead = true;
//             FSM.ChangeState(MonsterState.Death);
//             
//             return true;
//         }
//
//
//         private void PlayAnimation(MonsterState key)
//         {
//             switch (key)
//             {
//                 case MonsterState.Idle:
//                     monsterAnimator.SetTrigger("Idle");
//
//                     break;
//                 case MonsterState.Run:
//                     monsterAnimator.SetTrigger("Run");
//                     break;
//                 case MonsterState.Attack:
//                     monsterAnimator.SetTrigger("Attack");
//                     break;
//                 case MonsterState.Death:
//                     monsterAnimator.SetTrigger("Death");
//                     break;
//                 default:
//                     monsterAnimator.SetTrigger("Idle");
//                     break;
//             }
//         }
//
//         private IEnumerator EventHit()
//         {
//             isEventHitRunning = true;
//
//             foreach (var spriteRenderer in monsterSpriteRenderers) spriteRenderer.color = new Color(0.83f, 0f, 0f);
//
//             yield return attackDelay;
//
//             foreach (var spriteRenderer in monsterSpriteRenderers) spriteRenderer.color = Color.white;
//
//             isEventHitRunning = false;
//         }
//
//         protected static float GetNormalizedTime(Animator animator)
//         {
//             var currentInfo = animator.GetCurrentAnimatorStateInfo(0);
//
//             return currentInfo.normalizedTime;
//         }
//
//         private void SetUIHealthBar()
//         {
//             var temp = currentHealth * 100 / maxHealth;
//             var temp2 = temp.ToString();
//
//             healthBar.value = int.Parse(temp2);
//         }
//     }
// }