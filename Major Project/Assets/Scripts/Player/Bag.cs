using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Only used for debugging, remove later
using NaughtyAttributes;

public class Bag : MonoBehaviour
{
    protected Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();
    public Dictionary<ItemData, int> Items { get { return items; } protected set { items = value; SaveSystem.Instance.Save(); } }

    //! Only used for debugging, remove later
    [Button]
    protected void LogItems()
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

    protected int[] SetDefaultAmounts(ItemData[] items)
    {
        int[] amounts = new int[items.Length];
        for (int i = 0; i < amounts.Length; i++)
        {
            amounts[i] = 1;
        }
        return amounts;
    }
}