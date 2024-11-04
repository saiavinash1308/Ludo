using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this to work with UI elements like the Slider

public class BatControllerScriptForBatsman : MonoBehaviour
{
    public static BatControllerScriptForBatsman instance;

    public GameObject ball; // The ball GameObject
    public float batElevation; // The bat's elevation angle i.e., the bat's x rotation axis 
    public float boundaryPointX; // Max x value the bat can cover

    public float batsmanReachLimitMin; // The ball can be hit once it is inside this limit
    public float batsmanReachLimitMax; // The ball cannot be hit once it gets outside this limit
    public Vector3 ballsPositionAtHit; // The ball's position when it gets hit by the bat

    private bool isBatSwinged; // Has the bat swung
    public Vector3 defaultPosition; // Bat's default beginning position
    public Quaternion defaultRotation; // Bat's default beginning rotation
    public Animator animator; // Animator component for bat swing animation

    public Slider batMovementSlider; // Reference to the slider controlling bat movement
    public float batMovementRange = 5f; // Range in X-axis the bat can move within

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

        // Ensure the slider starts in the middle
        if (batMovementSlider != null)
        {
            batMovementSlider.value = 0.5f; // Start the slider at the middle value
        }
    }

    void Update()
    {
        // Control bat movement based on slider value
        if (batMovementSlider != null)
        {
            // Map slider value (0 to 1) to the bat's X-axis movement range
            float sliderValue = batMovementSlider.value;

            // Move bat between defaultPosition.x - batMovementRange and defaultPosition.x + batMovementRange
            float batPosX = Mathf.Lerp(defaultPosition.x - batMovementRange, defaultPosition.x + batMovementRange, sliderValue);

            // Apply the new X position to the bat, keeping the Y and Z positions unchanged
            transform.localPosition = new Vector3(batPosX, transform.localPosition.y, transform.localPosition.z);

            // Clamp the bat's local position within the defined boundary (relative to default position)
            float clampedX = Mathf.Clamp(transform.localPosition.x, defaultPosition.x - boundaryPointX, defaultPosition.x + boundaryPointX);
            transform.localPosition = new Vector3(clampedX, transform.localPosition.y, transform.localPosition.z);
        }

        // If the bat has swung and the ball is hit, then update its position to the ball's position at the time of hit
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
            // the bat's transform
            BallControllerScriptForBatsman.instance.HitTheBall((transform.forward), 0); // No speed needed since slider controls movement
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
