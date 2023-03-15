using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolStorage : Bag
{
    public static ToolStorage Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
