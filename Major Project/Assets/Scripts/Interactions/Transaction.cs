using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transaction", menuName = "Inventory/Transaction")]
public class Transaction : ScriptableObject
{
    [Serializable]
    public struct RequiredItem
    {
        [SerializeField] private ItemData item;
        public ItemData Item => item;
        [SerializeField] private int amount;
        public int Amount => amount;
    }

    [SerializeField] private RequiredItem[] requiredItems;
    public RequiredItem[] RequiredItems => requiredItems;

    [Serializable]
    public struct RewardItemStruct
    {
        [SerializeField] private ItemData item;
        public ItemData Item => item;
        [SerializeField] private int amount;
        public int Amount => amount;
    }

    [SerializeField] private RewardItemStruct rewardItem;
    public RewardItemStruct RewardItem => rewardItem;

    public void Trade()
    {
        Inventory.Instance.RemoveItems(requiredItems.Select(x => x.Item).ToArray(), requiredItems.Select(x => x.Amount).ToArray());

        switch (rewardItem.Item.Type)
        {
            case ItemData.ItemType.Item:
                Inventory.Instance.AddItem(rewardItem.Item, rewardItem.Amount);
                break;
            case ItemData.ItemType.Collectible:
                CollectibleStorage.Instance.AddItem(rewardItem.Item, rewardItem.Amount);
                rewardItem.Item.Collect();
                break;
            case ItemData.ItemType.Tool:
                break;
            default:
                break;
        }
    }
}
