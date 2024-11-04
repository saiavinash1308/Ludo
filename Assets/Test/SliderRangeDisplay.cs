using CricketBowlingAnimations;
using UnityEngine;
using UnityEngine.UI; // Use this if you're using the regular UI Text
// using TMPro; // Uncomment if you're using TextMeshPro

public class SliderRangeDisplay : MonoBehaviour
{
    public Slider slider; // Reference for the UI Slider component
    public Text displayText; // Reference for the UI Text component
    // public TMP_Text displayText; // Uncomment if you're using TextMeshPro
    public Animator animator; // Reference for the Animator component
    public Transform player; // Reference to the player Transform

    // Adjust this to control how fast the slider moves
    public float speed = 1f;

    private bool animationTriggered = false; // Flag to control animation triggering
    private Vector3 originalPosition; // Store the original position of the player
    public BallControllerScript Ballcontroller;
    public CanvasManagerScript CanvasManager;
    public AnimationTester AnimationTester;

    void Start()
    {
        // Set initial value of the slider
        slider.value = 0f;
        // Store the original position of the player
        originalPosition = player.position;

        // Start the coroutine to update the slider value continuously
        StartCoroutine(UpdateSliderValue());
    }

    private System.Collections.IEnumerator UpdateSliderValue()
    {
        while (true) // Continuous loop
        {
            // Increment the slider value over time
            slider.value += Time.deltaTime * speed; // Use speed to control the increment rate

            // Loop back to 0 after reaching 1
            if (slider.value >= 1f) // Use >= to ensure it resets at 1
            {
                slider.value = 0f;
            }

            // Update the UI text
            displayText.text = slider.value.ToString("F2"); // Show with 2 decimal places

            yield return null; // Wait until the next frame
        }
    }

    void Update()
    {
        // Check for screen tap to run the animation based on the current slider value
        if (Input.GetMouseButtonDown(0)) // For mouse or touch input
        {
            TriggerAnimationBasedOnValue();
        }
    }

    private void TriggerAnimationBasedOnValue()
    {
        if (!animationTriggered) // Only trigger if not already playing
        {
            animationTriggered = true; // Set the flag to true

            // Determine which animation to play
            if (slider.value < 0.30f)
            {
                // animator.Play("Bowling"); // Replace with your animation name
                AnimationTester.PlayBowlingAnimation();
                Ballcontroller.OnThrowButtonClick();
                CanvasManager.OnStraightBallButton();
                Debug.Log("StraightShot");  
            }
            else if (slider.value < 0.60f)
            {
                //  animator.Play("Bowling"); // Replace with your animation name
                AnimationTester.PlayBowlingAnimation();
                Ballcontroller.OnThrowButtonClick();
                CanvasManager.OnLegSpinBallButton();
                Debug.Log("LegShot");
            }
            else
            {
                // animator.Play("Bowling"); // Replace with your animation name
                AnimationTester.PlayBowlingAnimation();
                Ballcontroller.OnThrowButtonClick();
                CanvasManager.OnOffSpinBallButton();
                Debug.Log("OffShot");
            }

            StartCoroutine(ResetAfterAnimation()); // Reset the player position and flag after animation
        }
    }

    private System.Collections.IEnumerator ResetAfterAnimation()
    {
        // Wait for a short duration (adjust based on animation length)
        yield return new WaitForSeconds(1f); // Adjust this duration to match your animation length

        // Reset the player position to the original position
        player.position = originalPosition;

        // Stop the current animation (if necessary)
        animator.Play("Idle"); // Replace with your idle animation state or appropriate state

        animationTriggered = false; // Reset the flag to allow new animation triggers

    }
}
