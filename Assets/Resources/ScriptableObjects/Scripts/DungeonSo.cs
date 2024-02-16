using System;
using Data;
using UnityEngine;

namespace ScriptableObjects.Scripts
{
    [Serializable]
    public class DungeonSo
    {
        public string dungeonName;
        public Enums.DungeonType dungeonType;
        public int targetScore;
        public int monsterSpawnCountsPerSubStage;
        public int waveTime;
        public Enums.CurrencyType rewardType;
        public int reward;
    }
}