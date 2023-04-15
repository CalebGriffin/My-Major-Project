using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private float rotationSpeed = 35f;

    private float walkSpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        Loop();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Loop()
    {
        // Trigger the idle animation
        SetIdleAnim();

        LeanTween.delayedCall(Random.Range(1, 6), () => 
        {
            // Pick a random rotation
            float rotationAngle = Random.Range(0, 360);

            // Calculate the time it will take to rotate
            float rotationTime = Mathf.Abs(rotationAngle - transform.eulerAngles.y) / rotationSpeed;

            // Rotate
            LeanTween.rotateY(gameObject, rotationAngle, rotationTime).setEaseInOutQuad().setOnComplete(() => 
            {
                // Choose a random action
                int action = Random.Range(0, 3);

                switch (action)
                {
                    // Walk in place
                    case 0:
                        anim.SetBool("Walk", true);
                        break;
                    
                    // Eat
                    case 1:
                        anim.SetBool("Eat", true);
                        break;

                    // Turn head
                    case 2:
                        anim.SetBool("Turn Head", true);
                        break;
                }

                LeanTween.delayedCall(Random.Range(1, 4), () =>
                {
                    // Loop
                    Loop();
                });
            });
        });
    }

    private void SetIdleAnim()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
        anim.SetBool("Eat", false);
        anim.SetBool("Turn Head", false);
    }
}
