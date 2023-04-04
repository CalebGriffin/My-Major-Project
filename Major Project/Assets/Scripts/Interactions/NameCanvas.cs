using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameCanvas : MonoBehaviour
{
    private Transform mainCameraTransform;
    private new Transform transform;

    void OnEnable()
    {
        if (mainCameraTransform == null)
            mainCameraTransform = GRefs.Instance.PlayerCameraTransform;
        
        if (transform == null)
            transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCameraTransform);
        transform.Rotate(0, 180, 0);
    }
}
