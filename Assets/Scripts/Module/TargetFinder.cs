using UnityEngine;

namespace Module
{
    public class TargetFinder : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayers;
        
        public Color gizmoColor;
        
        private float followRange;
        
        private Collider2D[] nearbyTargets;
        
        public Transform ScanNearestEnemy(float range)
        {
            Transform target = null;

            if (ScanNearby(range) == null) return null;
            
            var distance = Vector3.Distance(transform.position, nearbyTargets[0].transform.position);
            
            foreach (var nearbyTarget in nearbyTargets)
            {
                var compareDistance = Vector3.Distance(transform.position, nearbyTarget.transform.position);

                if ((distance < compareDistance)) continue;
                
                distance = compareDistance;
                target = nearbyTarget.transform;
            }

            return target;
        }

        public Collider2D[] ScanNearby(float range)
        {
            followRange = range;
            
            nearbyTargets = Physics2D.OverlapCircleAll(transform.position, followRange, targetLayers);

            return nearbyTargets.Length <= 0 ? null : nearbyTargets;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, followRange);
        }
    }
}