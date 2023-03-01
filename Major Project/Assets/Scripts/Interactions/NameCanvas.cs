using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameCanvas : MonoBehaviour
{
    private Transform mainCameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCameraTransform);
        transform.Rotate(0, 180, 0);
    }
}
