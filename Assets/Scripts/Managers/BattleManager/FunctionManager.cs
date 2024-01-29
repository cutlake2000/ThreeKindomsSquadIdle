using UnityEngine;

namespace Managers.BattleManager
{
    public class FunctionManager : MonoBehaviour
    {
        public static FunctionManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public static Vector2 Vector3ToVector2(Vector3 inputVec3)
        {
            return new Vector2(inputVec3.x, inputVec3.y);
        }
    }
}