using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<ItemData> InventoryItems = new List<ItemData>();
    public List<int> InventoryItemAmounts = new List<int>();

    public List<ItemData> ToolItems = new List<ItemData>();

    public List<ItemData> CollectibleItems = new List<ItemData>();

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
