using UnityEngine;

namespace Module
{
    public class IdGenerator : MonoBehaviour
    {
        public string GenerateRandomId()
        {
            // 유저ID 생성
            var guid = System.Guid.NewGuid().ToString();
            var cleanedGuid = guid.Replace("-", "").ToLower();

            var desiredLength = 8;
            var randomId = cleanedGuid.Substring(0, Mathf.Min(cleanedGuid.Length, desiredLength));
        
            Debug.Log("Random ID: " + randomId);

            return "Userid_" + randomId;
        }
    }
}