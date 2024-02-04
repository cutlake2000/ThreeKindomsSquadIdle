using System;
using System.Collections.Generic;
using Creature.Data;
using Data;
using Function;
using Managers.BottomMenuManager.SquadPanel;
using UnityEngine;

namespace Managers.BattleManager
{
    public class AccountManager : MonoBehaviour
    {
        public static AccountManager Instance;
        public event Action<Enums.CurrencyType, string> OnCurrencyChanged;
        public event Action<BigInteger> ChangedExpAction;
        public static Action LevelUpAction;
        
        [Header("레벨 초기값")] public int accountLevel;
        [Header("레벨 최대값")] public int accountMaxLevel;
        [Header("스탯 포인트")] public int statPoint;
        [Header("경험치 기본값")] [SerializeField] private int baseAccountExp;
        [Header("경험치 추가값")] [SerializeField] private int extraAccountExp;
        [Header("현재 경험치")] public BigInteger currentAccountExp;
        [Header("최대 경험치")] public BigInteger currentAccountMaxExp;
        
        // 모든 통화의 목록 
        public List<Currency> currencies = new();

        private void Awake()
        {
            Instance = this;
        }

        // 재화 매니저 초기화 메서드
        public void InitAccountManager()
        {
            accountLevel = ES3.Load($"{nameof(accountLevel)}", 1);
            statPoint = ES3.Load($"{nameof(statPoint)}", 0);

            currentAccountExp = ES3.KeyExists(nameof(currentAccountExp)) switch
            {
                true => new BigInteger(ES3.Load<string>($"{nameof(currentAccountExp)}")),
                false => 0
            };
            
            currentAccountMaxExp = accountLevel == 1 ? baseAccountExp : baseAccountExp * (int)Mathf.Pow(accountLevel - 1, 2) + extraAccountExp * (accountLevel - 1);
            UIManager.Instance.squadPanelUI.squadStatPanelUI.squadStatPanelPlayerInfoUI.UpdateSquadStatPanelSquadInfoAllUI(accountLevel, currentAccountExp, currentAccountMaxExp, statPoint);
            UIManager.Instance.playerInfoPanelUI.UpdateLevelPanelUI(accountLevel);
            SetEventListener();
            SetCurrencies();
        }

        private void SetEventListener()
        {
            OnCurrencyChanged += UpdateCurrencyUI;
            LevelUpAction += UpdateLevel;
        }

        private void SetCurrencies()
        {
            if (ES3.KeyExists("currencies")) LoadCurrencies();

            foreach (var currency in currencies) UpdateCurrencyUI(currency.currencyType, currency.amount);
        }

        public void AddExp(BigInteger value)
        {
            currentAccountExp += value;
            ES3.Save($"{nameof(currentAccountExp)}", currentAccountExp.ToString());

            if (currentAccountExp >= currentAccountMaxExp && accountLevel < accountMaxLevel)
            {
                UIManager.Instance.squadPanelUI.squadStatPanelUI.squadStatPanelPlayerInfoUI.UpdateSquadStatPanelSquadInfoLevelUpButton(true);
            }
            
            var sliderValue = currentAccountExp == 0 ? 0 : int.Parse((currentAccountExp * 100/ currentAccountMaxExp).ToString());
            UIManager.Instance.squadPanelUI.squadStatPanelUI.squadStatPanelPlayerInfoUI.UpdateSquadStatPanelSquadInfoExpUI(currentAccountExp, currentAccountMaxExp);
        }
        
