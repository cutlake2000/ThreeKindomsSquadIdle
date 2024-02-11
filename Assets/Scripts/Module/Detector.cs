using UnityEngine;

namespace Module
{
    public class Detector : MonoBehaviour
    {
        [SerializeField] private string targetTag;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(targetTag)) return;
            // Debug.Log($"{gameObject.name}가 {other.gameObject.name} 감지");
            gameObject.transform.parent.GetComponent<Creature.CreatureClass.Creature>().currentTarget = other.transform;
            gameObject.SetActive(false);
        }
    }
}