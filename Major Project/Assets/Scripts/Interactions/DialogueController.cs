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
    [SerializeField] private CollectibleItemData collectibleItemData;

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
        }

        ConversationManager.Instance.SetBool("HasCollectible", CollectibleStorage.Instance.HasItem(collectibleItemData));
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
