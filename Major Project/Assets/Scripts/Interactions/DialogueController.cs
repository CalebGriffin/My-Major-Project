using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DialogueEditor;

public class DialogueController : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerControls playerControls;
    private InputAction nextOptionAction;
    private InputAction prevOptionAction;
    private InputAction selectOptionAction;
    private InputAction endConversationAction;

    [SerializeField] private NPCConversation introConversation;
    [SerializeField] private NPCConversation mainConversation;
    private bool spokenToPlayer = false;
    [SerializeField] private int responseCount = 0;
    [SerializeField] private int hintCount = 0;
    [SerializeField] private int chatCount = 0;

    [SerializeField] private Transaction[] transactions;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GRefs.Instance.PlayerInput;
        playerControls = GRefs.Instance.PlayerControls;

        nextOptionAction = playerInput.actions[playerControls.Dialogue.NextOption.name];
        nextOptionAction.performed += NextOption;
        prevOptionAction = playerInput.actions[playerControls.Dialogue.PrevOption.name];
        prevOptionAction.performed += PrevOption;
        selectOptionAction = playerInput.actions[playerControls.Dialogue.SelectOption.name];
        selectOptionAction.performed += SelectOption;
        endConversationAction = playerInput.actions[playerControls.Dialogue.EndConversation.name];
        endConversationAction.performed += EndConversation;

        ConversationManager.OnConversationEnded += () => playerInput.SwitchCurrentActionMap(GRefs.Instance.PlayerActionMap);
    }

    private void OnDisable()
    {
        nextOptionAction.performed -= NextOption;
        prevOptionAction.performed -= PrevOption;
        selectOptionAction.performed -= SelectOption;
        endConversationAction.performed -= EndConversation;
        ConversationManager.OnConversationEnded -= () => playerInput.SwitchCurrentActionMap(GRefs.Instance.PlayerActionMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartConversation()
    {
        playerInput.SwitchCurrentActionMap(GRefs.Instance.DialogueActionMap);
        ConversationManager.Instance.StartConversation(spokenToPlayer ? mainConversation : introConversation);
        ParameterSetup();
        spokenToPlayer = true;
    }

    private void ParameterSetup()
    {
        if (!spokenToPlayer) return;

        // Pick a random response, hint and chat option
        ConversationManager.Instance.SetInt("ResponseChoice", Random.Range(0, responseCount));
        ConversationManager.Instance.SetInt("HintChoice", Random.Range(0, hintCount));
        ConversationManager.Instance.SetInt("ChatChoice", Random.Range(0, chatCount));

        // Setup the parameters for the transactions
        for (int i = 0; i < transactions.Length; i++)
        {
            bool hasItems = true;

            foreach (Transaction.RequiredItem requiredItem in transactions[i].RequiredItems)
            {
                if (!Inventory.Instance.HasItem(requiredItem.Item, requiredItem.Amount))
                {
                    hasItems = false;
                    break;
                }
            }

            ConversationManager.Instance.SetBool($"HasItems{i}", hasItems);

            switch (transactions[i].RewardItem.Item.Type)
            {
                case ItemData.ItemType.Collectible:
                    ConversationManager.Instance.SetBool($"HasUntradable{i}", CollectibleStorage.Instance.HasItem(transactions[i].RewardItem.Item));
                    break;
                case ItemData.ItemType.Tool:
                    ConversationManager.Instance.SetBool($"HasUntradable{i}", ToolStorage.Instance.HasItem(transactions[i].RewardItem.Item));
                    break;
            }
        }
    }

    public void Trade(int itemIndex)
    {
        transactions[itemIndex].Trade();
    }

    private void NextOption(InputAction.CallbackContext context)
    {
        ConversationManager.Instance.SelectNextOption();
    }

    private void PrevOption(InputAction.CallbackContext context)
    {
        ConversationManager.Instance.SelectPreviousOption();
    }

    private void SelectOption(InputAction.CallbackContext context)
    {
        ConversationManager.Instance.PressSelectedOption();
    }

    private void EndConversation(InputAction.CallbackContext context)
    {
        ConversationManager.Instance.EndConversation();
    }
}
