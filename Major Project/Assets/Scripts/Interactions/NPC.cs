using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline), typeof(DialogueController))]
public class NPC : Interactable
{
    [SerializeField] private Outline outline;
    [SerializeField] private DialogueController dialogueController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnLook(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;
        
        outline.enabled = true;
    }

    protected override void OnLookAway(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;

        outline.enabled = false;
    }

    protected override void OnInteract(int id)
    {
        if (id != gameObject.GetInstanceID())
            return;
        
        dialogueController.StartConversation();
    }
}
