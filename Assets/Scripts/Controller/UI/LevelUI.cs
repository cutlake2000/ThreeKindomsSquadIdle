using Data;
using Managers;
using Managers.BattleManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI
{
    public class LevelUI : MonoBehaviour
    {
        public static LevelUI Instance;

        [SerializeField] private TMP_Text currentLevelText;
        [SerializeField] private TMP_Text currentExpText;
        [SerializeField] private TMP_Text currentAttack;
        [SerializeField] private TMP_Text currentHealth;
        [SerializeField] private TMP_Text currentDefence;
        [SerializeField] private TMP_Text currentCriticalRate;
        [SerializeField] private TMP_Text currentCriticalDamage;

        [Header("[경험치 증가값]")] [SerializeField] private float currentExpIncreaseValue = 20f;

        [SerializeField] private Button increaseExp;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializeButtonListeners();
        }

        private void InitializeButtonListeners()
        {
            increaseExp.onClick.AddListener(OnClickIncreaseExp);
        }

        private void OnClickIncreaseExp()
        {
            SquadBattleManager.Instance.SquadLevel.IncreaseExp(currentExpIncreaseValue);
        }

        public void SetUI()
        {
            currentLevelText.text = $"레벨 : {SquadBattleManager.Instance.SquadLevel.CurrentLevel}";
            currentExpText.text =
                $"경험치 : {SquadBattleManager.Instance.SquadLevel.CurrentExp} / {SquadBattleManager.Instance.SquadLevel.MaxExp}";
            currentAttack.text = $"공격력 : {SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.Attack)}";
            currentHealth.text = $"체력 : {SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.Health)}";
            currentDefence.text = $"방어력 : {SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.Defence)}";
            currentCriticalRate.text =
                $"치명타 확률 : {SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.CriticalRate)}";
            currentCriticalDamage.text =
                $"치명타 데미지 : {SquadBattleManager.Instance.GetTotalSquadStat(Enums.SquadStatType.CriticalDamage)}";
        }
    }
}