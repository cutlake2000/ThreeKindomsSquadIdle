using System;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(fileName = "Summon", menuName = "ScriptableObjects/Summon")]
    public class SummonSo : ScriptableObject
    {
        [field:SerializeField] public int[] SummonRequiredCount { get; private set; }
        [field:SerializeField] public SummonProbability[] SummonWeapons { get; private set; }
        [field:SerializeField] public SummonProbability[] SummonGears { get; private set; }
        [field:SerializeField] public SummonProbability[] SummonSquads { get; private set; }

        public int GetRequiredExp(Enums.SummonType summonType, int level)
        {
            return SummonRequiredCount[level - 1];
        }
        
        public SummonProbability GetProbability(Enums.SummonType summonType, int level)
        {
            return summonType switch
            {
                Enums.SummonType.Weapon => SummonWeapons[level - 1],
                Enums.SummonType.Gear => SummonGears[level - 1],
                Enums.SummonType.Squad => SummonSquads[level - 1],
                _ => throw new ArgumentOutOfRangeException(nameof(summonType), summonType, null)
            };
        }
    }

    [Serializable]
    public class SummonProbability
    {
        [field: Header("[레벨 당 소환 확률]")]
        [field: SerializeField] public float[] SummonProbabilities { get; private set; }
    }
}