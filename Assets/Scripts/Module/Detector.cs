using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Module
{
    public class Detector : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayer;
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"{other.name} | {other.gameObject.layer} || {gameObject.name} | {targetLayer.value}");
            if (other.gameObject.layer != targetLayer) return;
            Debug.Log($"{gameObject.transform.parent.GetComponent<Creature.CreatureClass.Creature>()} 타겟 감지");
            gameObject.transform.parent.GetComponent<Creature.CreatureClass.Creature>().currentTarget = other.transform;
            gameObject.SetActive(false);
        }
    }
}