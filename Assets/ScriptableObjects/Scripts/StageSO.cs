using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace ScriptableObjects.Scripts
{
    [CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData")]
    public class StageSo : ScriptableObject
    {
        [field: Header("메인 스테이지 정보")]
        [field: SerializeField] public List<MainStageInfo> MainStageInfos { get; private set; }
        [field: Header("메인 스테이지 당 서브 스테이지 수")]
        [field: SerializeField] public int SubStageCountsPerMainStage { get; private set; }
        [field: Header("서브 스테이지 당 웨이브 수")]
        [field: SerializeField] public int WaveCountsPerSubStage { get; private set; }
        [field: Header("서브 스테이지 당 스폰시킬 몬스터 수")]
        [field: SerializeField] public int MonsterSpawnCountsPerSubStage { get; private set; }
        [field: Header("스테이지 제한 시간")]
        [field: SerializeField] public float StageLimitedTime { get; private set; }
    }

    [Serializable]
    public struct MainStageInfo
    {
        [field: Header("메인 스테이지 이름")]
        [field: SerializeField] public string MainStageName { get; private set; }
        [field: Header("메인 스테이지 몬스터 타입")]
        [field: SerializeField] public Enums.MonsterClassType[] MainStageMonsterTypes { get; private set; }
    }
}
