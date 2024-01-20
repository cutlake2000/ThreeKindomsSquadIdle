using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI
{
    public class SquadUI : MonoBehaviour
    {
        public static SquadUI Instance;

        [Header("=== 스쿼드 정보 패널 ===")]
        [SerializeField] private TMP_Text squadLevelText;
        [SerializeField] private TMP_Text squadStatPointText;
        [SerializeField] private Slider squadExpSlider;
        [SerializeField] private TMP_Text squadExpText;
        [SerializeField] private Button levelUpButton;

        [Header("=== 스탯 증가 배율 버튼")]
        [SerializeField] private Button LevelUpX1Button;
        [SerializeField] private Button LevelUpX10Button;
        [SerializeField] private Button LevelUpX100Button;
        private void Awake()
        {
            Instance = this;
        }
    }
}