using UnityEngine;

namespace Controller.Projectiles.SkillAttack.Warrior
{
    public class PillarOfFire : SkillAttackController
    {
        [Header("Projectile 발사 여부")] [SerializeField]
        protected bool readyToLaunch;

        private void Update()
        {
            if (!readyToLaunch) return;

            particleCurrentTime += Time.deltaTime;

            switch (particleCurrentTime)
            {
                case >= 1.5f when attackColliders[0].activeInHierarchy:
                    attackColliders[0].SetActive(false);
                    break;
                case >= 0.8f when attackColliders[0].activeInHierarchy == false:
                    attackColliders[0].SetActive(true);
                    break;
            }
        }

        protected override void ActivateSkill()
        {
            particleCurrentTime = 0f;
            projectileTransform.position = targetPosition;
            readyToLaunch = true;
            gameObject.GetComponent<ParticleSystem>().Play(true);
        }
    }
}