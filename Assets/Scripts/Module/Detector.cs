using System;
using UnityEngine;

namespace Module
{
    public class Detector : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayers;
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"{other.gameObject.layer} 감지!!");
            if (other.gameObject.layer != targetLayers) return;
            Debug.Log($"{other.gameObject.layer} 감지!!");
            gameObject.transform.parent.GetComponent<Creature.CreatureClass.Creature>().currentTarget = other.transform;
            gameObject.SetActive(false);
        }
    }
}