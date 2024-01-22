using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI
{
    public class DungeonItemUI : MonoBehaviour
    {
        [SerializeField] private Button ClearDungeonButton;
        [SerializeField] private Button enterDungeonButton;
        [SerializeField] private Toggle autoChallengeButton;
        [SerializeField] private Button previousStageButton;
        [SerializeField] private Button nextStageButton;
        [SerializeField] private TMP_Text currentStageName;
        [SerializeField] private TMP_Text currentStageReward;
    }
}