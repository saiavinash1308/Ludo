using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FielderManager : MonoBehaviour
{
    public SphereCollider sphereCollider;
    public Rigidbody rb;
    public float speed;
    private Transform ballTransform;
    private bool moveTowardsBall = false;

    [Header("Animator")]
    public Animator animator; // Reference to Animator component

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing!");
        }
    }

    void Update()
    {
        // If moveTowardsBall is true, continuously move the player towards the ball
        if (moveTowardsBall && ballTransform != null)
        {
            Vector3 direction = (ballTransform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

            // Set the Animator parameter to true while moving towards the ball
            animator.SetBool("Fielder Throw", true);
        }
        else
        {
            // Set the Animator parameter to false when not moving towards the ball
            animator.SetBool("Fielder Throw", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Start moving towards the ball when it enters the trigger
        if (other.gameObject.tag == "Ball")
        {
            ballTransform = other.transform;
            moveTowardsBall = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Stop moving towards the ball when it exits the trigger
        if (other.gameObject.tag == "Ball")
        {
            moveTowardsBall = false;
            ballTransform = null;
        }
    }
}
