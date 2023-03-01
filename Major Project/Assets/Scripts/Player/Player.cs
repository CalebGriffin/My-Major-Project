using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Input references
    private PlayerInput playerInput;
    private InputAction interactAction;

    // Look references
    [SerializeField] private Transform cameraTransform;
    private float sphereCastRadius = 0.005f;
    private float sphereCastDistance = 4f;
    [SerializeField] private LayerMask interactablesLayerMask;
    private int currentLookedObjectID = -1;

    // Event System Stuff
    public static event Action<int> OnLookEvent;
    public static event Action<int> OnLookAwayEvent;
    public static event Action<int> OnInteractEvent;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player input and the interact action, then add the OnInteract method to the action's performed event
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
        interactAction.performed += Interact;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Look();
    }

    // Fires a sphere cast from the camera's position in the direction it's facing, and calls the OnLookEvent
    // If the sphere cast doesn't hit anything, it calls the OnLookAwayEvent
    void Look()
    {
        // TODO: Change the layer mask to only include interactable objects
        if (Physics.SphereCast(cameraTransform.position, sphereCastRadius, cameraTransform.forward, out RaycastHit hit, sphereCastDistance, interactablesLayerMask, QueryTriggerInteraction.Collide))
        {
            if (currentLookedObjectID == hit.collider.gameObject.GetInstanceID())
                return;
            currentLookedObjectID = hit.collider.gameObject.GetInstanceID();
            OnLookEvent?.Invoke(currentLookedObjectID);
        }
        else
        {
            OnLookAwayEvent?.Invoke(currentLookedObjectID);
            currentLookedObjectID = -1;
        }
    }

    // Called when the player presses the interact button
    // If the player is looking at an interactable object, it sends a message to that object
    void Interact(InputAction.CallbackContext context)
    {
        OnInteractEvent?.Invoke(currentLookedObjectID);
    }
}
