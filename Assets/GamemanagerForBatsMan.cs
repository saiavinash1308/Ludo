using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamemanagerForBatsMan : MonoBehaviour
{
    public static GamemanagerForBatsMan instance;

    [Header("Button References")]
    [SerializeField] public Button straightButton;
    [SerializeField] public Button legSpinButton;
    [SerializeField] public Button offSpinButton;

    private Button activeButton; // To store the active button

    private int currentIndex = 0;
    public int throwCount = 0; // Count to keep track of the number of balls thrown

    [Header("TextMeshPro References")]
    [SerializeField] private List<TextMeshProUGUI> throwTexts; // List to store TextMeshPro text objects
    [SerializeField] private List<TextMeshProUGUI> boundaryTexts; // List to store TextMeshPro boundary texts for each throw

    [Header("Score Management")]
    public TextMeshProUGUI scoreText;  // TextMeshPro component to display the score
    public int currentScore = 0;      // Current score of the player
    public int highestBoundaryValue = 0;  // To store the highest boundary value reached in the current shot


    [Header("Ball Reference")]
    public GameObject ball;  // Reference to the ball object

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetButtons();
        UpdateScoreText();
    }

    void Update()
    {
        ButtonHandler();
        // DetectBoundaryHit() is removed since we are using triggers
    }

    // Called from the boundary when the ball passes through
    public void UpdateHighestBoundary(int boundaryValue)
    {
        // Update the highest boundary value if the new one is larger
        if (boundaryValue > highestBoundaryValue)
        {
            highestBoundaryValue = boundaryValue;
        }
    }

    // Call this to finalize the score and reset the highest boundary for the next ball
    public void ResetHighestBoundaryAndFinalizeScore()
    {
        // Add the highest boundary value to the current score
        currentScore += highestBoundaryValue;

        // Update the boundary text for this throw with the highest boundary hit
        if (throwCount < boundaryTexts.Count)
        {
            boundaryTexts[throwCount - 1].gameObject.SetActive(true); // Ensure the boundary text is visible
            boundaryTexts[throwCount - 1].text = $"{highestBoundaryValue}"; // Update the boundary text
        }

        // Reset the highest boundary value for the next ball
        highestBoundaryValue = 0;

        // Update the score display
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        // Update the score display to show both current score and highest boundary value
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void ButtonHandler()
    {
        List<Button> activeButtons = new List<Button>();

        // Check which buttons are active
        if (straightButton.gameObject.activeSelf)
            activeButtons.Add(straightButton);
        if (legSpinButton.gameObject.activeSelf)
            activeButtons.Add(legSpinButton);
        if (offSpinButton.gameObject.activeSelf)
            activeButtons.Add(offSpinButton);

        if (activeButtons.Count == 1)
        {
            // Only one button is active, no need to select a random one
            activeButton = activeButtons[0];
        }
        else if (activeButtons.Count > 1)
        {
            // If multiple buttons are active, select one randomly
            activeButton = activeButtons[Random.Range(0, activeButtons.Count)];
        }
    }

    public void RegisterThrow()
    {
        if (throwCount < throwTexts.Count)
        {
            float speed = BallControllerScriptForBatsman.instance.ballSpeed + 50; // Calculate ball speed
            throwTexts[throwCount].gameObject.SetActive(true); // Activate next text
            throwTexts[throwCount].text = $"Ball {throwCount + 1} Speed: {speed.ToString("F1")} km/h"; // Set the text to display the ball speed with one decimal place
        }

        throwCount++; // Increment throw count whenever a ball is thrown

        // Finalize and reset highest boundary after each throw
        ResetHighestBoundaryAndFinalizeScore();

        if (throwCount >= 6)
        {
            ResetThrows();
            ResetButtons();
            SceneManager.LoadScene("RightSpinBowler");
        }
    }

    private void ResetThrows()
    {
        // Reset all throw texts after 6 throws
        foreach (var text in throwTexts)
        {
            text.gameObject.SetActive(false);
        }

        foreach (var boundaryText in boundaryTexts)
        {
            boundaryText.gameObject.SetActive(false);
        }

        throwCount = 0;

        // Clear all button conditions
        BowlermanagerForBatsman.instance.straightButtonEnable = false;
        BowlermanagerForBatsman.instance.LegspinnButtonEnable = false;
        BowlermanagerForBatsman.instance.OffspinButtonEnable = false;

        ResetButtons(); // Ensure all buttons are active again
    }

    public void ActivateButton(Button selectedButton)
    {
        if (selectedButton == straightButton)
        {
            BowlermanagerForBatsman.instance.straightButtonEnable = true;
            BowlermanagerForBatsman.instance.LegspinnButtonEnable = false;
            BowlermanagerForBatsman.instance.OffspinButtonEnable = false;

            legSpinButton.gameObject.SetActive(false);
            offSpinButton.gameObject.SetActive(false);
        }
        else if (selectedButton == legSpinButton)
        {
            BowlermanagerForBatsman.instance.straightButtonEnable = false;
            BowlermanagerForBatsman.instance.LegspinnButtonEnable = true;
            BowlermanagerForBatsman.instance.OffspinButtonEnable = false;

            straightButton.gameObject.SetActive(false);
            offSpinButton.gameObject.SetActive(false);
        }
        else if (selectedButton == offSpinButton)
        {
            BowlermanagerForBatsman.instance.straightButtonEnable = false;
            BowlermanagerForBatsman.instance.LegspinnButtonEnable = false;
            BowlermanagerForBatsman.instance.OffspinButtonEnable = true;

            straightButton.gameObject.SetActive(false);
            legSpinButton.gameObject.SetActive(false);
        }
    }

    public void ResetButtons()
    {
        // Reset the button states after 6 throws or at the start
        legSpinButton.gameObject.SetActive(true);
        offSpinButton.gameObject.SetActive(true);
        straightButton.gameObject.SetActive(true);
    }

    public void ClickActiveButton()
    {
        if (activeButton != null)
        {
            // Simulate a button click
            activeButton.onClick.Invoke();
        }
    }
}
