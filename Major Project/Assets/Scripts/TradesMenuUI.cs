using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TradesMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    [SerializeField] private RectTransform content;

    private InputAction scrollAction;

    private bool menuOpen = false;

    private float toggleAnimationTime = 0.3f;

    private float scrollAnimationTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to input action events
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Player.OpenTradesMenu.name].performed += ToggleTradesMenu;
        scrollAction = GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.TradesMenu.Scroll.name];
        scrollAction.performed += ScrollTradeMenuAction;
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.TradesMenu.Close.name].performed += ToggleTradesMenu;
    }

    private void OnDisable()
    {
        // Unsubscribe form input action events
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Player.OpenTradesMenu.name].performed -= ToggleTradesMenu;
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.TradesMenu.Scroll.name].performed -= ScrollTradeMenuAction;
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.TradesMenu.Close.name].performed -= ToggleTradesMenu;
    }

    private void ToggleTradesMenu(InputAction.CallbackContext obj)
    {
        if (LeanTween.isTweening(parentObject))
            return;

        SoundSystem.Instance.PlayEffect(GRefs.Instance.MenuOpenSound, GRefs.Instance.MenuOpenSoundVolume);

        int targetPosition = menuOpen ? 400 : 0;

        if (!menuOpen)
            GRefs.Instance.PlayerInput.SwitchCurrentActionMap(GRefs.Instance.TradesMenuActionMap);

        LeanTween.moveLocalX(parentObject, targetPosition, toggleAnimationTime).setEaseInOutSine().setOnComplete(() =>
        {
            if (menuOpen)
                GRefs.Instance.PlayerInput.SwitchCurrentActionMap(GRefs.Instance.PlayerActionMap);
            menuOpen = !menuOpen;
        });
    }

    IEnumerator ScrollActionHeld()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(scrollAnimationTime);
        if (scrollAction.ReadValue<float>() != 0)
        {
            ScrollTradeMenu(scrollAction.ReadValue<float>());
            StartCoroutine(ScrollActionHeld());
        }
    }

    private void ScrollTradeMenuAction(InputAction.CallbackContext context)
    {
        ScrollTradeMenu(context.ReadValue<float>());
        StopAllCoroutines();
        StartCoroutine(ScrollActionHeld());
    }

    private void ScrollTradeMenu(float value)
    {
        if (LeanTween.isTweening(content.gameObject))
            return;

        float targetScrollPosition = content.offsetMax.y - (value * 77.5f);

        targetScrollPosition = Mathf.Clamp(targetScrollPosition, 0f, 2247.5f);

        SoundSystem.Instance.PlayEffect(GRefs.Instance.MenuMoveSelectionSound, 0.6f);

        LeanTween.value(content.gameObject, content.offsetMax.y, targetScrollPosition, scrollAnimationTime).setEaseInOutSine().setOnUpdate((float value) =>
        {
            content.offsetMax = new Vector2(content.offsetMax.x, value);
        });
    }
}
