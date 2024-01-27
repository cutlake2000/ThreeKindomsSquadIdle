using System;
using System.Collections.Generic;
using UnityEngine;
using Enum = Data.Enum;

namespace ScriptableObjects.Scripts
{
    [Serializable]
    public class SquadEffectSo
    {
        public List<SquadEffect> squadEffects;
    }

    [Serializable]
    public struct SquadEffect
    {
        public Enum.SquadStatType statType;
        public Enum.IncreaseStatValueType increaseStatType;
        public int increaseValue;
    }
}