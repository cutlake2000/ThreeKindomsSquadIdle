using System;
using System.Collections;
using System.Collections.Generic;
using Creature.Data;
using Data;
using Function;
using Keiwando.BigInteger;
using Module;
using UnityEngine;
using UnityEngine.UI;

namespace Creature.CreatureClass
{
    public class Creature : MonoBehaviour
    {
        private const float FADE_TIME = 0.8f;
        [SerializeField] protected Enums.CreatureClassType creatureClassType;

        [Header("EnemyFinder")] [SerializeField]
        protected float followRange;

        public Transform currentTarget;
        public Transform projectileSpawn;
        public Slider healthBar;
        public Animator animator;
        public AnimationData animationData;
        public AnimationEventReceiver animationEventReceiver;
        public Rigidbody2D rigid;

        public bool isDead;
        public BigInteger damage;
        public BigInteger currentHealth;
        public BigInteger maxHealth;
        public BigInteger defence;
        // public BigInteger criticalRate;
        // public BigInteger criticalDamage;
        public float attackSpeed;
        public float attackRange;
        public float moveSpeed;

        [Header("Sprite")] [SerializeField] public List<SpriteRenderer> allSprites = new();
        [Header("HpBar")] [SerializeField] protected GameObject hpBar;

        protected TargetFinder TargetFinder;

        protected virtual void Update()
        {
            FlipHpBar();
        }

        protected virtual void OnEnable()
        {
            SetCreatureComponent();
            AddEventListener();
            SetAllSpritesList();
            InitCreature();
        }

        protected void OnDisable()
        {
            SubtractEventListener();
        }

        protected virtual void AddEventListener()
        {
        }

        protected virtual void SubtractEventListener()
        {
            
        }

        public virtual void SetAllSpritesList()
        {
        }

        protected virtual void ResetAllSpritesList()
        {
        }

        private void InitCreature()
        {
            hpBar.gameObject.SetActive(true);
            gameObject.GetComponent<Collider2D>().enabled = true;
            ResetAllSpritesList();
            SetCreatureStats();
            SetUIHealthBar();
        }

        protected virtual void SetCreatureStats()
        {
        }

        private void SetCreatureComponent()
        {
            TargetFinder = GetComponent<TargetFinder>();
            rigid = GetComponent<Rigidbody2D>();
            healthBar.maxValue = 100;

            animationData.InitializeData();
        }

        protected virtual void FindNearbyEnemy()
        {
        }

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

            while (percent < 1.0f)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / FADE_TIME;

                foreach (var item in allSprites)
                {
                    var color = item.color;
                    color.a = Mathf.Lerp(start, end, percent);
                    item.color = color;
                }

                yield return null;
            }

            CreatureDeath();
            gameObject.SetActive(false);
        }

        protected virtual void CreatureDeath()
        {
        }
    }
}