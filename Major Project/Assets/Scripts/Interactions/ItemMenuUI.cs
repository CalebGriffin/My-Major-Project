using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ItemMenuUI : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerControls playerControls;
    private InputAction itemMenuEnableAction;
    private InputAction itemMenuDisableAction;
    private InputAction moveItemSelectionAction;

    private bool menuOpen = false;

    private float itemMenuAnimationTime = 0.5f;

    [SerializeField] private GameObject itemMenuObject;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ToolsUI toolsUI;
    [SerializeField] private CollectiblesUI collectiblesUI;

    [SerializeField] private ItemSlot currentItemSlot;

    [SerializeField] private TextMeshProUGUI inventoryCurrentItemText;
    [SerializeField] private TextMeshProUGUI toolsCurrentItemText;
    [SerializeField] private TextMeshProUGUI collectiblesCurrentItemText;

    private enum Direction { Up, Down, Left, Right }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GRefs.Instance.PlayerInput;
        playerControls = GRefs.Instance.PlayerControls;

        itemMenuEnableAction = playerInput.actions[playerControls.Player.OpenItemMenu.name];
        itemMenuEnableAction.performed += ItemMenuToggle;
        itemMenuDisableAction = playerInput.actions[playerControls.UI.CloseItemMenu.name];
        itemMenuDisableAction.performed += ItemMenuToggle;

        moveItemSelectionAction = playerInput.actions[playerControls.UI.MoveItemSelection.name];
        moveItemSelectionAction.performed +=  OnMoveItemActionPerformed;
    }

    private void ItemMenuToggle(InputAction.CallbackContext context)
    {
        if (menuOpen)
        {
            menuOpen = false;
            AnimateItemMenuUIOut();
        }
        else
        {
            menuOpen = true;
            inventoryUI.UpdateInventoryUI();
            currentItemSlot.Highlight();
            AnimateItemMenuUIIn();
            string newActionMap = playerControls.UI.ToString().Replace("PlayerControls+", "").Replace("Actions", "");
            playerInput.SwitchCurrentActionMap(newActionMap);
        }
    }

    private void AnimateItemMenuUIIn()
    {
        // Animate the item menu UI in
        LeanTween.moveLocalY(itemMenuObject, 0, itemMenuAnimationTime).setEaseOutCirc();
        LeanTween.scale(itemMenuObject, Vector3.one, itemMenuAnimationTime).setEaseOutBack();
    }

    private void AnimateItemMenuUIOut()
    {
        // Animate the item menu UI out
        LeanTween.moveLocalY(itemMenuObject, -1000, itemMenuAnimationTime).setEaseInCirc();
        LeanTween.scale(itemMenuObject, Vector3.zero, itemMenuAnimationTime).setEaseInBack().setOnComplete(() =>
        {
            string newActionMap = playerControls.Player.ToString().Replace("PlayerControls+", "").Replace("Actions", "");
            playerInput.SwitchCurrentActionMap(newActionMap);
        });
    }

    IEnumerator MoveActionHeld()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.15f);
        if (moveItemSelectionAction.ReadValue<Vector2>() != Vector2.zero)
        {
            MoveItemSelection(moveItemSelectionAction.ReadValue<Vector2>());
            StartCoroutine(MoveActionHeld());
        }
    }

    private void OnMoveItemActionPerformed(InputAction.CallbackContext context)
    {
        MoveItemSelection(context.ReadValue<Vector2>());
        StopAllCoroutines();
        StartCoroutine(MoveActionHeld());
    }
    private void MoveItemSelection(Vector2 directionVector)
    {
        currentItemSlot.UnHighlight();

        inventoryCurrentItemText.text = "Current Item:";
        toolsCurrentItemText.text = "Current Item:";
        collectiblesCurrentItemText.text = "Current Item:";

        if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y))
        {
            switch (directionVector.x)
            {
                case float x when x > 0:
                    currentItemSlot = currentItemSlot.Right;
                    break;
                case float x when x < 0:
                    currentItemSlot = currentItemSlot.Left;
                    break;
            }
        }
        else
        {
            switch (directionVector.y)
            {
                case float y when y > 0:
                    currentItemSlot = currentItemSlot.Up;
                    break;
                case float y when y < 0:
                    currentItemSlot = currentItemSlot.Down;
                    break;
            }
        }

        currentItemSlot.Highlight();
    }
}
