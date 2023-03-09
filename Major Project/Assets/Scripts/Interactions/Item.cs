using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Outline), typeof(Collider))]
public class Item : Interactable
{
    // Inventory Stuff
    [SerializeField] protected ItemData itemData;
    
    // GameObject Stuff
    [SerializeField] protected Outline outline;
    [SerializeField] protected GameObject nameCanvas;
    [SerializeField] protected TextMeshProUGUI nameText;
    protected Vector3 nameCanvasScale = new Vector3(0.0005f, 0.0005f, 1f);
    protected float nameCanvasScaleTime = 0.2f;
    protected float waitTime = 0.5f;
    protected bool canvasEnabled = false;

    // Collection Animation Stuff
    protected float collectHeight = 0.3f;
    protected float collectAnimationTime = 0.3f;
    
    void Awake()
    {
        // Set up the name canvas and text
        nameText.text = itemData.ItemName;
        nameCanvas.transform.localScale = Vector3.zero;
        nameCanvas.SetActive(false);
    }

    override protected void OnLook(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;

        outline.enabled = true;
        StartCoroutine(ShowCanvas());
    }

    override protected void OnLookAway(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;

        outline.enabled = false;
        LeanTween.cancel(nameCanvas);
        StopCoroutine(ShowCanvas());
        HideCanvas();
    }

    override protected void OnInteract(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;

        // Add the item to the inventory
        Inventory.Instance.AddItem(this.itemData);

        canvasEnabled = false;
        LeanTween.cancel(nameCanvas);
        nameCanvas.SetActive(false);

        // Collect the item
        Collect();
    }

    // Defaults to animating the item being plucked from the ground
    // Can be overridden to play a different animation
    virtual protected void Collect()
    {
        if (itemData.ItemParticles != null)
            GameObject.Instantiate(itemData.ItemParticles, transform.position, Quaternion.Euler(-90, 0, 0));
        LeanTween.scale(gameObject, Vector3.zero, collectAnimationTime).setOnComplete(() => Destroy(gameObject));
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + collectHeight, collectAnimationTime);
    }

    protected IEnumerator ShowCanvas()
    {
        Debug.Log("ShowCanvas");
        canvasEnabled = true;
        yield return new WaitForSeconds(waitTime);
        if (!canvasEnabled)
            yield break;
        Debug.Log("ShowCanvas (after wait)");
        nameCanvas.SetActive(true);
        LeanTween.scale(nameCanvas, nameCanvasScale, nameCanvasScaleTime);
    }

    protected void HideCanvas()
    {
        Debug.Log("HideCanvas");
        canvasEnabled = false;
        LeanTween.scale(nameCanvas, Vector3.zero, nameCanvasScaleTime).setOnComplete(() => nameCanvas.SetActive(false));
    }
}
