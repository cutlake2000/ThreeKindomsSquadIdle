using Controller;
using Controller.Effects;
using Module;
using UnityEngine;
using Enum = Data.Enum;

namespace Managers
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
        
        public void CreateParticlesAtPosition(Vector3 position, Enum.SquadClassType squadClassType, bool destroy)
        {
            ParticleSystem effectParticle = null;
            int index;

            if (destroy)
            {
                switch (squadClassType)
                {
                    case Enum.SquadClassType.Warrior:
                        index = Random.Range(0, (warriorProjectileEffectOnDestroy.Length));
                        effectParticle = warriorProjectileEffectOnDestroy[index];
                        break;
                    case Enum.SquadClassType.Archer:
                        index = Random.Range(0, (archerProjectileEffectOnDestroy.Length));
                        effectParticle = archerProjectileEffectOnDestroy[index];
                        break;
                    case Enum.SquadClassType.Wizard:
                        index = Random.Range(0, (wizardProjectileEffectOnDestroy.Length));
                        effectParticle = wizardProjectileEffectOnDestroy[index];
                        break;
                }
            }
            else
            {
                switch (squadClassType)
                {
                    // case Enum.CharacterClass.Warrior:
                    //     index = Random.Range(0, (warriorProjectileEffectOnDestroy.Length));
                    //     effectParticle = warriorProjectileEffectOnShoot[index];
                    //     break;
                    case Enum.SquadClassType.Archer:
                        index = Random.Range(0, (archerProjectileEffectOnDestroy.Length));
                        effectParticle = archerProjectileEffectOnShoot[index];
                        break;
                    // case Enum.CharacterClass.Wizard:
                    //     index = Random.Range(0, (wizardProjectileEffectOnDestroy.Length));
                    //     effectParticle = wizardProjectileEffectOnDestroy[index];
                    //     break;
                }
            }


            if (effectParticle == null) return;

            effectParticle.transform.position = position;
            effectParticle.Play(true);
        }
        
        //TODO: 추후에 인자로 BIGINTEGER 타입의 데미지를 전해줘야 한다
        public void CreateEffectsAtPosition(Vector2 startPosition, string damage, Enum.PoolType poolType)
        {
            var obj = objectPool.SpawnFromPool(poolType);
            obj.transform.position = startPosition;

            switch (poolType)
            {
                case Enum.PoolType.EffectDamage:
                    var effectDamageController = obj.GetComponent<EffectDamageController>();
                    effectDamageController.InitializeEffectDamage($"{damage}");
                    break;
            }

            obj.SetActive(true);
        }
    }
}