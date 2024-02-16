using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/PushNotesData")]
public class PushNotesDataSO : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private string desc;
    [SerializeField] private int pushTime;
    [SerializeField] private Enums.RewardType rewardType;
    [SerializeField] private int amount;

    public string Title => title;
    public string Desc => desc;
    public int PushTime => pushTime;
    public Enums.RewardType RewardType => rewardType;
    public int Amount => amount;
}
