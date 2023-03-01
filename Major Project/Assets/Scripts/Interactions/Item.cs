using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Outline))]
public class Item : Interactable
{
    // Inventory Stuff
    [SerializeField] string itemName;
    [SerializeField] Sprite itemSprite;
    
    [SerializeField] private Outline outline;
    [SerializeField] private GameObject nameCanvas;
    [SerializeField] private TextMeshProUGUI nameText;
    private Vector3 nameCanvasScale = new Vector3(0.0005f, 0.0005f, 1f);
    private float nameCanvasScaleTime = 0.2f;
    private float waitTime = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        nameText.text = itemName;
        nameCanvas.transform.localScale = Vector3.zero;
        nameCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override protected void OnLook()
    {
        Debug.Log("Looked at " + itemName);
        outline.enabled = true;
        StartCoroutine(ShowCanvas());
    }

    override protected void OnLookAway()
    {
        Debug.Log("Looked away from " + itemName);
        outline.enabled = false;
        HideCanvas();
    }

    override protected void OnInteract()
    {

    }

    private IEnumerator ShowCanvas()
    {
        yield return new WaitForSeconds(waitTime);
        nameCanvas.SetActive(true);
        LeanTween.scale(nameCanvas, nameCanvasScale, nameCanvasScaleTime);
    }

    private void HideCanvas()
    {
        LeanTween.scale(nameCanvas, Vector3.zero, nameCanvasScaleTime).setOnComplete(() => nameCanvas.SetActive(false));
    }
}
