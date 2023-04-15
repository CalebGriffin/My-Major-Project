using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DialogueEditor;
using NaughtyAttributes;

[RequireComponent(typeof(Outline), typeof(Collider))]
public class TimeMachine : Interactable
{
    public static TimeMachine Instance;
    [SerializeField] private Transform innerParent;
    private float rotationTime = 5f;
    private float rotationTimeAdjustment = 2f;

    private float animationTime = 0.5f;

    [SerializeField] private Outline outline;
    [SerializeField] private NPCConversation conversation;

    public bool EndingAchieved = false;
    public bool TimeMachineKept = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomlyRotateInnerParentLoop();

        if (!EndingAchieved || !TimeMachineKept)
            gameObject.SetActive(false);
    }

    private void RandomlyRotateInnerParentLoop()
    {
        Vector3 randomRotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        LeanTween.rotateLocal(innerParent.gameObject, randomRotation, rotationTime).setEaseInOutQuad().setOnComplete(RandomlyRotateInnerParentLoop);
    }

    override protected void OnLook(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;
        
        outline.enabled = true;
        rotationTime /= rotationTimeAdjustment;
    }

    protected override void OnLookAway(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;
        
        outline.enabled = false;
        rotationTime *= rotationTimeAdjustment;
    }

    protected override void OnInteract(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;

        if (EndingAchieved)
            return;
        
        GRefs.Instance.PlayerInput.SwitchCurrentActionMap(GRefs.Instance.DialogueActionMap);
        // Start the conversation
        ConversationManager.Instance.CurrentNPC = ConversationManager.eNPC.NONE;
        ConversationManager.Instance.StartConversation(conversation);
        SubscribeToDialogueActions();

        // Ending achieved
        EndingAchieved = true;
        SaveSystem.Instance.UpdateEndingAchieved();
    }

    [Button]
    public void AnimateIn()
    {
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        LeanTween.scale(gameObject, Vector3.one, animationTime).setEaseOutBack();
    }

    public void AnimateOut()
    {
        LeanTween.scale(gameObject, Vector3.zero, animationTime).setEaseInBack().setOnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void KeepTimeMachine(bool keep)
    {
        TimeMachineKept = keep;
        SaveSystem.Instance.UpdateTimeMachineKept();
        if (!keep)
            AnimateOut();
    }

    private void SubscribeToDialogueActions()
    {
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Dialogue.NextOption.name].performed += NextOption;
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Dialogue.PrevOption.name].performed += PrevOption;
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Dialogue.SelectOption.name].performed += SelectOption;
        ConversationManager.OnConversationEnded += UnsubscribeFromDialogueActions;
    }

    private void UnsubscribeFromDialogueActions()
    {
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Dialogue.NextOption.name].performed -= NextOption;
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Dialogue.PrevOption.name].performed -= PrevOption;
        GRefs.Instance.PlayerInput.actions[GRefs.Instance.PlayerControls.Dialogue.SelectOption.name].performed -= SelectOption;
        ConversationManager.OnConversationEnded -= UnsubscribeFromDialogueActions;
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
}
