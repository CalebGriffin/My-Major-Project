using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace Caleb
{
    public class Debug : MonoBehaviour
    {
        private List<ItemData> collectibleItems = new List<ItemData>();

        void Start()
        {
            string[] assetNames = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Prefabs/Item Data/Collectibles" });
            collectibleItems.Clear();
            foreach (string SOName in assetNames)
            {
                var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
                var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
                collectibleItems.Add(itemData);
            }

            foreach (ItemData item in collectibleItems)
            {
                print(item.name);
            }
        }

        private void OnApplicationQuit()
        {
            foreach (ItemData item in collectibleItems)
            {
                item.Reset();
            }
        }
    }
}
#endif