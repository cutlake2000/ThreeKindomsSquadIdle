using Controller.Projectiles.BaseAttack;
using Controller.Projectiles.SkillAttack;
using Data;
using Function;
using Module;
using UnityEngine;

namespace Managers.BattleManager
{
    public class ProjectileManager : MonoBehaviour
    {
        public static ProjectileManager Instance;
        private ObjectPool objectPool;

        private void Awake()
        {
            Instance = this;
        }

        protected void Start()
        {
            objectPool = GetComponent<ObjectPool>();
        }

        public void InstantiateBaseAttack(BigInteger damage, Vector2 startPosition, Vector2 direction,
            Enums.PoolType poolType)
        {
            var obj = objectPool.SpawnFromPool(poolType);
            obj.transform.position = startPosition;

            switch (poolType)
            {
                case Enums.PoolType.ProjectileBaseAttackWarrior:
                    var warriorBaseAttackController = obj.GetComponent<ProjectileBaseAttackWarriorController>();
                    warriorBaseAttackController.InitializeWarriorBaseAttack(damage, direction);
                    break;
                case Enums.PoolType.ProjectileBaseAttackArcher:
                    var archerBaseAttackController = obj.GetComponent<ProjectileBaseAttackArcherController>();
                    archerBaseAttackController.InitializeArcherBaseAttack(damage, direction);
                    break;
                case Enums.PoolType.ProjectileBaseAttackWizard:
                    var wizardBaseAttackController = obj.GetComponent<ProjectileBaseAttackWizardController>();
                    wizardBaseAttackController.InitializeWizardBaseAttack(damage, direction);
                    break;
                case Enums.PoolType.ProjectileBaseAttackMonster:
                    var monsterBaseAttackController = obj.GetComponent<ProjectileBaseAttackMonsterController>();
                    monsterBaseAttackController.InitializeMonsterBaseAttack(damage, direction);
                    break;
            }

            obj.SetActive(true);
        }

        public void InstantiateSkillAttack(GameObject targetSkill, BigInteger damage, Vector2 startPosition,
            Vector2 targetPosition)
        {
            var skillAttackController = targetSkill.GetComponent<SkillAttackController>();
            skillAttackController.InitializeSkillAttack(damage, startPosition, targetPosition);
        }
    }
}