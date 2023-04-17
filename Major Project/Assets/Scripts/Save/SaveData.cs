using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<string> InventoryItems = new List<string>();
    public List<int> InventoryItemAmounts = new List<int>();

    public List<string> ToolItems = new List<string>();

    public List<string> CollectibleItems = new List<string>();

    public Vector3 PlayerPosition;
    public Quaternion PlayerRotation;

    public bool SpokenToCedric;
    public bool SpokenToIsabella;
    public bool SpokenToSimon;
    public bool SpokenToEdith;
    public bool SpokenToMarcus;
    public bool SpokenToTobias;

    public bool EndingAchieved;
    public bool TimeMachineKept;

    public bool GameHasBeenPlayedBefore;
}
