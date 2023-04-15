using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolItem : Item
{
    private void Start()
    {
        if (ToolStorage.Instance.HasItem(itemData))
            Destroy(gameObject);
    }

    protected override void OnInteract(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;
        
        // Add the item to the tool storage
        ToolStorage.Instance.AddItem(itemData);

        canvasEnabled = false;
        LeanTween.cancel(nameCanvas);
        nameCanvas.SetActive(false);

        // Collect the item
        Collect();
    }

    protected override void Collect()
    {
        LeanTween.scale(gameObject, Vector3.zero, collectAnimationTime).setEaseInBack().setOnComplete(() =>
        {
            itemData.Collect();
            Destroy(gameObject);
        });
    }
}
