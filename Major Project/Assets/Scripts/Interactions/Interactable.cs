using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        // Subscribe to Player events
        Player.OnLookEvent += OnLook;
        Player.OnLookAwayEvent += OnLookAway;
        Player.OnInteractEvent += OnInteract;
    }

    protected virtual void OnDisable()
    {
        // Unsubscribe from Player events
        Player.OnLookEvent -= OnLook;
        Player.OnLookAwayEvent -= OnLookAway;
        Player.OnInteractEvent -= OnInteract;
    }

    protected virtual void OnLook(int id)
    {
        Debug.Log("Looked at.");
    }

    protected virtual void OnLookAway(int id)
    {
        Debug.Log("Looked away.");
    }

    protected virtual void OnInteract(int id)
    {
        Debug.Log("Interacted with.");
    }
}
