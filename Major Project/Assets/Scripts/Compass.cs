using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    private new RectTransform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the compass to match the player's rotation
        transform.localRotation = Quaternion.Euler(0, 0, GRefs.Instance.Player.transform.eulerAngles.y);
    }
}
