using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

public class ObjectTesting : MonoBehaviour
{
    public List<string> objectNames = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void PrintWorldScales()
    {
        print("PrintWorldScales() called");
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        objects = objects.Where(obj => obj.name.Contains("Log")).ToArray();
        
        foreach (GameObject obj in objects)
        {
            Debug.Log($"{obj.name} world scale: {obj.transform.lossyScale}");
        }
        print("PrintWorldScales() finished");
    }

    [Button]
    public void FixObjectRotation()
    {
        print("FixObjectRotation() called");

        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

        foreach (string name in objectNames)
        {
            GameObject[] temp = objects.Where(obj => obj.name.Contains(name)).ToArray();

            foreach (GameObject obj in temp)
            {
                obj.transform.rotation = Quaternion.Euler(0, obj.transform.localEulerAngles.y, 0);
            }
        }
        
        print("FixObjectRotation() finished");
    }
}
