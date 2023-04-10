using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Collectible Item"), System.Serializable]
public class CollectibleItemData : ItemData
{
    [SerializeField] string actualName;
    public string ActualName => actualName;

    public override void Collect()
    {
        ItemName = actualName;
    }

    public override void Reset()
    {
        ItemName = "???";
    }
}
