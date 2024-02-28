using System;
using Data;
using UnityEngine;

namespace Resources.ScriptableObjects.Scripts
{
    [Serializable]
    public class DungeonSo
    {
        [Header("던전 정보")]
        public string dungeonName;
        public Enums.DungeonType dungeonType;
        public int targetScore;
        public int monsterSpawnCountsPerSubStage;
        public int waveTime;
        public Enums.CurrencyType rewardType;
        public int reward;

        [Space(5)] [Header("던전 UI")]
        public GameObject dungeonUI;
        public GameObject dungeonMap;
    }
}