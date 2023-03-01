using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Only used for debugging, remove later
using NaughtyAttributes;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();
    public Dictionary<ItemData, int> Items
    {
        get { return items; }

        private set
        {
            items = value;
        }
    }

    void Awake()
    {
        // Set the instance to this if it is null, otherwise destroy this
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //! Only used for debugging, remove later
    [Button]
    void LogItems()
    {
        foreach (KeyValuePair<ItemData, int> item in Items)
        {
            Debug.Log(item.Key.ItemName + ": " + item.Value);
        }
    }

    #region Add Items
    public void AddItem(ItemData item, int amount = 1)
    {
        if (Items.ContainsKey(item))
        {
            Items[item] += amount;
        }
        else
        {
            Items.Add(item, amount);
        }
    }

    public void AddItems(ItemData[] items, int[] amounts = null)
    {
        if (amounts == null)
            amounts = SetDefaultAmounts(items);

        for (int i = 0; i < items.Length; i++)
        {
            AddItem(items[i], amounts[i]);
        }
    }
    #endregion

    #region Has Items
    public bool HasItem(ItemData item, int amount = 1)
    {
        if (Items.ContainsKey(item))
        {
            return Items[item] >= amount;
        }
        else
        {
            return false;
        }
    }

    public bool HasItems(ItemData[] items, int[] amounts = null)
    {
        if (amounts == null)
            amounts = SetDefaultAmounts(items);

        for (int i = 0; i < items.Length; i++)
        {
            if (!HasItem(items[i], amounts[i]))
            {
                return false;
            }
        }
        return true;
    }
    #endregion

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
            amounts = SetDefaultAmounts(items);

        for (int i = 0; i < items.Length; i++)
        {
            RemoveItem(items[i], amounts[i]);
        }
    }
    #endregion

    #region Get Items

    #endregion
    private int[] SetDefaultAmounts(ItemData[] items)
    {
        int[] amounts = new int[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            amounts[i] = 1;
        }
        return amounts;
    }
}
