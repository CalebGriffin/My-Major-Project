using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item"), System.Serializable]
public class ItemData : ScriptableObject
{
    [SerializeField] string itemName;
    public string ItemName { get { return itemName; } protected set { itemName = value; } }

    [SerializeField] Sprite itemSprite;
    public Sprite ItemSprite { get { return itemSprite; } }

    [SerializeField] GameObject itemParticles;
    public GameObject ItemParticles { get { return itemParticles; } }

    [SerializeField] bool isTradeable;
    public bool IsTradeable { get { return isTradeable; } }

    [System.Serializable]
    public enum ItemType
    {
        Item,
        Tool,
        Collectible
    }

    [SerializeField] ItemType itemType;
    public ItemType Type { get { return itemType; } }

    public virtual void Collect()
    {}

    public virtual void Reset()
    {}
}
