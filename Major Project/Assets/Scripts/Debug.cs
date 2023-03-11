using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caleb
{
    public class Debug : MonoBehaviour
    {
        [SerializeField] private ItemData[] collectibleItems;

        private void OnApplicationQuit()
        {
            #if UNITY_EDITOR
            foreach (ItemData item in collectibleItems)
            {
                item.Reset();
            }
            #endif
        }
    }
}

