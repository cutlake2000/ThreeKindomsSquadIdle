using System;
using System.Collections.Generic;
using UnityEngine;
using Enum = Data.Enum;

namespace ScriptableObjects.Scripts
{
    [Serializable]
    public class SquadEffectSo
    {
        public List<SquadEffect> squadEffects = new();
    }

    [Serializable]
    public class SquadEffect
    {
        public Enum.StatTypeBySquadConfigurePanel statType;
        public Enum.IncreaseStatValueType increaseStatType;
        public int increaseValue;
    }
}