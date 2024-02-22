using System.Collections.Generic;
using Controller.Projectiles.BaseAttack;
using Controller.Projectiles.SkillAttack;
using Data;
using Function;
using Keiwando.BigInteger;
using Module;
using UnityEngine;

namespace Managers.GameManager
{
    public class ProjectileManager : MonoBehaviour
    {
        public static ProjectileManager Instance;
        private ObjectPool objectPool;

        public Transform spawnedProjectile;
        public List<Transform> spawnedSkill;
        public Transform spawnedAttack;

        private void Awake()
        {
            Instance = this;
        }

        protected void Start()
        {
            objectPool = GetComponent<ObjectPool>();
        }

        public void InstantiateBaseAttack(BigInteger damage, Vector2 startPosition, Vector2 direction, Enums.PoolType poolType, int criticalRate, int criticalDamage)
        {
            var obj = objectPool.SpawnFromPool(poolType);

            if (obj == null) return;

            if (poolType == Enums.PoolType.ProjectileBaseAttackWarrior)
            {
                var warriorBaseAttackController = obj.GetComponent<ProjectileBaseAttackWarriorController>();
                warriorBaseAttackController.InitializeWarriorBaseAttack(damage, direction, criticalRate, criticalDamage);
            }
            else
            {
                obj.transform.position = startPosition;
                
                switch (poolType)
                {
                    case Enums.PoolType.ProjectileBaseAttackArcher:
                        var archerBaseAttackController = obj.GetComponent<ProjectileBaseAttackArcherController>();
                        archerBaseAttackController.InitializeArcherBaseAttack(damage, direction, criticalRate, criticalDamage);
                        break;
                    case Enums.PoolType.ProjectileBaseAttackWizard:
                        var wizardBaseAttackController = obj.GetComponent<ProjectileBaseAttackWizardController>();
                        wizardBaseAttackController.InitializeWizardBaseAttack(damage, direction, criticalRate, criticalDamage);
                        break;
                    case Enums.PoolType.ProjectileBaseAttackMonster:
                        var monsterBaseAttackController = obj.GetComponent<ProjectileBaseAttackMonsterController>();
                        monsterBaseAttackController.InitializeMonsterBaseAttack(damage, direction);
                        break;
                }
            }
            
            obj.SetActive(true);
        }

        public void InstantiateSkillAttack(GameObject targetSkill, BigInteger damage, Vector2 startPosition,
            Vector2 targetPosition)
        {
            targetSkill.SetActive(true);
            var skillAttackController = targetSkill.GetComponent<SkillAttackController>();
            skillAttackController.InitializeSkillAttack(damage, startPosition, targetPosition);
        }

        public void DestroyAllProjectile()
        {
            for (var i = 0; i < spawnedProjectile.transform.childCount; i++)
            {
                Destroy(spawnedProjectile.transform.GetChild(i).gameObject);
            }

            foreach (var t in spawnedSkill)
            {
                for (var j = 0; j < t.transform.childCount; j++)
                {
                    var target = t.GetChild(j).gameObject;
                    if (target.activeInHierarchy) target.SetActive(false);
                }
            }
            
            for (var i = 0; i < spawnedAttack.transform.childCount; i++)
            {
                var target = spawnedAttack.GetChild(i).gameObject;
                if (target.activeInHierarchy) target.SetActive(false);
            }
        }
    }
}