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
}
