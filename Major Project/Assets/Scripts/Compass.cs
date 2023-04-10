using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Compass : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerControls playerControls;

    private new RectTransform transform;
    [SerializeField] private RectTransform compassTransform;

    private bool isVisible = false;

    private float compassHideX = -100;
    private float compassShowX = 100;

    private float compassHideTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<RectTransform>();

        playerInput = GRefs.Instance.PlayerInput;
        playerControls = GRefs.Instance.PlayerControls;
    }

    // Update is called once per frame
    void Update()
    {
        // Either show or hide the compass depending on the player's current action map
        // The compass should only be visible if the current action map is player controls
        if (playerInput.currentActionMap.name != GRefs.Instance.PlayerActionMap && isVisible && !LeanTween.isTweening(compassTransform.gameObject))
        {
            HideCompass();
        }
        else if (playerInput.currentActionMap.name == GRefs.Instance.PlayerActionMap && !isVisible && !LeanTween.isTweening(compassTransform.gameObject))
        {
            ShowCompass();
        }

        if (!isVisible)
            return;

        // Rotate the compass to match the player's rotation
        transform.localRotation = Quaternion.Euler(0, 0, GRefs.Instance.Player.transform.eulerAngles.y);
    }

    void HideCompass()
    {
        LeanTween.moveX(compassTransform.gameObject, compassHideX, compassHideTime).setOnComplete(() => isVisible = false);
    }

    void ShowCompass()
    {
        LeanTween.moveX(compassTransform.gameObject, compassShowX, compassHideTime).setOnComplete(() => isVisible = true);
    }
}
