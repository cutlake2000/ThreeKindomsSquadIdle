using Data;
using UnityEngine;

namespace Resources.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "SO/PushNotesData")]
    public class PushNotesDataSo : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] private string desc;
        [SerializeField] private int pushTime;
        [SerializeField] private Enums.RewardType rewardType;
        [SerializeField] private int amount;

        public string Title => title;
        public string Desc => desc;
        public int PushTime => pushTime;
        public Enums.RewardType RewardType => rewardType;
        public int Amount => amount;
    }
}
