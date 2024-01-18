using System;
using Function;
using TMPro;
using UnityEngine;
using Enum = Data.Enum;

namespace Creature.Data
{
    [Serializable]
    public class Currency
    {
        public Enum.CurrencyType currencyType;
        public string amount;
        public TMP_Text currencyUI;
    
        public void Add(BigInteger value)
        {
            var currentAmount = new BigInteger(amount);
            currentAmount += value;
            amount = currentAmount.ToString();
        }

        public bool Subtract(BigInteger value)
        {
            var currentAmount = new BigInteger(amount);
            if (currentAmount - value < 0) return false;
            currentAmount -= value;
            amount = currentAmount.ToString();
            return true;
        }
    
        public Currency(Enum.CurrencyType currencyType, string initialAmount)
        {
            this.currencyType = currencyType;
            this.amount = initialAmount;
        }
    }
}