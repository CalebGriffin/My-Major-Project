using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private Dictionary<Item, int> items = new Dictionary<Item, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item item, int amount)
    {
        if (items.ContainsKey(item))
        {
            items[item] += amount;
        }
        else
        {
            items.Add(item, amount);
        }
    }

    public void AddItems(Item[] items, int[] amounts)
    {
        for (int i = 0; i < items.Length; i++)
        {
            AddItem(items[i], amounts[i]);
        }
    }

    public bool HasItem(Item item, int amount)
    {
        if (items.ContainsKey(item))
        {
            return items[item] >= amount;
        }
        else
        {
            return false;
        }
    }

    public bool HasItems(Item[] items, int[] amounts)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (!HasItem(items[i], amounts[i]))
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveItem(Item item, int amount)
    {
        if (items.ContainsKey(item))
        {
            items[item] -= amount;
            if (items[item] <= 0)
            {
                items.Remove(item);
            }
        }
    }

    public void RemoveItems(Item[] items, int[] amounts)
    {
        for (int i = 0; i < items.Length; i++)
        {
            RemoveItem(items[i], amounts[i]);
        }
    }
}
