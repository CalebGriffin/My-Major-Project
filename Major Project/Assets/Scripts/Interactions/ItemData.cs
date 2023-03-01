using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] string itemName;
    public string ItemName { get { return itemName; } }

    [SerializeField] Sprite itemSprite;
    public Sprite ItemSprite { get { return itemSprite; } }
}
