using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GRefs : MonoBehaviour
{
    // Creates a singleton instance of this class that can be accessed from anywhere
    public static GRefs Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Player References
    [SerializeField] private GameObject player;
    public GameObject Player { get { return player; } }

    [SerializeField] private Transform playerTransform;
    public Transform PlayerTransform { get { return playerTransform; } }

    [SerializeField] private Camera playerCamera;
    public Camera PlayerCamera { get { return playerCamera; } }

    [SerializeField] private Transform playerCameraTransform;
    public Transform PlayerCameraTransform { get { return playerCameraTransform; } }

    [SerializeField] private PlayerControls playerControls;
    public PlayerControls PlayerControls { get { return playerControls; } }
}
