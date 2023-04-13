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

        playerControls = new PlayerControls();
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

    [SerializeField] private PlayerInput playerInput;
    public PlayerInput PlayerInput { get { return playerInput; } }

    [SerializeField] private PlayerControls playerControls;
    public PlayerControls PlayerControls { get { return playerControls; } }

    public string PlayerActionMap { get { return playerControls.Player.ToString().ActionMapStringReplace(); } }

    public string UIActionMap { get { return playerControls.UI.ToString().ActionMapStringReplace(); } }

    public string DialogueActionMap { get { return playerControls.Dialogue.ToString().ActionMapStringReplace(); } }

    public string TimeUIActionMap { get { return playerControls.TimeUI.ToString().ActionMapStringReplace(); } }

    public string MainMenuActionMap { get { return playerControls.MainMenu.ToString().ActionMapStringReplace(); } }

    public string ControlsMenuActionMap { get { return playerControls.ControlsMenu.ToString().ActionMapStringReplace(); } }
}

public static class GRefsExtensions
{
    public static string ActionMapStringReplace(this string str)
    {
        return str.Replace("PlayerControls+", "").Replace("Actions", "");
    }
}
