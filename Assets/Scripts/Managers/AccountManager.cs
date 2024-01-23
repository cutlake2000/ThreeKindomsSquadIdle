using System;
using System.Collections.Generic;
using Creature.Data;
using Function;
using UnityEngine;
using Enum = Data.Enum;

namespace Managers
{
    public class AccountManager : MonoBehaviour
    {
        public static AccountManager Instance;
        
        public event Action<Enum.CurrencyType, string> OnCurrencyChanged;

        // 모든 통화의 목록 
        public List<Currency> currencies = new();

        private void Awake()
        {
            Instance = this;
        }

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

            foreach (var currency in currencies)
            {
                UpdateCurrencyUI(currency.currencyType, currency.amount);
            }
        }

        // 특정 통화를 증가시키는 메서드
        public void AddCurrency(Enum.CurrencyType currencyType, BigInteger value)
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
        public bool SubtractCurrency(Enum.CurrencyType currencyType, BigInteger value)
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
        public string GetCurrencyAmount(Enum.CurrencyType currencyType)
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
            
            foreach (var currency in currencies)
            {
                OnCurrencyChanged?.Invoke(currency.currencyType, currency.amount); // 로딩 후 이벤트 발생
            }
        }

        private void CreateCurrencies()
        {
            throw new NotImplementedException();
        }

        // 통화의 UI를 업데이트 시키는 메서드
        private void UpdateCurrencyUI(Enum.CurrencyType currencyType, string amount)
        {
            Currency currency;
            
            switch (currencyType)
            {
                case Enum.CurrencyType.StatPoint:
                    currency = currencies.Find(c => c.currencyType == Enum.CurrencyType.StatPoint);
                    currency.currencyUI.text = $"스탯 포인트 : {BigInteger.ChangeMoney(amount)}";
                    break;
                case Enum.CurrencyType.Gold:
                    currency = currencies.Find(c => c.currencyType == Enum.CurrencyType.Gold);
                    currency.currencyUI.text = $"<sprite=15> {BigInteger.ChangeMoney(amount)}";
                    break;
                case Enum.CurrencyType.Dia:
                    currency = currencies.Find(c => c.currencyType == Enum.CurrencyType.Dia);
                    currency.currencyUI.text = $"<sprite=16> {BigInteger.ChangeMoney(amount)}";
                    break;
            }
        }
    }
}