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
    private float sphereCastDistance = 3f;
    private GameObject currentLookedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player input and the interact action, then add the OnInteract method to the action's performed event
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"];
        interactAction.performed += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Look();
        Debug.Log("Looking at " + currentLookedObject);
    }

    // Fires a sphere cast from the camera's position in the direction it's facing, and if it hits an interactable object, it sends a message to that object
    // Keeps track of the last interactable object that was looked at, and sends a message to that object when the player looks away
    void Look()
    {
        // TODO: Change the layer mask to only include interactable objects
        if (Physics.SphereCast(cameraTransform.position, sphereCastRadius, cameraTransform.forward, out RaycastHit hit, sphereCastDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * hit.distance, Color.yellow);
            if (hit.collider.gameObject != currentLookedObject && hit.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactable))
            {
                currentLookedObject = hit.collider.gameObject;
                currentLookedObject.SendMessage("OnLook");
            }
            else if (hit.collider.gameObject != currentLookedObject && currentLookedObject != null && currentLookedObject.TryGetComponent<Interactable>(out interactable))
            {
                currentLookedObject.SendMessage("OnLookAway");
                currentLookedObject = null;
            }
        }
        else
        {
            if (currentLookedObject != null && currentLookedObject.TryGetComponent<Interactable>(out Interactable interactable))
            {
                currentLookedObject.SendMessage("OnLookAway");
            }
            currentLookedObject = null;
        }
    }

    // Called when the player presses the interact button
    // If the player is looking at an interactable object, it sends a message to that object
    void OnInteract(InputAction.CallbackContext context)
    {
        if (currentLookedObject != null && currentLookedObject.TryGetComponent<Interactable>(out Interactable interactable))
        {
            currentLookedObject.SendMessage("OnInteract");
        }
    }
}
