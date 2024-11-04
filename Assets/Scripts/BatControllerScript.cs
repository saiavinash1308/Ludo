using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatControllerScript : MonoBehaviour
{
    public static BatControllerScript instance;

    public GameObject ball; // The ball GameObject
    public float batSpeed; // The bat's speed
    public float batElevation; // The bat's elevation angle i.e., the bat's x rotation axis 
    public float boundaryPointX; // Max x value the bat can cover

    public float batsmanReachLimitMin; // The ball can be hit once it is inside this limit
    public float batsmanReachLimitMax; // The ball cannot be hit once it gets outside this limit
    public Vector3 ballsPositionAtHit; // The ball's position when it gets hit by the bat

    private bool isBatSwinged; // Has the bat swung
    public Vector3 defaultPosition; // Bat's default beginning position
    public Quaternion defaultRotation; // Bat's default beginning rotation
    public Animator animator; // Animator component for bat swing animation
    public float custombatX = 528f;

    // Offset between the bat and the batsman (parent)
    public float batOffsetX = 0.5f; // Bat will be 0.5 units in front of the batsman in x direction

    public float BatSpeed { set { batSpeed = value; } }
    public bool IsBatSwinged { set { isBatSwinged = value; } get { return isBatSwinged; } }

    public float BatElevation
    {
        set
        {
            batElevation = value;
            transform.localRotation = Quaternion.Euler(batElevation, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z); // Update the bat's rotation to match the elevation
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Set default position and rotation to the bat's beginning local transform
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localRotation;

        // Get the Animator component attached to the bat
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the ball is in the range for the bat to hit
        if (!isBatSwinged && BallControllerScript.instance.IsBallThrown && ball.transform.position.z <= batsmanReachLimitMax)
        {
            // Move the bat in front of the ball while keeping the batsman behind
            Vector3 newBatPosition = new Vector3(ball.transform.position.x - batOffsetX, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = newBatPosition;
        }

        // Clamp the bat's local position within the pitch width
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, custombatX, boundaryPointX), transform.localPosition.y, transform.localPosition.z);

        // If the bat has swung and the ball is hit by the bat, then update its position to the ball's position at the time of hit
        if (IsBatSwinged && BallControllerScript.instance.IsBallHit)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, ballsPositionAtHit, Time.deltaTime * 20);
        }
    }

    public void HitTheBall(float dragAngle)
    {
        // If the ball is inside the bat's hit range, then hit the ball
        if (ball.transform.position.z >= batsmanReachLimitMin && ball.transform.position.z <= batsmanReachLimitMax)
        {
            AudioManagerScript.instance.PlayBatHitAudio(); // Play the bat hit sound
            ballsPositionAtHit = ball.transform.position; // Set the ballsHitPosition to the ball's position at the time of hit

            // Call the HitBall function of the BallControllerScript and pass it the forward direction of 
            // the bat's transform and the bat's speed
            BallControllerScript.instance.HitTheBall((transform.forward), batSpeed);
            Debug.Log("BALL HAS BEEN HIT");
        }
    }

    public void ResetBat() // Reset the values
    {
        transform.localRotation = defaultRotation; // Reset to default local rotation
        transform.localPosition = defaultPosition; // Reset to default local position
        isBatSwinged = false;
    }
}
