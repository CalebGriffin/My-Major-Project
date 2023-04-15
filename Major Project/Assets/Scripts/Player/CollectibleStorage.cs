using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Only used for debugging, remove later
using NaughtyAttributes;

public class CollectibleStorage : Bag
{
    public static CollectibleStorage Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public override void AddItem(ItemData item, int amount = 1)
    {
        base.AddItem(item, amount);
        if (Items.Count >= 18)
        {
            TimeMachine.Instance.AnimateIn();
        }
    }
}
