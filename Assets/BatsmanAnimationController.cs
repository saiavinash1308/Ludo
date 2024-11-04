using UnityEngine;

public class BattingAnimationController : MonoBehaviour
{
    public static BattingAnimationController instance;
    [Header("Animator")]
    [SerializeField] private Animator animator;  // Assign the Animator component here

    [Header("Animation Parameters")]
    [SerializeField] private string batTrigger = "Bat";  // Trigger for starting the animation
    [SerializeField] private string battingTypeParam = "BattingType"; // Integer parameter to specify type of shot

    [Header("Batting Types")]
    [SerializeField] private int IdleState = 0; // Integer value for idleState
    [SerializeField] public int straightShotType = 1; // Integer value for straight shot
    [SerializeField] public int legShotType = 2;       // Integer value for leg shot
    [SerializeField] public int offShotType = 3;       // Integer value for off shot

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    void Awake()
    {
        instance = this;
        // Store the initial transform values
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Check for input and trigger the corresponding batting animation
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayBattingAnimation(straightShotType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayBattingAnimation(legShotType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayBattingAnimation(offShotType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ResetToIdleState();
        }
    }

    /// <summary>
    /// Triggers the batting animation based on the specified type.
    /// </summary>
    /// <param name="battingType">The integer value for the batting type</param>
    public void PlayBattingAnimation(int battingType)
    {
        // Set the batting type parameter
        animator.SetInteger(battingTypeParam, battingType);

        // Trigger the animation
        animator.SetTrigger(batTrigger);
    }

    /// <summary>
    /// Resets the bat's transform to the initial state.
    /// </summary>
    public void ResetToIdleState()
    {
        // Reset the transform to its initial values
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;

        // Optionally reset the animator parameters
        animator.SetInteger(battingTypeParam, IdleState);
        animator.ResetTrigger(batTrigger);
    }

    /// <summary>
    /// Called by Animation Event when the animation ends.
    /// </summary>
    public void OnAnimationEnd()
    {
        ResetToIdleState();
    }
}
