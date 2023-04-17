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
    private InputAction removeSelectedItemAction;

    private bool menuOpen = false;

    private float itemMenuAnimationTime = 0.5f;

    [SerializeField] private GameObject itemMenuObject;

    [SerializeField] private GameObject toolTipsObject;
    [SerializeField] private GameObject keyboardToolTipsObject;
    [SerializeField] private GameObject gamepadToolTipsObject;
    [SerializeField] private TextMeshProUGUI keyboardDropItemText;
    [SerializeField] private TextMeshProUGUI gamepadDropItemText;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ToolsUI toolsUI;
    [SerializeField] private CollectiblesUI collectiblesUI;

    [SerializeField] private ItemSlot currentItemSlot;

    [SerializeField] private TextMeshProUGUI inventoryCurrentItemText;
    [SerializeField] private TextMeshProUGUI toolsCurrentItemText;
    [SerializeField] private TextMeshProUGUI collectiblesCurrentItemText;

    private Vector3 smallScale = new Vector3(0.001f, 0.001f, 0.001f);

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

        removeSelectedItemAction = playerInput.actions[playerControls.UI.RemoveSelectedItem.name];
        removeSelectedItemAction.performed += RemoveSelectedItem;
    }

    private void OnDisable()
    {
        itemMenuEnableAction.performed -= ItemMenuToggle;
        itemMenuDisableAction.performed -= ItemMenuToggle;
        moveItemSelectionAction.performed -= OnMoveItemActionPerformed;
        removeSelectedItemAction.performed -= RemoveSelectedItem;
    }

    private void ItemMenuToggle(InputAction.CallbackContext context)
    {
        if (menuOpen)
        {
            menuOpen = false;
            toolTipsObject.SetActive(false);
            AnimateItemMenuUIOut();
        }
        else
        {
            menuOpen = true;
            UpdateToolTipScheme();
            MoveItemSelection(Vector2.zero);
            inventoryUI.UpdateInventoryUI();
            collectiblesUI.UpdateCollectiblesUI();
            toolsUI.UpdateToolsUI();
            currentItemSlot.Highlight();
            AnimateItemMenuUIIn();
            playerInput.SwitchCurrentActionMap(GRefs.Instance.UIActionMap);
        }
    }

    private void AnimateItemMenuUIIn()
    {
        itemMenuObject.transform.localScale = smallScale;
        // Animate the item menu UI in
        LeanTween.moveLocalY(itemMenuObject, 0, itemMenuAnimationTime).setEaseOutCirc();
        LeanTween.scale(itemMenuObject, Vector3.one, itemMenuAnimationTime).setEaseOutBack().setOnComplete(() =>
        {
            toolTipsObject.SetActive(true);
        });

        SoundSystem.Instance.PlayEffect(GRefs.Instance.MenuOpenSound, GRefs.Instance.MenuOpenSoundVolume);
    }

    private void AnimateItemMenuUIOut()
    {
        // Animate the item menu UI out
        LeanTween.moveLocalY(itemMenuObject, -1000, itemMenuAnimationTime).setEaseInCirc();
        LeanTween.scale(itemMenuObject, smallScale, itemMenuAnimationTime).setEaseInBack().setOnComplete(() =>
        {
            playerInput.SwitchCurrentActionMap(GRefs.Instance.PlayerActionMap);
        });
        SoundSystem.Instance.PlayEffect(GRefs.Instance.MenuOpenSound, GRefs.Instance.MenuOpenSoundVolume);
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
        if (!menuOpen) return;
        MoveItemSelection(context.ReadValue<Vector2>());
        StopAllCoroutines();
        StartCoroutine(MoveActionHeld());
    }
    private void MoveItemSelection(Vector2 directionVector)
    {
        if (directionVector == Vector2.zero) return;

        currentItemSlot.UnHighlight();

        inventoryCurrentItemText.text = "Current Item:";
        toolsCurrentItemText.text = "Current Item:";
        collectiblesCurrentItemText.text = "Current Item:";

        if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y))
        {
            currentItemSlot = directionVector.x > 0 ? currentItemSlot.Right : currentItemSlot.Left;
        }
        else
        {
            currentItemSlot = directionVector.y > 0 ? currentItemSlot.Up : currentItemSlot.Down;
        }

        SoundSystem.Instance.PlayEffect(GRefs.Instance.MenuMoveSelectionSound);

        currentItemSlot.Highlight();

        if (currentItemSlot.ItemData != null)
        {
            keyboardDropItemText.color = currentItemSlot.ItemData.IsTradeable ? Color.white : new Color(1, 1, 1, 0.3f);
            gamepadDropItemText.color = keyboardDropItemText.color;
        }
        else
        {
            keyboardDropItemText.color = new Color(1, 1, 1, 0.3f);
            gamepadDropItemText.color = keyboardDropItemText.color;
        }
    }

    private void UpdateToolTipScheme()
    {
        if (playerInput.currentControlScheme == playerControls.KeyboardScheme.name)
        {
            keyboardToolTipsObject.SetActive(true);
            gamepadToolTipsObject.SetActive(false);
        }
        else if (playerInput.currentControlScheme == playerControls.GamepadScheme.name)
        {
            keyboardToolTipsObject.SetActive(false);
            gamepadToolTipsObject.SetActive(true);
        }
        else
        {
            keyboardToolTipsObject.SetActive(false);
            gamepadToolTipsObject.SetActive(false);
        }
    }

    private void RemoveSelectedItem(InputAction.CallbackContext context)
    {
        if (currentItemSlot.ItemData == null) return;
        if (!currentItemSlot.ItemData.IsTradeable) return;

        Inventory.Instance.RemoveItem(currentItemSlot.ItemData);
        inventoryUI.UpdateInventoryUI();
        currentItemSlot.Highlight();

          SoundSystem.Instance.PlayEffect(GRefs.Instance.MenuCancelSound, GRefs.Instance.MenuCancelSoundVolume);
    }
}
