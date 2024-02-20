using Controller.Effects;
using Data;
using Module;
using UnityEngine;

namespace Managers.GameManager
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;

        [SerializeField] protected ParticleSystem[] warriorProjectileEffectOnShoot;
        [SerializeField] protected ParticleSystem[] archerProjectileEffectOnShoot;
        [SerializeField] protected ParticleSystem[] wizardProjectileEffectOnShoot;
        [SerializeField] protected ParticleSystem[] warriorProjectileEffectOnDestroy;
        [SerializeField] protected ParticleSystem[] archerProjectileEffectOnDestroy;
        [SerializeField] protected ParticleSystem[] wizardProjectileEffectOnDestroy;

        private ObjectPool objectPool;

        private void Awake()
        {
            Instance = this;
        }

        protected void Start()
        {
            objectPool = GetComponent<ObjectPool>();
        }

        public void CreateParticlesAtPosition(Vector3 position, Enums.CharacterType characterType, bool destroy)
        {
            ParticleSystem effectParticle = null;
            int index;

            if (destroy)
                switch (characterType)
                {
                    case Enums.CharacterType.Warrior:
                        index = Random.Range(0, warriorProjectileEffectOnDestroy.Length);
                        effectParticle = warriorProjectileEffectOnDestroy[index];
                        break;
                    case Enums.CharacterType.Archer:
                        index = Random.Range(0, archerProjectileEffectOnDestroy.Length);
                        effectParticle = archerProjectileEffectOnDestroy[index];
                        break;
                    case Enums.CharacterType.Wizard:
                        index = Random.Range(0, wizardProjectileEffectOnDestroy.Length);
                        effectParticle = wizardProjectileEffectOnDestroy[index];
                        break;
                }
            else
                switch (characterType)
                {
                    // case Enum.CharacterClass.Warrior:
                    //     index = Random.Range(0, (warriorProjectileEffectOnDestroy.Length));
                    //     effectParticle = warriorProjectileEffectOnShoot[index];
                    //     break;
                    case Enums.CharacterType.Archer:
                        index = Random.Range(0, archerProjectileEffectOnDestroy.Length);
                        effectParticle = archerProjectileEffectOnShoot[index];
                        break;
                    // case Enum.CharacterClass.Wizard:
                    //     index = Random.Range(0, (wizardProjectileEffectOnDestroy.Length));
                    //     effectParticle = wizardProjectileEffectOnDestroy[index];
                    //     break;
                }


            if (effectParticle == null) return;

            effectParticle.transform.position = position;
            effectParticle.Play(true);
        }

        //TODO: 추후에 인자로 BIGINTEGER 타입의 데미지를 전해줘야 한다
        public void CreateEffectsAtPosition(Vector2 startPosition, string damage, Enums.PoolType poolType)
        {
            var obj = objectPool.SpawnFromPool(poolType);
            obj.transform.position = startPosition;

            EffectDamageController effectDamageController;
            
            switch (poolType)
            {
                case Enums.PoolType.EffectDamageNormal:
                    effectDamageController = obj.GetComponent<EffectDamageController>();
                    effectDamageController.InitializeEffectDamage($"{damage}");
                    break;
                case Enums.PoolType.EffectDamageCritical:
                    effectDamageController = obj.GetComponent<EffectDamageController>();
                    effectDamageController.InitializeEffectDamage($"{damage}");
                    break;
            }

            obj.SetActive(true);
        }
    }
}