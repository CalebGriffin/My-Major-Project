using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color normalColor;

    [SerializeField] private Image image;

    private bool isHovering = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerOver()
    {
        Debug.Log("Hovering");
        isHovering = true;
        image.color = hoverColor;
    }

    public void OnPointerExit()
    {
        Debug.Log("Not Hovering");
        isHovering = false;
        image.color = normalColor;
    }
}
