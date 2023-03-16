using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private Image image;

    [HideInInspector]
    public ItemData ItemData;

    [SerializeField] private TextMeshProUGUI currentItemText;

    [SerializeField] private ItemSlot up;
    public ItemSlot Up { get { return up; } set { up = value; } }

    [SerializeField] private ItemSlot down;
    public ItemSlot Down { get { return down; } set { down = value; } }

    [SerializeField] private ItemSlot left;
    public ItemSlot Left { get { return left; } set { left = value; } }

    [SerializeField] private ItemSlot right;
    public ItemSlot Right { get { return right; } set { right = value; } }

    private bool isHovering = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Highlight()
    {
        image.color = hoverColor;

        if (ItemData == null)
            currentItemText.text = "Current Item:";
        else
        {
            int amount = -1;

            if (transform.GetChild(1).GetComponent<TextMeshProUGUI>().text != "")
                amount = int.Parse(transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
            
            string amountString = amount == -1 ? "" : amount.ToString() + " ";

            currentItemText.text = $"Current Item: {amountString}{ItemData.ItemName}";

            if (amount > 1 && ItemData.ItemName.Substring(ItemData.ItemName.Length - 1) != "s")
                currentItemText.text += "s";
        }

        isHovering = true;
    }

    public void UnHighlight()
    {
        image.color = normalColor;
        isHovering = false;
    }
}
