using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassPoint : MonoBehaviour
{
    [SerializeField] private RectTransform compassTransform;
    private new RectTransform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, 0, -compassTransform.localRotation.eulerAngles.z);
    }
}
