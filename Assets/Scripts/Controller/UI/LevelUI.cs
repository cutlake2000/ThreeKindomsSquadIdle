using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
    
        [Header("[경험치 증가값]")]
        [SerializeField] private float currentExpIncreaseValue = 20f;
    
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
            Managers.SquadManager.Instance.SquadLevel.IncreaseExp(currentExpIncreaseValue);
        }
    
        public void SetUI()
        {
            currentLevelText.text = $"레벨 : {Managers.SquadManager.Instance.SquadLevel.CurrentLevel}";
            currentExpText.text = $"경험치 : {Managers.SquadManager.Instance.SquadLevel.CurrentExp} / {Managers.SquadManager.Instance.SquadLevel.MaxExp}";
            currentAttack.text = $"공격력 : {Managers.SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.Attack)}";
            currentHealth.text = $"체력 : {Managers.SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.Health)}";
            currentDefence.text = $"방어력 : {Managers.SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.Defence)}";
            currentCriticalRate.text = $"치명타 확률 : {Managers.SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.CriticalRate)}";
            currentCriticalDamage.text = $"치명타 데미지 : {Managers.SquadManager.Instance.GetTotalSquadStat(Enum.SquadStatType.CriticalDamage)}";
        }
    }
}