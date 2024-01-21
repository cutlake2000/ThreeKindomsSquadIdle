using System;
using System.Collections;
using System.Collections.Generic;
using Creature.Data;
using Function;
using Managers;
using Module;
using UnityEngine;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Creature.CreatureClass
{
    public class Creature : MonoBehaviour
    {
        private const float FadeTime = 2f;
        [SerializeField] protected Enum.CreatureClassType creatureClassType;

        [Header("EnemyFinder")]
        [SerializeField] protected float followRange;

        public Transform currentTarget;
        public Transform projectileSpawn;
        public Slider healthBar;
        public Animator animator;
        public AnimationData animationData;
        public AnimationEventReceiver animationEventReceiver;
        public Rigidbody2D rigid;

        public bool isDead;
        public BigInteger attack;
        public BigInteger currentHealth;
        public BigInteger maxHealth;
        public BigInteger defence;
        public float attackSpeed;
        public float attackRange;
        public float moveSpeed;

        [Header("Sprite")]
        [SerializeField] protected List<SpriteRenderer> allSprites = new();

        protected EnemyFinder enemyFinder;

        protected virtual void Awake()
        {
            SetCreatureComponent();
            SetEventListener();
            SetAllSpritesList();
        }

        protected virtual void OnEnable()
        {
            InitCreature();
        }

        protected virtual void Update()
        {
            FlipHpBar();
        }

        protected virtual void SetEventListener() { }
        
        protected virtual void SetAllSpritesList() { }
        
        protected virtual void ResetAllSpritesList() { }
        
        public void InitCreature()
        {
            gameObject.GetComponent<Collider2D>().enabled = true;
            ResetAllSpritesList();
            SetCreatureStats();
            SetUIHealthBar();
        }

        protected void OnDisable()
        {
            SetCreatureState();
        }

        protected virtual void SetCreatureState() { }

        protected virtual void SetCreatureStats() { }
        
        private void SetCreatureComponent()
        {
            animator = GetComponentInChildren<Animator>();
            animationEventReceiver = GetComponentInChildren<AnimationEventReceiver>();
            enemyFinder = GetComponent<EnemyFinder>();
            rigid = GetComponent<Rigidbody2D>();
            healthBar.maxValue = 100;

            animationData.InitializeData();
        }

        protected virtual void FindNearbyEnemy() { }

        protected void FlipHpBar()
        {
            Transform healthBarTransform;
            Vector3 localScale;

            switch (transform.localScale.x)
            {
                case < 0:
                    healthBarTransform = healthBar.transform;
                    localScale = healthBarTransform.localScale;
                    localScale = new Vector3(-Mathf.Abs(localScale.x), Mathf.Abs(localScale.y),
                        Mathf.Abs(localScale.z));

                    healthBarTransform.localScale = localScale;
                    break;
                case > 0:
                    healthBarTransform = healthBar.transform;
                    localScale = healthBarTransform.localScale;
                    localScale = new Vector3(Mathf.Abs(localScale.x), Mathf.Abs(localScale.y), Mathf.Abs(localScale.z));

                    healthBarTransform.localScale = localScale;
                    break;
            }
        }

        public void SetUIHealthBar()
        {
            var sliderValue = currentHealth * 100 / maxHealth;
            healthBar.value = int.Parse(sliderValue.ToString());
        }

        public IEnumerator Fade(float start, float end)
        {
            var currentTime = 0.0f;
            var percent = 0.0f;

            while (percent < 1)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / FadeTime;

                foreach (var item in allSprites)
                {
                    var color = item.color;
                    color.a = Mathf.Lerp(start, end, percent);
                    item.color = color;
                }

                yield return null;
            }

            if (creatureClassType == Enum.CreatureClassType.Squad)
            {
                StageManager.CheckRemainedSquad?.Invoke();   
            }
            gameObject.SetActive(false);
        }
    }
}