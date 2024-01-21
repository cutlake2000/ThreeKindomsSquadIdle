using Controller.CharacterProjectiles;
using Controller.CharacterProjectiles.BaseAttack;
using Controller.Projectiles.BaseAttack;
using Controller.Projectiles.SkillAttack;
using Data;
using Function;
using Module;
using UnityEngine;

namespace Managers
{
    public class ProjectileManager : MonoBehaviour
    {
        public static ProjectileManager Instance;
        private ObjectPool objectPool;

        [SerializeField] private GameObject[] WarriorSkillProjectiles;
        [SerializeField] private GameObject[] ArcherSkillProjectiles;
        [SerializeField] private GameObject[] WizardSkillProjectiles;
        
        private void Awake()
        {
            Instance = this;
        }
        
        protected void Start()
        {
            objectPool = GetComponent<ObjectPool>();
        }
        
        public void InstantiateBaseAttack(BigInteger damage, Vector2 startPosition, Vector2 direction, Enum.PoolType poolType)
        {
            var obj = objectPool.SpawnFromPool(poolType);
            obj.transform.position = startPosition;

            switch (poolType)
            {
                case Enum.PoolType.ProjectileBaseAttackWarrior:
                    var warriorBaseAttackController = obj.GetComponent<ProjectileBaseAttackWarriorController>();
                    warriorBaseAttackController.InitializeWarriorBaseAttack(damage, direction);
                    break;
                case Enum.PoolType.ProjectileBaseAttackArcher:
                    var archerBaseAttackController = obj.GetComponent<ProjectileBaseAttackArcherController>();
                    archerBaseAttackController.InitializeArcherBaseAttack(damage, direction);
                    break;
                case Enum.PoolType.ProjectileBaseAttackWizard:
                    var wizardBaseAttackController = obj.GetComponent<ProjectileBaseAttackWizardController>();
                    wizardBaseAttackController.InitializeWizardBaseAttack(damage, direction);
                    break;
                case Enum.PoolType.ProjectileBaseAttackMonster:
                    var monsterBaseAttackController = obj.GetComponent<ProjectileBaseAttackMonsterController>();
                    monsterBaseAttackController.InitializeMonsterBaseAttack(damage, direction);
                    break;
            }

            obj.SetActive(true);
        }
        
        public void InstantiateSkillAttack(BigInteger damage, Vector2 startPosition, Vector2 direction, Enum.PoolType poolType, int skillIndex)
        {
            GameObject obj = null;
            
            switch (poolType)
            {
                case Enum.PoolType.ProjectileSkillAttackWarrior:
                    obj = WarriorSkillProjectiles[skillIndex];
                    var warriorSkillAttackController = obj.GetComponent<ProjectileSkillAttackController>();
                    warriorSkillAttackController.InitializeBaseAttack(damage, direction);
                    break;
                case Enum.PoolType.ProjectileSkillAttackArcher:
                    obj = ArcherSkillProjectiles[skillIndex];
                    var archerSkillAttackController = obj.GetComponent<ProjectileSkillAttackController>();
                    archerSkillAttackController.InitializeBaseAttack(damage, direction);
                    break;
                case Enum.PoolType.ProjectileSkillAttackWizard:
                    obj = WizardSkillProjectiles[skillIndex];
                    var wizardSkillAttackController = obj.GetComponent<ProjectileSkillAttackController>();
                    wizardSkillAttackController.InitializeBaseAttack(damage, direction);
                    break;
            }

            if (obj == null) return;
            
            obj.transform.position = startPosition;
            obj.SetActive(true);
        }
    }
}