using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectibleItem : Item
{
    private void Start()
    {
        if (CollectibleStorage.Instance.HasItem(itemData))
            Destroy(gameObject);
    }

    protected override void OnInteract(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;
        
        // Add the item to the collectible storage
        CollectibleStorage.Instance.AddItem(itemData);

        canvasEnabled = false;
        LeanTween.cancel(nameCanvas);
        nameCanvas.SetActive(false);

        // Collect the item
        Collect();
    }

    protected override void Collect()
    {
        itemData.Collect();
        base.Collect();
    }
}
