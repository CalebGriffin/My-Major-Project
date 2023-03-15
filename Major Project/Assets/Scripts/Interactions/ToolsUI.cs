using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToolsUI : MonoBehaviour
{
    private ItemData[] prevItemsKeysArray;

    [SerializeField] private GameObject[] itemSlots = new GameObject[6];

    public void UpdateToolsUI()
    {
        if (ToolsChanged())
        {
            // Clear the UI
            foreach (GameObject itemSlot in itemSlots)
            {
                itemSlot.transform.GetChild(0).GetComponent<Image>().sprite = null;
                itemSlot.GetComponent<ItemSlot>().ItemData = null;
                itemSlot.transform.GetChild(0).gameObject.SetActive(false);
                itemSlot.transform.GetChild(1).gameObject.SetActive(false);
            }

            // Update the UI
            for (int i = 0; i < ToolStorage.Instance.Items.Count; i++)
            {
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = ToolStorage.Instance.Items.Keys.ToArray()[i].ItemSprite;
                itemSlots[i].GetComponent<ItemSlot>().ItemData = ToolStorage.Instance.Items.Keys.ToArray()[i];
                itemSlots[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private bool ToolsChanged()
    {
        ItemData[] currentItemsKeysArray = ToolStorage.Instance.Items.Keys.ToArray();

        if (prevItemsKeysArray == currentItemsKeysArray)
        {
            return false;
        }
        else
        {
            prevItemsKeysArray = currentItemsKeysArray;
            return true;
        }
    }
}
