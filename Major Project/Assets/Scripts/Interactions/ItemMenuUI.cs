using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ItemMenuUI : MonoBehaviour
{
    private enum CurrentSection
    {
        Inventory,
        Tools,
        Collectibles
    };

    private CurrentSection currentSection = CurrentSection.Inventory;

    private PlayerInput playerInput;
    private PlayerControls playerControls;
    private InputAction itemMenuEnableAction;
    private InputAction itemMenuDisableAction;

    private Camera mainCamera;

    private bool menuOpen = false;

    private float itemMenuAnimationTime = 0.5f;

    [SerializeField] private GameObject itemMenuObject;

    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ToolsUI toolsUI;
    [SerializeField] private CollectiblesUI collectiblesUI;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GRefs.Instance.PlayerInput;
        playerControls = GRefs.Instance.PlayerControls;
        itemMenuEnableAction = playerInput.actions[playerControls.Player.ItemMenu.name];
        itemMenuEnableAction.performed += ctx => ItemMenuToggle();
        itemMenuDisableAction = playerInput.actions[playerControls.UI.ItemMenu.name];
        itemMenuDisableAction.performed += ctx => ItemMenuToggle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
    }

    private void ItemMenuToggle()
    {
        if (menuOpen)
        {
            menuOpen = false;
            string newActionMap = playerControls.Player.ToString().Replace("PlayerControls+", "").Replace("Actions", "");
            playerInput.SwitchCurrentActionMap(newActionMap);
            AnimateItemMenuUIOut();
        }
        else
        {
            menuOpen = true;
            // Change the action map
            string newActionMap = playerControls.UI.ToString().Replace("PlayerControls+", "").Replace("Actions", "");
            Debug.Log(newActionMap);
            playerInput.SwitchCurrentActionMap(newActionMap);
            //!inventoryUI.UpdateInventoryUI();
            AnimateItemMenuUIIn();
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
        LeanTween.scale(itemMenuObject, Vector3.zero, itemMenuAnimationTime).setEaseInBack();
    }
}
