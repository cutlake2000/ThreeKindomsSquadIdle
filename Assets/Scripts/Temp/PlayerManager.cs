// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class PlayerManager : MonoBehaviour
// {
//     public static PlayerManager instance;
//
//     [SerializeField] Player player;
//     [SerializeField] PlayerControler playerControler;
//
//     [SerializeField] private GameObject projectilePrefab;
//     [SerializeField] private int poolSize = 10;
//
//     private Queue<GameObject> projectilePool = new Queue<GameObject>();
//
//     
//
//     private void Awake()
//     {
//         instance = this;
//
//         InitializeProjectilePool();
//     }
//
//     void InitializeProjectilePool()
//     {
//         for (int i = 0; i < poolSize; i++) 
//         {
//             GameObject projectile = Instantiate(projectilePrefab);
//             projectile.SetActive(false);
//             projectilePool.Enqueue(projectile);
//         }
//     }
//
//     public GameObject Getprojectile()
//     {
//         if (projectilePool.Count > 0)
//         {
//             GameObject projectile = projectilePool.Dequeue();
//             projectile.SetActive(true);
//             return projectile;
//         }
//         else
//         {
//             GameObject newProjectile = Instantiate(projectilePrefab);
//             return newProjectile;
//         }
//     }
//
//     public void ReturnProjectile(GameObject projectile)
//     {
//         projectile.SetActive(false);
//         projectilePool.Enqueue(projectile);
//     }
// }
