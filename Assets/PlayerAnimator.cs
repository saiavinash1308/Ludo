using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public BatControllerScript batController; // Reference to the BatControllerScript
    public GameObject ball; // Reference to the ball

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if the ball is in the range for the player to swing
        if (batController != null && ball != null)
        {
            // Check if the ball is within the range to hit
            if (!batController.IsBatSwinged && BallControllerScript.instance.IsBallThrown &&
                ball.transform.position.z >= batController.batsmanReachLimitMin &&
                ball.transform.position.z <= batController.batsmanReachLimitMax)
            {
                // Trigger player animation to get ready to hit the ball
                animator.SetBool("Shot", true);
            }
            else
            {
                // Reset the player animation if the ball is not in range
                animator.SetBool("Shot", false);
            }
        }
    }
}