        private void UpdateLevel()
        {
            while (currentAccountExp >= currentAccountMaxExp)
            {
                currentAccountExp -= currentAccountMaxExp;
                accountLevel++;
                statPoint++;
                currentAccountMaxExp = baseAccountExp * (int)Mathf.Pow(accountLevel - 1, 2) + extraAccountExp * (accountLevel - 1);
            }
            
            var sliderValue = currentAccountExp == 0 ? 0 : int.Parse((currentAccountExp * 100 / currentAccountMaxExp).ToString());
            
            QuestManager.Instance.IncreaseQuestProgress(Enums.QuestType.SquadLevel, accountLevel);
            UIManager.Instance.squadPanelUI.squadStatPanelUI.CheckRequiredCurrencyOfMagnificationButton(SquadStatManager.Instance.levelUpMagnification);
            UIManager.Instance.squadPanelUI.squadStatPanelUI.squadStatPanelPlayerInfoUI.UpdateSquadStatPanelSquadInfoLevelUpButton(false);
            UIManager.Instance.squadPanelUI.squadStatPanelUI.squadStatPanelPlayerInfoUI.UpdateSquadStatPanelSquadInfoAllUI(accountLevel, currentAccountExp, currentAccountMaxExp, statPoint);
            UIManager.Instance.playerInfoPanelUI.UpdateLevelPanelUI(accountLevel);
            
            ES3.Save($"{nameof(currentAccountExp)}", currentAccountExp.ToString());
            ES3.Save($"{nameof(accountLevel)}", accountLevel);
            ES3.Save($"{nameof(statPoint)}", statPoint);
        }

        // 특정 통화를 증가시키는 메서드
        public void AddCurrency(Enums.CurrencyType currencyType, BigInteger value)
        {
            var currency = currencies.Find(c => c.currencyType == currencyType);
            if (currency != null)
            {
                currency.Add(value);
                OnCurrencyChanged?.Invoke(currencyType, currency.amount); // 이벤트 발생
                SaveCurrencies();
            }
        }

        // 특정 통화를 감소시키는 메서드
        public bool SubtractCurrency(Enums.CurrencyType currencyType, BigInteger value)
        {
            // 모든 통화중 매개변수로 받은 이름이 있나 체크
            var currency = currencies.Find(c => c.currencyType == currencyType);

            if (currency == null) return false;
            // 통화의 양을 감소시키, 결과에 따라 이벤트 발생
            var result = currency.Subtract(value);
            SaveCurrencies();
            if (result) OnCurrencyChanged?.Invoke(currencyType, currency.amount);
            return result;
        }

        // 특정 통화의 현재 양을 반환하는 메서드
        public string GetCurrencyAmount(Enums.CurrencyType currencyType)
        {
            var currency = currencies.Find(c => c.currencyType == currencyType);
            return currency?.amount ?? "0";
        }

        // 모든 통화를 로컬에 저장시키는 메서드
        public void SaveCurrencies()
        {
            ES3.Save("currencies", currencies);
        }

        // 로컬에 저장되어있는 모든 통화를 불러오는 메서드
        private void LoadCurrencies()
        {
            currencies = ES3.Load<List<Currency>>("currencies");

            foreach (var currency in currencies) OnCurrencyChanged?.Invoke(currency.currencyType, currency.amount); // 로딩 후 이벤트 발생
        }

        // 통화의 UI를 업데이트 시키는 메서드
        private void UpdateCurrencyUI(Enums.CurrencyType currencyType, string amount)
        {
            Currency currency;

            switch (currencyType)
            {
                case Enums.CurrencyType.StatPoint:
                    currency = currencies.Find(c => c.currencyType == Enums.CurrencyType.StatPoint);
                    currency.currencyUI.text = $"스탯 포인트 : {BigInteger.ChangeMoney(amount)}";
                    break;
                case Enums.CurrencyType.Gold:
                    currency = currencies.Find(c => c.currencyType == Enums.CurrencyType.Gold);
                    currency.currencyUI.text = $"<sprite={(int)Enums.IconType.Gold}> {BigInteger.ChangeMoney(amount)}";
                    break;
                case Enums.CurrencyType.Dia:
                    currency = currencies.Find(c => c.currencyType == Enums.CurrencyType.Dia);
                    currency.currencyUI.text = $"<sprite={(int)Enums.IconType.Dia}> {BigInteger.ChangeMoney(amount)}";
                    break;
                case Enums.CurrencyType.SquadEnhanceStone:
                    currency = currencies.Find(c => c.currencyType == Enums.CurrencyType.SquadEnhanceStone);
                    currency.currencyUI.text = $"<sprite={(int)Enums.IconType.EnhanceStoneSquad}> {BigInteger.ChangeMoney(amount)}";
                    break;
            }
        }
    }
}