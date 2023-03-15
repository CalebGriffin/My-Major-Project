using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectiblesUI : MonoBehaviour
{
    private ItemData[] prevItemsKeysArray;

    [SerializeField] private GameObject[] itemSlots = new GameObject[18];

    public void UpdateCollectiblesUI()
    {
        if (CollectiblesChanged())
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
            for (int i = 0; i < CollectibleStorage.Instance.Items.Count; i++)
            {
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = CollectibleStorage.Instance.Items.Keys.ToArray()[i].ItemSprite;
                itemSlots[i].GetComponent<ItemSlot>().ItemData = CollectibleStorage.Instance.Items.Keys.ToArray()[i];
                itemSlots[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private bool CollectiblesChanged()
    {
        ItemData[] currentItemsKeysArray = CollectibleStorage.Instance.Items.Keys.ToArray();

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
