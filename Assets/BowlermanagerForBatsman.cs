using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CricketBowlingAnimations;

public class BowlermanagerForBatsman : MonoBehaviour
{
    public static BowlermanagerForBatsman instance;
    // References to other components/scripts
    private AnimationTester animationTester;
    private BallControllerScriptForBatsman ballController;
    private Animator animator;

    [Header("Button References")]
    [SerializeField] private Button straightButton;
    [SerializeField] private Button legSpinButton;
    [SerializeField] private Button offSpinButton;

    public bool straightButtonEnable = false;
    public bool OffspinButtonEnable = false;
    public bool LegspinnButtonEnable = false;

    [Header("GameObjects")]
    public GameObject BowlerCamera;
    public GameObject Bowler; // Reference to the bowler GameObject

    public bool isLeftSide = true; // Track which side the bowler is on, default to left side

    private Vector3 leftBowlerPosition;  // Initial position of the bowler on the left side
    private Vector3 rightBowlerPosition; // Initial position of the bowler on the right side

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get references to the required components
        animationTester = GetComponent<AnimationTester>();
        ballController = FindObjectOfType<BallControllerScriptForBatsman>();
        animator = GetComponent<Animator>();

        if (animationTester == null || ballController == null || animator == null)
        {
            Debug.LogError("Required components are not assigned.");
            return;
        }

        // Assign button click events
        straightButton.onClick.AddListener(() => OnStraightButtonPressed());
        legSpinButton.onClick.AddListener(() => OnLegSpinButtonPressed());
        offSpinButton.onClick.AddListener(() => OnOffSpinButtonPressed());

        // Initialize bowler positions
        leftBowlerPosition = Bowler.transform.position;
        rightBowlerPosition = new Vector3(-leftBowlerPosition.x, leftBowlerPosition.y, leftBowlerPosition.z);

        BowlerCamera.SetActive(false);
        GamemanagerForBatsMan.instance.ResetButtons();
    }

    void Update()
    {
        // Check if left or right arrow key is pressed to switch sides
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!isLeftSide) // If currently on the right side
            {
                SwitchSide(); // Switch to the left side
                isLeftSide = true; // Update the state after switching
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isLeftSide) // If currently on the left side
            {
                SwitchSide(); // Switch to the right side
                isLeftSide = false; // Update the state after switching
            }
        }


    }

    private void OnStraightButtonPressed()
    {
        // Set the position for Straight Ball
        animationTester.SetBowlerPosition(new Vector3(530f, 398.4958f, -30f)); // Example position for Straight
        animationTester.bowlingAnimation = AnimationTester.BowlingAnimation.RightArmMediumFastBowler;
        BowlerCamera.SetActive(true);

        straightButtonEnable = true;
        OffspinButtonEnable = false;
        LegspinnButtonEnable = false;
    }

    private void OnLegSpinButtonPressed()
    {
        // Set the position for Leg Spin
        animationTester.SetBowlerPosition(new Vector3(529.89f, 398.5f, -4f)); // Example position for Leg Spin
        animationTester.bowlingAnimation = AnimationTester.BowlingAnimation.RightArmLegSpinner;
        straightButtonEnable = false;
        OffspinButtonEnable = false;
        LegspinnButtonEnable = true;
        BowlerCamera.SetActive(false);
    }

    private void OnOffSpinButtonPressed()
    {
        // Set the position for Off Spin
        animationTester.SetBowlerPosition(new Vector3(529.89f, 398.5f, -2f)); // Example position for Off Spin
        animationTester.bowlingAnimation = AnimationTester.BowlingAnimation.RightArmOffSpinner;

        straightButtonEnable = false;
        OffspinButtonEnable = true;
        LegspinnButtonEnable = false;
        BowlerCamera.SetActive(false);
    }

    public void SwitchSide()
    {
        // Switch the bowler's position
        if (isLeftSide)
        {
            Bowler.transform.position = rightBowlerPosition;
            isLeftSide = false;
        }
        else
        {
            Bowler.transform.position = leftBowlerPosition;
            isLeftSide = true;
        }

        // Optionally, update any additional state or notify other components here
    }
}
