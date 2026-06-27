using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool IsWalking = Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d");
        bool runPressed = Input.GetKey("left shift");

        animator.SetBool("IsWalking", IsWalking);

        bool IsRunning = animator.GetBool("IsRunning");

        if (!IsRunning && IsWalking && runPressed)
        {
            animator.SetBool("IsRunning", true);
        }

        if (IsRunning && (!IsWalking || !runPressed))
        {
            animator.SetBool("IsRunning", false);
        }

        if (Input.GetKey("space"))
        {
            animator.SetBool("Jumping", true);
        }

        if (!Input.GetKey("space"))
        {
            animator.SetBool("Jumping", false);
        }


    }
}
