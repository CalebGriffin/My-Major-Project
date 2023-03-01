using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//! Only used for debugging, remove later
using NaughtyAttributes;

public class InventoryUI : MonoBehaviour
{
    private Dictionary<ItemData, int> prevItems = new Dictionary<ItemData, int>();

    [SerializeField] private GameObject[] itemSlots = new GameObject[30];

    [SerializeField] private TextMeshProUGUI currentItemText;

    private int currentItemIndex = 0;
    private int previousItemIndex = 0;

    private Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color deselectedColor = new Color(0, 0, 0, 0.5f);

    private bool inventoryOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //! Only used for debugging, remove later
    [Button]
    private void InventoryUIToggle()
    {
        // Toggle the inventory UI
        inventoryOpen = !inventoryOpen;

        if (inventoryOpen)
        {
            UpdateInventoryUI();
            AnimateInventoryUIIn();
        }
        else
        {
            AnimateInventoryUIOut();
        }
    }

    private void UpdateInventoryUI()
    {
        // Check if the inventory has changed
        if (Inventory.Instance.Items != prevItems)
        {
            // Update the previous items
            prevItems = Inventory.Instance.Items;

            // Clear the UI
            foreach (GameObject itemSlot in itemSlots)
            {
                itemSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;
                itemSlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }

            // Update the UI
            for (int i = 0; i < Inventory.Instance.Items.Count; i++)
            {
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = Inventory.Instance.Items.Keys.ToArray()[i].ItemSprite;
                itemSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Inventory.Instance.Items.Values.ToArray()[i].ToString();
            }
        }
    }

    private void AnimateInventoryUIIn()
    {
        // Animate the inventory UI in
        LeanTween.moveLocalY(gameObject, 0, 0.5f).setEaseOutBack();
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEaseOutBack();
    }

    private void AnimateInventoryUIOut()
    {
        // Animate the inventory UI out
        LeanTween.moveLocalY(gameObject, -1000, 0.5f).setEaseInBack();
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEaseInBack();
    }
}