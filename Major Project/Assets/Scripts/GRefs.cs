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
    
    [SerializeField] private Transform npcLookTarget;
    public Transform NPCLookTarget { get { return npcLookTarget; } }

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

    public string TradesMenuActionMap { get { return playerControls.TradesMenu.ToString().ActionMapStringReplace(); } }

    [SerializeField] private AudioClip menuOpenSound;
    public AudioClip MenuOpenSound { get { return menuOpenSound; } }
    private float menuOpenSoundVolume = 0.15f;
    public float MenuOpenSoundVolume { get { return menuOpenSoundVolume; } }
    private float mainMenuOpenSoundVolume = 0.075f;
    public float MainMenuOpenSoundVolume { get { return mainMenuOpenSoundVolume; } }

    [SerializeField] private AudioClip menuMoveSelectionSound;
    public AudioClip MenuMoveSelectionSound { get { return menuMoveSelectionSound; } }

    [SerializeField] private AudioClip menuSelectSound;
    public AudioClip MenuSelectSound { get { return menuSelectSound; } }

    [SerializeField] private AudioClip menuCancelSound;
    public AudioClip MenuCancelSound { get { return menuCancelSound; } }
    private float menuCancelSoundVolume = 0.4f;
    public float MenuCancelSoundVolume { get { return menuCancelSoundVolume; } }
}

public static class GRefsExtensions
{
    public static string ActionMapStringReplace(this string str)
    {
        return str.Replace("PlayerControls+", "").Replace("Actions", "");
    }
}
