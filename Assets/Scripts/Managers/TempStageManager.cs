// using UnityEngine;
//
// namespace Managers
// {
//     using System;
// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;
// using Newtonsoft.Json;
//
// [Serializable]
// public class StageData
// {
//     public int StageChapter;
//     public int StageNumber;
//     public int KillGoal;
//     public int GoldPerKill;
//     public int SpawnCount;
//     public float SpawnDelay;
//     public int BossMonsterType;
//     public int MonsterType;
//     public int MapType;
// }
//
// public class StageDataParser
// {
//     public static List<StageData> ParseCsv(string filePath)
//     {
//         var stages = new List<StageData>();
//
//         using (var reader = new StreamReader(filePath))
//         {
//             bool isFirstLine = true;
//             string line;
//             int lineNumber = 0;
//
//             while ((line = reader.ReadLine()) != null)
//             {
//                 lineNumber++; // 줄 번호 증가
//
//                 if (isFirstLine)
//                 {
//                     isFirstLine = false;
//                     continue;
//                 }
//
//                 if (string.IsNullOrWhiteSpace(line)) // 빈 줄 건너뛰기
//                     continue;
//
//                 var values = line.Split(',');
//
//                 if (values.Length != 9) // 예상된 열의 수 확인
//                 {
//                     Debug.LogError($"Line {lineNumber}: Number of columns mismatch. Expected 9 columns, but found {values.Length}.");
//                     continue;
//                 }
//
//                 try
//                 {
//                     var stageData = new StageData
//                     {
//                         StageChapter = int.Parse(values[0].Trim()),
//                         StageNumber = int.Parse(values[1].Trim()),
//                         KillGoal = int.Parse(values[2].Trim()),
//                         GoldPerKill = int.Parse(values[3].Trim()),
//                         SpawnCount = int.Parse(values[4].Trim()),
//                         SpawnDelay = float.Parse(values[5].Trim()),
//                         BossMonsterType = int.Parse(values[6].Trim()),
//                         MonsterType = int.Parse(values[7].Trim()),
//                         MapType = int.Parse(values[8].Trim())
//                     };
//                     stages.Add(stageData);
//                 }
//                 catch (FormatException ex)
//                 {
//                     Debug.LogError($"FormatException in line {lineNumber}: {line}. Error: {ex.Message}");
//                 }
//             }
//         }
//
//
//
//
//         return stages;
//     }
//
//     public static void SaveAsJson(List<StageData> stages, string filePath)
//     {
//         string json = JsonConvert.SerializeObject(stages, Formatting.Indented);
//         File.WriteAllText(filePath, json);
//     }
// }
//
//
// public class TempStageManager : MonoBehaviour
// {
//     [SerializeField] MonsterSpawner monsterPool;
//
//
//     [SerializeField] List<StageData> stageDatas = new List<StageData>();
//
//     [SerializeField] private int currentStageIndex = 0;
//     [SerializeField] private int currentKillCount = 0;
//
//     [SerializeField] private bool isSpawningMonsters = false;
//     private float currentSpawnTimer = 0f;
//
//
//     [SerializeField] private bool isBossSpawned = false;
//     [SerializeField] private float bossSpawnTimer = 0f;
//     [SerializeField] private bool isKillBossMonster = false;
//     private float bossFightTimer = 0f;
//
//     StageData currentStage;
//
//     private List<GameObject> normalMonsters = new List<GameObject>();
//
//
//     private void Start()
//     {
//         StageDataLoad();
//     }
//
//     void StageDataLoad()
//     {
//         // CSV 파일 경로 설정 (Resources 폴더 내)
//         string csvFilePath = Path.Combine(Application.dataPath, "Resources/StageInfo.csv");
//         stageDatas = StageDataParser.ParseCsv(csvFilePath);
//
//         // JSON 파일 경로 설정 (persistentDataPath 사용)
//         string jsonFilePath = Path.Combine(Application.persistentDataPath, "StageInfo.json");
//
//         // JSON 파일이 없으면 새로 생성
//         if (!File.Exists(jsonFilePath))
//         {
//             StageDataParser.SaveAsJson(stageDatas, jsonFilePath);
//         }
//
//         // 스테이지 데이터 로그 출력
//         foreach (var stage in stageDatas)
//         {
//             Debug.Log(stage + "\n");
//         }
//     }
//
//     void Update()
//     {
//         // 게임 진행 중일 때
//         if (isSpawningMonsters)
//         {
//             // 현재 스테이지의 데이터 가져오기
//             StageData currentStage = stageDatas[currentStageIndex];
//
//             // 일정 시간마다 몬스터 소환
//             currentSpawnTimer += Time.deltaTime;
//             if (currentSpawnTimer >= currentStage.SpawnDelay - 0.5f)
//             {
//                 currentSpawnTimer = 0f;
//
//                 if (!isBossSpawned) // 보스 몬스터가 생성되지 않은 경우에만 일반 몬스터 생성
//                 {
//
//                     // 몬스터를 소환하는 함수를 호출하고 반환된 몬스터를 받음
//                     GameObject spawnedMonster = monsterPool.SpawnMonster(currentStage.MonsterType);
//
//                     // 만약 몬스터가 소환되었다면
//                     if (spawnedMonster != null)
//                     {
//                         // 몬스터가 처치되면 KillCount를 증가시키고 몬스터를 비활성화
//                         MonsterController monsterController = spawnedMonster.GetComponent<MonsterController>();
//                         monsterController.OnMonsterKilled += () =>
//                         {
//                             currentKillCount++;
//
//                             // KillCount가 목표치에 도달하면 보스 몬스터 생성
//                             if (currentKillCount >= currentStage.KillGoal && !isBossSpawned)
//                             {
//                                 isBossSpawned = true;
//                                 bossFightTimer = 0f; // 보스 몬스터와의 전투 타이머 초기화
//                                 Debug.Log("보스 몬스터 생성!");
//
//                                 // 모든 현재 소환된 일반 몬스터를 파괴
//                                 foreach (var monster in normalMonsters)
//                                 {
//                                     monster.SetActive(false);
//                                 }
//
//                                 normalMonsters.Clear(); // List 초기화
//
//                                 // 여기서 보스 몬스터 생성 및 필요한 설정을 수행하세요.
//                                 // SpawnBossMonster 함수를 호출하여 보스 몬스터를 생성합니다.
//                                 var bossMonster = SpawnBossMonster(currentStage.BossMonsterType);
//
//                                 bossMonster.OnMonsterKilled += delegate { isKillBossMonster = true; };
//                             }
//
//                             // 모든 스테이지를 클리어했을 때의 처리 (보스 몬스터 등)
//                             if (currentStageIndex >= stageDatas.Count)
//                             {
//                                 // 게임 클리어 처리 등을 여기에 추가
//                                 // 예: 게임 클리어 화면 표시, 다음 스테이지 로드 등
//                                 Debug.Log("All stages cleared!");
//                                 isSpawningMonsters = false;
//                             }
//                         };
//
//                         // 생성된 몬스터를 List에 추가
//                         normalMonsters.Add(spawnedMonster);
//                     }
//                 }
//             }
//         }
//
//         // 보스 몬스터가 생성되면
//         if (isBossSpawned)
//         {
//             // 보스 몬스터와의 전투 시간 제한
//             bossFightTimer += Time.deltaTime;
//
//             // 보스 몬스터를 제한 시간 내에 처치한 경우
//             if (bossFightTimer < 60f)
//             {
//                 //isKillBossMonster = true; // 보스 몬스터를 처치했음을 표시
//             }
//
//             // 보스 몬스터를 처치했거나 제한 시간이 지났을 경우
//             if (isKillBossMonster || bossFightTimer >= 60f)
//             {
//                 currentStageIndex++; // Proceed to the next stage
//                 currentKillCount = 0;
//                 isBossSpawned = false;
//                 isKillBossMonster = false;
//                 Debug.Log("다음 스테이지로 진행합니다.");
//             }
//             else
//             {
//                 currentKillCount = 0;
//                 isBossSpawned = false;
//                 isKillBossMonster = false;
//
//                 GameObject spawnedMonster = monsterPool.SpawnMonster(currentStage.MonsterType);
//             }
//         }
//     }
//
//
//
//
//     // 보스 몬스터 소환 함수
//     private MonsterController SpawnBossMonster(int bossMonsterType)
//     {
//         // 보스 몬스터를 소환하고 필요한 설정을 추가할 수 있습니다.
//         // 이 함수를 구현하여 보스 몬스터의 생성 및 설정을 수행하세요.
//
//         return monsterPool.SpawnBossMonster(bossMonsterType).GetComponent<MonsterController>();
//
//         // 예: Instantiate(bossMonsterPrefab, spawnPoint.position, Quaternion.identity);
//     }
// }
// }