using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using DialogueEditor;
using NaughtyAttributes;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private InputAction moveSelection;
    private InputAction select;
    private InputAction slider;
    private InputAction back;

    [SerializeField] private NPCConversation starterConversation;

    [SerializeField] private GameObject reticleCanvas;
    [SerializeField] private NewFirstPersonController playerController;
    [SerializeField] private PostProcessVolume postProcessVolume;
    private DepthOfField depthOfField;
    [SerializeField] private RectTransform titleText;
    [SerializeField] private GameObject buttonParent;
    [SerializeField] private GameObject controlsParent;
    [SerializeField] private Slider controlsSlider;
    [SerializeField] private RectTransform[] arrows;
    [SerializeField] private TextMeshProUGUI[] buttonTexts;
    private int arrowIndex = 0;
    private float arrowAnimationTime = 0.1f;

    private bool controlsOpen = false;
    private bool gameStartedBefore = false;

    private float apertureOffValue = 0.1f;
    private float apertureOnValue = 2.5f;
    private float focalLengthOffValue = 1f;
    private float focalLengthOnValue = 25f;

    private float animationTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        depthOfField = postProcessVolume.profile.GetSetting<DepthOfField>();
        reticleCanvas.SetActive(false);

        // Get the input actions
        moveSelection = GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.MainMenu.MoveSelection.name];
        moveSelection.performed += MoveSelection;

        select = GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.MainMenu.Select.name];
        select.performed += ArrowAnimation;

        slider = GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.ControlsMenu.Slider.name];
        slider.performed += Slider;

        back = GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.ControlsMenu.Back.name];
        back.performed += ControlsToggle;

        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Player.OpenMainMenu.name].performed += FadeMenuIn;

        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.MainMenu.Continue.name].performed += FadeMenuOut;
    }

    private void OnDisable()
    {
        // Unsubscribe from the input actions
        moveSelection.performed -= MoveSelection;
        select.performed -= ArrowAnimation;
        slider.performed -= Slider;
        back.performed -= ControlsToggle;
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Player.OpenMainMenu.name].performed -= FadeMenuIn;
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.MainMenu.Continue.name].performed -= FadeMenuOut;
    }

    [Button]
    private void FadeMenuIn(InputAction.CallbackContext context)
    {
        // Switch to the main menu action map
        GRefs.Instance.PlayerInput.SwitchCurrentActionMap(GRefs.Instance.MainMenuActionMap);

        // Hide the reticle
        reticleCanvas.SetActive(false);

        // Update Depth of Field values
        depthOfField.active = true;
        LeanTween.value(gameObject, apertureOffValue, apertureOnValue, animationTime).setOnUpdate((float val) =>
        {
            depthOfField.aperture.value = val;
        });
        LeanTween.value(gameObject, focalLengthOffValue, focalLengthOnValue, animationTime).setOnUpdate((float val) =>
        {
            depthOfField.focalLength.value = val;
        });

        // Update the UI elements
        LeanTween.move(titleText, new Vector3(0, -100, 0), animationTime).setEaseOutBack();
        LeanTween.moveLocalY(buttonParent, -75, animationTime).setEaseOutQuad();

        SoundSystem.Instance.PlayEffect(GRefs.Instance.MenuOpenSound, GRefs.Instance.MainMenuOpenSoundVolume);
    }

    [Button]
    private void FadeMenuOut(InputAction.CallbackContext context)
    {
        if (!gameStartedBefore) return;

        SoundSystem.Instance.PlayEffect(GRefs.Instance.MenuOpenSound, GRefs.Instance.MainMenuOpenSoundVolume);

        // Update Depth of Field values
        depthOfField.active = true;
        LeanTween.value(gameObject, apertureOnValue, apertureOffValue, animationTime).setOnUpdate((float val) =>
        {
            depthOfField.aperture.value = val;
        });
        LeanTween.value(gameObject, focalLengthOnValue, focalLengthOffValue, animationTime).setOnUpdate((float val) =>
        {
            depthOfField.focalLength.value = val;
        }).setOnComplete(() =>
        {
            depthOfField.active = false;

            if (SaveSystem.Instance.GameHasBeenPlayedBefore)
            {
                // Switch to the player action map
                GRefs.Instance.PlayerInput.SwitchCurrentActionMap(GRefs.Instance.PlayerActionMap);
            }
            else
            {
                // Switch to the dialogue action map
                GRefs.Instance.PlayerInput.SwitchCurrentActionMap(GRefs.Instance.DialogueActionMap);
                SaveSystem.Instance.GameHasBeenPlayedBefore = true;
                // Start the conversation
                ConversationManager.Instance.CurrentNPC = ConversationManager.eNPC.NONE;
                ConversationManager.Instance.StartConversation(starterConversation);
                SubscribeToDialogueActions();
            }

            // Show the reticle
            reticleCanvas.SetActive(true);
        });

        // Update the UI elements
        LeanTween.move(titleText, new Vector3(0, 100, 0), animationTime).setEaseInBack();
        LeanTween.moveLocalY(buttonParent, -600, animationTime).setEaseInQuad();
    }

    private void ArrowAnimation(InputAction.CallbackContext context)
    {
        LeanTween.value(gameObject, 10, 15, arrowAnimationTime).setOnUpdate((float val) =>
        {
            arrows[arrowIndex].offsetMax = new Vector2(-val, arrows[arrowIndex].offsetMax.y);
            arrows[arrowIndex].offsetMin = new Vector2(val, arrows[arrowIndex].offsetMin.y);
        }).setEaseInOutSine().setOnComplete(() =>
        {
            SoundSystem.Instance.PlayEffect(GRefs.Instance.MenuSelectSound);
            LeanTween.value(gameObject, 15, 10, arrowAnimationTime).setOnUpdate((float val) =>
            {
                arrows[arrowIndex].offsetMax = new Vector2(-val, arrows[arrowIndex].offsetMax.y);
                arrows[arrowIndex].offsetMin = new Vector2(val, arrows[arrowIndex].offsetMin.y);
            }).setEaseInOutSine().setOnComplete(() =>
            {
                switch(arrowIndex)
                {
                    case 0:
                        Continue();
                        break;
                    case 1:
                        NewGame();
                        break;
                    case 2:
                        ControlsToggle(new InputAction.CallbackContext {});
                        break;
                    case 3:
                        Quit();
                        break;
                    default:
                        break;
                }
            });
        });
    }

    private void MoveSelection(InputAction.CallbackContext context)
    {
        SoundSystem.Instance.PlayEffect(GRefs.Instance.MenuMoveSelectionSound);
        arrowIndex = (int)Mathf.Repeat(arrowIndex - context.ReadValue<float>(), arrows.Length);
        for (int i = 0; i < arrows.Length; i++)
        {
            if (i == arrowIndex)
            {
                arrows[i].gameObject.SetActive(true);
                if (ColorUtility.TryParseHtmlString("#9B9FFE", out Color color))
                    buttonTexts[i].color = color;
                buttonTexts[i].fontStyle = FontStyles.Bold;
            }
            else
            {
                arrows[i].gameObject.SetActive(false);
                buttonTexts[i].color = Color.white;
                buttonTexts[i].fontStyle = FontStyles.Normal;
            }
        }
    }

    private void Slider(InputAction.CallbackContext context)
    {
        float inputVal = context.ReadValue<float>() / 10;
        controlsSlider.value += inputVal;
        playerController.mouseSensitivity = controlsSlider.value;
    }

    private void Continue()
    {
        gameStartedBefore = true;
        FadeMenuOut(new InputAction.CallbackContext {});
    }

    private void NewGame()
    {
        // Delete the save file
        SaveSystem.Instance.DeleteSave();

        // Delete the time file
        TimeSystem.Instance.DeleteTimeData();

        // Reset player position and rotation
        GRefs.Instance.PlayerTransform.position = new Vector3(35, 1.3f, -35);
        GRefs.Instance.PlayerTransform.rotation = Quaternion.identity;
        GRefs.Instance.PlayerCameraTransform.rotation = Quaternion.identity;

        // Find all the collectibles and reset them
        CollectibleItem[] collectibles = FindObjectsOfType<CollectibleItem>(true);
        foreach (CollectibleItem collectible in collectibles)
        {
            collectible.ItemData.Reset();
        }

        Item[] items = FindObjectsOfType<Item>(true);
        foreach (Item item in items)
        {
            print(item.gameObject.name);
            item.Reset();
        }

        TimeMachine timeMachine = FindObjectOfType<TimeMachine>(true);
        timeMachine.EndingAchieved = false;
        timeMachine.TimeMachineKept = false;

        Continue();
    }

    private void ControlsToggle(InputAction.CallbackContext context)
    {
        if (controlsOpen)
        {
            controlsOpen = false;
            GRefs.Instance.PlayerInput.SwitchCurrentActionMap(GRefs.Instance.MainMenuActionMap);
            LeanTween.moveLocalX(controlsParent, 800, animationTime).setEaseInQuad();
            LeanTween.moveLocalX(buttonParent, 0, animationTime).setEaseInQuad();
        }
        else
        {
            controlsOpen = true;
            GRefs.Instance.PlayerInput.SwitchCurrentActionMap(GRefs.Instance.ControlsMenuActionMap);
            LeanTween.moveLocalX(controlsParent, 250, animationTime).setEaseInQuad();
            LeanTween.moveLocalX(buttonParent, -250, animationTime).setEaseInQuad();
        }
    }

    private void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    // Dialogue stuff
    private void SubscribeToDialogueActions()
    {
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Dialogue.SelectOption.name].performed += SelectOption;
        ConversationManager.OnConversationEnded += UnsubscribeFromDialogueActions;
    }

    private void UnsubscribeFromDialogueActions()
    {
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Dialogue.SelectOption.name].performed -= SelectOption;
        ConversationManager.OnConversationEnded -= UnsubscribeFromDialogueActions;
    }

    private void SelectOption(InputAction.CallbackContext context)
    {
        ConversationManager.Instance.PressSelectedOption();
    }
}
