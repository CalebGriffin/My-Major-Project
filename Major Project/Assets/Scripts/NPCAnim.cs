using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using DialogueEditor;

public class NPCAnim : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private const string IDLE = "IDLE";
    private const string TALKING = "TALKING";
    private float originalYRotation;

    [SerializeField] private float rotationOffset = 90f;

    private float lookAtPlayerSpeed = 0.5f;
    private float lookWeight = 0f;

    private bool isLookingAtPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        originalYRotation = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (rotationOffset == 180 && isLookingAtPlayer)
        {
            anim.SetLookAtPosition(GRefs.Instance.NPCLookTarget.position);
            anim.SetLookAtWeight(lookWeight, 0.5f, 1f, 1f, 0.5f);
        }
    }

    public void LookAtPlayer()
    {
        // Stop all tweens
        LeanTween.cancel(gameObject);

        isLookingAtPlayer = true;

        // Rotate to face player
        LeanTween.rotateY(gameObject, GRefs.Instance.PlayerTransform.eulerAngles.y - rotationOffset, lookAtPlayerSpeed).setEaseInOutQuad();
        LeanTween.value(gameObject, 0, 1, lookAtPlayerSpeed).setOnUpdate((float val) => lookWeight = val).setEaseInOutQuad();

        SubscribeToTalkingEvents();
    }

    public void StopLookingAtPlayer()
    {
        // Stop all tweens
        LeanTween.cancel(gameObject);

        LeanTween.value(gameObject, 1, 0, lookAtPlayerSpeed).setOnUpdate((float val) => lookWeight = val).setEaseInOutQuad().setOnComplete(() => isLookingAtPlayer = false);

        // Rotate back to original rotation
        LeanTween.delayedCall(Random.Range(1,3), () =>
        {
            LeanTween.rotateY(gameObject, originalYRotation, lookAtPlayerSpeed * 5).setEaseInOutQuad();
        });

        UnsubscribeFromTalkingEvents();
    }

    public void SubscribeToTalkingEvents()
    {
        ConversationManager.OnDialogueStarted += OnStartTalking;
        ConversationManager.OnDialogueEnded += OnStopTalking;
    }

    public void UnsubscribeFromTalkingEvents()
    {
        ConversationManager.OnDialogueStarted -= OnStartTalking;
        ConversationManager.OnDialogueEnded -= OnStopTalking;
    }

    public void OnStartTalking()
    {
        anim.CrossFade(TALKING, 0.1f);
    }

    public void OnStopTalking()
    {
        anim.CrossFade(IDLE, 0.1f);
    }
}
