using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

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
        public Enums.SquadStatType statType;
        public Enums.IncreaseStatValueType increaseStatType;
        public int increaseValue;
    }
}