using System;
using UnityEngine;
using UnityEngine.Serialization;
using Enum = Data.Enum;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(fileName = "Summon", menuName = "ScriptableObjects/Summon")]
    public class SummonSo : ScriptableObject
    {
        [field:SerializeField] public SummonEquipments[] SummonEquipments { get; private set; }

        public SummonEquipments GetProbability(int level)
        {
            return SummonEquipments[level - 1];
        }
    }

    [Serializable]
    public class SummonEquipments
    {
        [field: Header("[소환 레벨]")]
        [field: SerializeField] public int SummonLevel { get; private set; }
        [field: Header("[레벨 당 소환 확률]")]
        [field: SerializeField] public SummonProbability[] SummonProbabilities { get; private set; }
    }

    [Serializable]
    public class SummonProbability
    {
        [FormerlySerializedAs("rarity")]
        [field: Header("--- 레어도 / 가중치 ---")]
        [field: SerializeField] public Enum.EquipmentRarity equipmentRarity;
        [field: SerializeField] public float weight;
    }
}