using System;
using Data;
using Function;
using Keiwando.BigInteger;
using TMPro;

namespace Creature.Data
{
    [Serializable]
    public class Currency
    {
        public Enums.CurrencyType currencyType;
        public string amount;
        public TMP_Text currencyUI;

        public Currency(Enums.CurrencyType currencyType, string initialAmount)
        {
            this.currencyType = currencyType;
            amount = initialAmount;
        }

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
    }
}