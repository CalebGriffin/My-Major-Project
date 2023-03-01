using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected virtual void OnLook()
    {
        Debug.Log("Looked at.");
    }

    protected virtual void OnLookAway()
    {
        Debug.Log("Looked away.");
    }

    protected virtual void OnInteract()
    {
        Debug.Log("Interacted with.");
    }
}
