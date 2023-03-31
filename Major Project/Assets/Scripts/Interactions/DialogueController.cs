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
    [SerializeField] private ConversationManager.eNPC npcName;
    private bool spokenToPlayer = false;
    [SerializeField] private int responseCount = 0;
    [SerializeField] private int hintCount = 0;
    [SerializeField] private int chatCount = 0;
    private int untradableIndex = 0;

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
        ConversationManager.Instance.CurrentNPC = npcName;
        ParameterSetup();
        spokenToPlayer = true;
    }

    public void ParameterSetup()
    {
        if (!spokenToPlayer) return;

        // Pick a random response, hint and chat option
        ConversationManager.Instance.SetInt("ResponseChoice", Random.Range(0, responseCount));
        ConversationManager.Instance.SetInt("HintChoice", Random.Range(0, hintCount));
        ConversationManager.Instance.SetInt("ChatChoice", Random.Range(0, chatCount));

        untradableIndex = 0;

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
                    ConversationManager.Instance.SetBool($"HasUntradable{untradableIndex}", CollectibleStorage.Instance.HasItem(transactions[i].RewardItem.Item));
                    untradableIndex++;
                    break;
                case ItemData.ItemType.Tool:
                    ConversationManager.Instance.SetBool($"HasUntradable{untradableIndex}", ToolStorage.Instance.HasItem(transactions[i].RewardItem.Item));
                    untradableIndex++;
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
        if (npcName != ConversationManager.Instance.CurrentNPC) return;
        ConversationManager.Instance.SelectNextOption();
    }

    private void PrevOption(InputAction.CallbackContext context)
    {
        if (npcName != ConversationManager.Instance.CurrentNPC) return;
        ConversationManager.Instance.SelectPreviousOption();
    }

    private void SelectOption(InputAction.CallbackContext context)
    {
        if (npcName != ConversationManager.Instance.CurrentNPC) return;
        ConversationManager.Instance.PressSelectedOption();
    }

    private void EndConversation(InputAction.CallbackContext context)
    {
        if (npcName != ConversationManager.Instance.CurrentNPC) return;
        ConversationManager.Instance.EndConversation();
    }
}
