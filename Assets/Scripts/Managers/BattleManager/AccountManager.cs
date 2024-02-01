using System;
using System.Collections.Generic;
using Creature.Data;
using Data;
using Function;
using UnityEngine;

namespace Managers.BattleManager
{
    public class AccountManager : MonoBehaviour
    {
        public static AccountManager Instance;

        public int accountLevel;
        
        // 모든 통화의 목록 
        public List<Currency> currencies = new();

        private void Awake()
        {
            Instance = this;
        }

        public event Action<Enums.CurrencyType, string> OnCurrencyChanged;

        // 재화 매니저 초기화 메서드
        public void InitAccountManager()
        {
            SetEventListener();
            SetCurrencies();
        }

        private void SetEventListener()
        {
            OnCurrencyChanged += UpdateCurrencyUI;
        }

        private void SetCurrencies()
        {
            if (ES3.KeyExists("currencies")) LoadCurrencies();

            foreach (var currency in currencies) UpdateCurrencyUI(currency.currencyType, currency.amount);
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

            foreach (var currency in
                     currencies) OnCurrencyChanged?.Invoke(currency.currencyType, currency.amount); // 로딩 후 이벤트 발생
        }

        private void CreateCurrencies()
        {
            throw new NotImplementedException();
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