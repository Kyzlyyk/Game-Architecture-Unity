using Kyzlyk.Core;
using UnityEngine;
using System;
using Kyzlyk.Helpers;

namespace Core.PlayerControl.Core
{
    public sealed class StatusParameter
    {
        public StatusParameter(int maxValue, int defaultValue)
        {
            if (defaultValue > maxValue)
                throw new ArgumentOutOfRangeException(nameof(defaultValue));

            Amount = defaultValue;
            MaxValue = maxValue;
        }

        public StatusParameter Zero => new(MaxValue, 0);

        public int Amount { get; private set; }
        public int MaxValue { get; }

        public event EventHandler<StatusParameter, int> OnChanged;

        public void AddStaticly(Percent percent)
        {
            AddInternal(percent.ValueFromPercent(MaxValue));
        }
        
        public void AddRelatively(Percent percent)
        {
            AddInternal(percent.ValueFromPercent(Amount));
        }
        
        public void SubtractRelatively(Percent percent)
        {
            AddRelatively(-percent);
        }
        
        public void SubtractStaticly(Percent percent)
        {
            AddStaticly(-percent);
        }

        private void AddInternal(int value)
        {
            int prev = Amount;

            Amount += value;
            Amount = Mathf.Clamp(Amount, 0, MaxValue);

            OnChanged?.Invoke(this, Amount - prev);
        }
    }
}