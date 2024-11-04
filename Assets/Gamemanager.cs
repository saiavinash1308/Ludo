using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Import TextMeshPro namespace

public class Gamemanager : MonoBehaviour
{
    public GameObject OutfitTopS;
    public GameObject OutfitBottomS;
    public List<Material> topMaterials;
    public List<Material> bottomMaterials;
    public GameObject Outfitcam;
    public GameObject closeButton;

    public static Gamemanager instance;

    private int currentIndex = 0;
    public int throwCount = 0; // Count to keep track of the number of balls thrown

    [Header("Button References")]
    [SerializeField] public Button straightButton;
    [SerializeField] public Button legSpinButton;
    [SerializeField] public Button offSpinButton;


    [Header("Score Management")]
    public TextMeshProUGUI scoreText;  // TextMeshPro component to display the score
    public int currentScore = 0;      // Current score of the player
    public int highestBoundaryValue = 0;  // To store the highest boundary value reached in the current shot

    [Header("TextMeshPro References")]
    [SerializeField] private List<TextMeshProUGUI> throwTexts; // List to store TextMeshPro text objects
    [SerializeField] private List<TextMeshProUGUI> boundaryTexts; // List to store TextMeshPro boundary texts for each throw

    void Start()
    {
        UpdateScoreText();
        // Set the initial materials
        SetOutfitMaterials(currentIndex);
        ResetButtons(); // Ensure all buttons are active at start

        // Initialize text objects to be inactive at start
        foreach (var text in throwTexts)
        {
            text.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        ButtonHandler();

    }

    public void UpdateHighestBoundary(int boundaryValue)
    {
        // Update the highest boundary value if the new one is larger
        if (boundaryValue > highestBoundaryValue)
        {
            highestBoundaryValue = boundaryValue;
            if (throwCount < boundaryTexts.Count)
            {
                boundaryTexts[throwCount - 1].text = $"{highestBoundaryValue}";
            }
        }

        // Immediately update the score text to show the highest boundary value
        UpdateScoreText();
    }

    // Call this to finalize the score and reset the highest boundary for the next ball
    public void ResetHighestBoundaryAndFinalizeScore()
    {
        // Add the highest boundary value to the current score
        currentScore += highestBoundaryValue;


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
        // Check for any active condition and deactivate other buttons accordingly
        if (Bowlermanager.instance.straightButtonEnable)
        {
            legSpinButton.gameObject.SetActive(false);
            offSpinButton.gameObject.SetActive(false);
        }
        else if (Bowlermanager.instance.LegspinnButtonEnable)
        {
            straightButton.gameObject.SetActive(false);
            offSpinButton.gameObject.SetActive(false);
        }
        else if (Bowlermanager.instance.OffspinButtonEnable)
        {
            legSpinButton.gameObject.SetActive(false);
            straightButton.gameObject.SetActive(false);
        }
        else
        {
            // If none of the buttons are specifically enabled, keep them active
            ResetButtons();
        }
    }

    private void ResetThrows()
    {
        // Reset all throw texts after 6 throws
        foreach (var text in throwTexts)
        {
            text.gameObject.SetActive(false);
        }

        throwCount = 0;

        // Clear all button conditions
        Bowlermanager.instance.straightButtonEnable = false;
        Bowlermanager.instance.LegspinnButtonEnable = false;
        Bowlermanager.instance.OffspinButtonEnable = false;

        ResetButtons(); // Ensure all buttons are active again
    }


    public void RegisterThrow()
    {
        if (throwCount < throwTexts.Count)
        {
            float speed = BallControllerScript.instance.ballSpeed + 50; // Calculate ball speed
            throwTexts[throwCount].gameObject.SetActive(true); // Activate next text
            throwTexts[throwCount].text = $"Ball {throwCount + 1} Speed: {speed.ToString("F1")} km/h"; // Set the text to display the ball speed with one decimal place
            
        }

        throwCount++; // Increment throw count whenever a ball is thrown

            if (throwCount >= 6)
        {
            ResetThrows();
            ResetButtons();
            SceneManager.LoadScene("batsmanScene");
        }
    }


    public void ResetButtons()
    {
        // Reset the button states after 6 throws or at the start
        legSpinButton.gameObject.SetActive(true);
        offSpinButton.gameObject.SetActive(true);
        straightButton.gameObject.SetActive(true);
    }

    private void SetOutfitMaterials(int index)
    {
        if (index < topMaterials.Count && index < bottomMaterials.Count)
        {
            OutfitTopS.GetComponent<Renderer>().material = topMaterials[index];
            OutfitBottomS.GetComponent<Renderer>().material = bottomMaterials[index];
        }
    }

    public void OnChangeOutfitButtonClick()
    {
        Outfitcam.SetActive(true);
        closeButton.SetActive(true);
        legSpinButton.gameObject.SetActive(false);
        offSpinButton.gameObject.SetActive(false);
        straightButton.gameObject.SetActive(false);
        currentIndex = (currentIndex + 1) % topMaterials.Count;
        SetOutfitMaterials(currentIndex);
    }

    public void closeButtonClicked()
    {
        Outfitcam?.SetActive(false);
        closeButton?.SetActive(false);
        legSpinButton.gameObject.SetActive(true);
        offSpinButton.gameObject.SetActive(true);
        straightButton.gameObject.SetActive(true);
    }
}
