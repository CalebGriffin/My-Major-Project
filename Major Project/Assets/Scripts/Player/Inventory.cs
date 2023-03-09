using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Only used for debugging, remove later
using NaughtyAttributes;

public class Inventory : Bag
{
    public static Inventory Instance;

    void Awake()
    {
        // Set the instance to this if it is null, otherwise destroy this
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #region Remove Items
    public void RemoveItem(ItemData item, int amount = 1)
    {
        if (HasItem(item, amount))
        {
            Items[item] -= amount;
            if (Items[item] <= 0)
            {
                Items.Remove(item);
            }
        }
    }

    public void RemoveItems(ItemData[] items, int[] amounts = null)
    {
        if (amounts == null)
            amounts = base.SetDefaultAmounts(items);

        for (int i = 0; i < items.Length; i++)
        {
            RemoveItem(items[i], amounts[i]);
        }
    }
    #endregion
}
