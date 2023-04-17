using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameCanvas : MonoBehaviour
{
    private new Transform transform;

    void OnEnable()
    {
        if (transform == null)
            transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(GRefs.Instance.PlayerCameraTransform);
        transform.Rotate(0, 180, 0);
    }
}
