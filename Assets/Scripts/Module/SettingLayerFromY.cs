using UnityEngine;

namespace Module
{
    public class SettingLayerFromY : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
        }
    }
}