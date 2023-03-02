using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Outline))]
public class Item : Interactable
{
    // Inventory Stuff
    [SerializeField] ItemData itemData;
    
    // GameObject Stuff
    [SerializeField] private Outline outline;
    [SerializeField] private GameObject nameCanvas;
    [SerializeField] private TextMeshProUGUI nameText;
    private Vector3 nameCanvasScale = new Vector3(0.0005f, 0.0005f, 1f);
    private float nameCanvasScaleTime = 0.2f;
    private float waitTime = 0.5f;
    private bool canvasEnabled = false;

    // Collection Animation Stuff
    private float collectHeight = 0.5f;
    private float collectAnimationTime = 0.5f;
    
    void Awake()
    {
        // Set up the name canvas and text
        nameText.text = itemData.ItemName;
        nameCanvas.transform.localScale = Vector3.zero;
        nameCanvas.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        LeanTween.cancel(nameCanvas);
        nameCanvas.SetActive(false);

        // Collect the item
        Collect();
    }

    // Defaults to animating the item being plucked from the ground
    // Can be overridden to play a different animation
    virtual protected void Collect()
    {
        LeanTween.scale(gameObject, Vector3.zero, collectAnimationTime).setOnComplete(() => Destroy(gameObject));
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + collectHeight, collectAnimationTime);
    }

    private IEnumerator ShowCanvas()
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

    private void HideCanvas()
    {
        Debug.Log("HideCanvas");
        canvasEnabled = false;
        LeanTween.scale(nameCanvas, Vector3.zero, nameCanvasScaleTime).setOnComplete(() => nameCanvas.SetActive(false));
    }
}
