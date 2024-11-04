
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    public static GM game;
    public RD rolingDice;
    public int numberofstepstoMove;
    public bool canPlayermove = true;
    public List<PPt> playeronpathpointList = new List<PPt>();
    public bool canDiceRoll = true;
    public bool transferDice = false;
    public bool selfDice = false;
    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;

    public RD[] manageRolingDice;
    public bool shouldRollAgain = false;

    public int blueFinishedPlayers = 0;
    public int redFinishedPlayers = 0;
    public int greenFinishedPlayers = 0;
    public int yellowFinishedPlayers = 0;
    public AudioSource ads;
    public AudioSource centerPathAudioSource; // Add this reference

    public float countdownTime = 10f; // Set the countdown time (10 seconds)
    private float currentCountdown; // To track the remaining time
    private Coroutine countdownCoroutine; // To handle the countdown coroutine
    //public Image countdownImage; // Reference to the circular countdown Image
    public Image blueTimerImage;
    public Image redTimerImage;
    public Image greenTimerImage;
    public Image yellowTimerImage;


    // Declare individual arrows
    public GameObject bluearrow;
    public GameObject redarrow;
    public GameObject greenarrow;
    public GameObject yellowarrow;

    private const string TOGGLE_PREF_KEY = "ButtonTogglerState";


    private void Awake()
    {
        game = this;
        ads = GetComponent<AudioSource>();
        rolingDice = manageRolingDice[0]; // Ensure it starts with the blue player's dice
        UpdateDiceOpacity(); // Update opacity at start
    }

    private void Start()
    {
        // Set initial arrow visibility
        UpdateArrowVisibility();

       ResizePlayerPieces(); // Ensure initial resizing
                             // Load the saved toggle state from PlayerPrefs
        bool isOn = PlayerPrefs.GetInt(TOGGLE_PREF_KEY, 1) == 1;  // Default to 'on' if no value is saved

        // Update the audio volume based on the toggle state
        SetAudioVolume(isOn);
    }


    public void SetAudioVolume(bool isOn)
    {
        float volume = isOn ? 1.0f : 0.0f;

        if (ads != null)
        {
            ads.volume = volume;
        }

        if (centerPathAudioSource != null)
        {
            centerPathAudioSource.volume = volume;
        }
    }


    public void AddPathPoint(PPt pathPoint)
    {
        playeronpathpointList.Add(pathPoint);
    }

    public void RemovePathPoint(PPt pathPoint)
    {
        if (playeronpathpointList.Contains(pathPoint))
        {
            playeronpathpointList.Remove(pathPoint);
        }
        else
        {
            Debug.Log("Path point not found to be removed");
        }
    }

    public void RollDice()
    {
        numberofstepstoMove = Random.Range(1, 7); // Simulate a dice roll between 1 and 6
        Debug.Log("Dice rolled: " + numberofstepstoMove);
    }




    private void UpdateDiceOpacity()
    {
        for (int i = 0; i < manageRolingDice.Length; i++)
        {
            // Get the SpriteRenderer for the NumberSpriteHolder
            SpriteRenderer diceRenderer = manageRolingDice[i].transform.Find("NSH").GetComponent<SpriteRenderer>();

            // Get the SpriteRenderer for the new GameObject (replace "NewGameObjectName" with your new GameObject's name)
            SpriteRenderer newObjectRenderer = manageRolingDice[i].transform.Find("DB").GetComponent<SpriteRenderer>();

            if (diceRenderer != null)
            {
                Color diceColor = diceRenderer.color;
                Color newObjectColor = newObjectRenderer.color;

                if (manageRolingDice[i] == rolingDice)
                {
                    // Set full opacity for the active dice and the new object
                    diceColor.a = 1f;
                    newObjectColor.a = 1f;
                }
                else
                {
                    // Set reduced opacity for inactive dice and the new object
                    diceColor.a = 0.5f;
                    newObjectColor.a = 0.1f;
                }

                diceRenderer.color = diceColor;
                newObjectRenderer.color = newObjectColor;
            }
            else
            {
                Debug.LogWarning(manageRolingDice[i].name + " does not have a SpriteRenderer on its NumberSpriteHolder or the new GameObject.");
            }
        }
    }

    public bool AllPlayersInHome()
    {
        List<PP> pieces = GetPlayerPiecesForCurrentDice();

        // All players are in home if none of the pieces are ready to move
        foreach (PP piece in pieces)
        {
            if (piece.isready)
            {
                return false;
            }
        }
        return true;
    }

    public void RolingDiceManager()
    {
        if (shouldRollAgain)
        {
            shouldRollAgain = false;
            canDiceRoll = true;
            return;
        }

        if (transferDice)
        {
            int currentIndex = System.Array.IndexOf(manageRolingDice, rolingDice);
            int nextIndex = (currentIndex + 1) % manageRolingDice.Length;

            rolingDice = manageRolingDice[nextIndex];
            Debug.Log("Current Dice Index: " + currentIndex + " | Next Dice Index: " + nextIndex);

            Handheld.Vibrate();
            UpdateDiceOpacity();
            UpdateArrowVisibility();
           ResizePlayerPieces(); // Call to resize pieces

            canDiceRoll = true;
            transferDice = false;

            StartCountdown();
        }
        else if (selfDice)
        {
            selfDice = false;
            canDiceRoll = true;
            StartCountdown();
            UpdateArrowVisibility();
            ResizePlayerPieces(); // Call to resize pieces
        }
    }

    private void StartCountdown()
    {
        ResetCountdown();

        // Deactivate all timers first
        blueTimerImage.gameObject.SetActive(false);
        redTimerImage.gameObject.SetActive(false);
        greenTimerImage.gameObject.SetActive(false);
        yellowTimerImage.gameObject.SetActive(false);

        // Determine which player's turn it is and start the corresponding timer
        if (rolingDice == manageRolingDice[0]) // Blue Player
        {
            blueTimerImage.gameObject.SetActive(true);  // Activate the blue timer
            countdownCoroutine = StartCoroutine(CountdownCoroutine(blueTimerImage));
        }
        else if (rolingDice == manageRolingDice[1]) // Red Player
        {
            redTimerImage.gameObject.SetActive(true);  // Activate the red timer
            countdownCoroutine = StartCoroutine(CountdownCoroutine(redTimerImage));
        }
        else if (rolingDice == manageRolingDice[2]) // Green Player
        {
            greenTimerImage.gameObject.SetActive(true);  // Activate the green timer
            countdownCoroutine = StartCoroutine(CountdownCoroutine(greenTimerImage));
        }
        else if (rolingDice == manageRolingDice[3]) // Yellow Player
        {
            yellowTimerImage.gameObject.SetActive(true);  // Activate the yellow timer
            countdownCoroutine = StartCoroutine(CountdownCoroutine(yellowTimerImage));
        }
    }



    public void ResetCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }
    private IEnumerator CountdownCoroutine(Image currentTimerImage)
    {
        float startTime = Time.time;  // Record the start time
        float endTime = startTime + countdownTime;  // Calculate the end time

        while (Time.time < endTime)
        {
            float timeRemaining = endTime - Time.time;  // Calculate the remaining time
            currentTimerImage.fillAmount = timeRemaining / countdownTime;  // Update the fill amount

            yield return null;  // Wait until the next frame
        }

        // Ensure the fill amount is set to 0 at the end of the countdown
        currentTimerImage.fillAmount = 0;

        // Deactivate the timer's GameObject after the countdown ends
        currentTimerImage.gameObject.SetActive(false);

        HandleTurnTimeout();  // Handle the timeout when time runs out
    }

    private void HandleTurnTimeout()
    {
        Debug.Log("Player's time is up!");

        // Move to the next player's turn
        transferDice = true;
        canDiceRoll = false;
        RolingDiceManager();
    }

    public void IncrementCenterCount(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                blueFinishedPlayers++;
                if (blueFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Red":
                redFinishedPlayers++;
                if (redFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Green":
                greenFinishedPlayers++;
                if (greenFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
            case "Yellow":
                yellowFinishedPlayers++;
                if (yellowFinishedPlayers == 4) LoadWinnerScene(playerColor);
                break;
        }
    }

    public bool HasPlayerWon(string playerColor)
    {
        switch (playerColor)
        {
            case "Blue":
                return blueFinishedPlayers == 4;
            case "Red":
                return redFinishedPlayers == 4;
            case "Green":
                return greenFinishedPlayers == 4;
            case "Yellow":
                return yellowFinishedPlayers == 4;
            default:
                return false;
        }
    }

    public void CheckWinCondition()
    {
        if (HasPlayerWon("Blue"))
        {
            Debug.Log("Blue Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Red"))
        {
            Debug.Log("Red Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Green"))
        {
            Debug.Log("Green Player Wins!");
            EndGame();
        }
        else if (HasPlayerWon("Yellow"))
        {
            Debug.Log("Yellow Player Wins!");
            EndGame();
        }
    }

    private void EndGame()
    {
        // Disable all player pieces and dice rolls
        foreach (PP piece in FindObjectsOfType<PP>())
        {
            piece.enabled = false;
        }

        foreach (RD dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        canDiceRoll = false;
        canPlayermove = false;

        // Additional end game logic like showing a winner screen can go here
    }

    private void LoadWinnerScene(string winner)
    {
        // You can pass the winner information using PlayerPrefs or any other method
        PlayerPrefs.SetString("Winner", winner);
        SceneManager.LoadScene("Winner");
    }

    public void ResetDiceRoll()
    {
        numberofstepstoMove = 0;
    }
    public List<PP> GetPlayerPiecesForCurrentDice()
    {
        List<PP> playerPieces = new List<PP>();

        foreach (PP piece in FindObjectsOfType<PP>())
        {
            if (piece.name.Contains("Blue") && rolingDice == manageRolingDice[0])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Green") && rolingDice == manageRolingDice[2])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Red") && rolingDice == manageRolingDice[1])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Yellow") && rolingDice == manageRolingDice[3])
            {
                playerPieces.Add(piece);
            }
        }

        return playerPieces;
    }


    private void UpdateArrowVisibility()
    {
        // Set all arrows inactive initially
        bluearrow.SetActive(false);
        redarrow.SetActive(false);
        greenarrow.SetActive(false);
        yellowarrow.SetActive(false);

        // Activate the corresponding arrow based on the active dice
        if (rolingDice == manageRolingDice[0]) // Blue player's turn
        {
            bluearrow.SetActive(true);
        }
        else if (rolingDice == manageRolingDice[1]) // Red player's turn
        {
            redarrow.SetActive(true);
        }
        else if (rolingDice == manageRolingDice[2]) // Green player's turn
        {
            greenarrow.SetActive(true);
        }
        else if (rolingDice == manageRolingDice[3]) // Yellow player's turn
        {
            yellowarrow.SetActive(true);
        }
    }
    void ResizePlayerPieces()
    {
        // Get the active color
        string activeColor = "";
        if (rolingDice == manageRolingDice[0]) activeColor = "Blue1";
        else if (rolingDice == manageRolingDice[1]) activeColor = "Red1";
        else if (rolingDice == manageRolingDice[2]) activeColor = "Green1";
        else if (rolingDice == manageRolingDice[3]) activeColor = "Yellow1";

        // Resize pieces based on the active color and safe zone condition
        foreach (string color in new[] { "Blue1", "Red1", "Green1", "Yellow1" })
        {
            // Default scale and height
            float scale = 0.35f;
            float height = 0.35f;
            float depth = 1.2f;
            int sortingOrder = 1; // Default sorting order

            // Adjust scale and height if the color is active
            if (color == activeColor)
            {
                scale = 0.5f;
                height = 0.5f;
                depth = 2.56f;
                sortingOrder = 2; // Higher sorting order for active color
            }

            // Find all pieces with the current color tag
            GameObject[] pieces = GameObject.FindGameObjectsWithTag(color);

            foreach (GameObject piece in pieces)
            {
                PP playerPiece = piece.GetComponent<PP>();
                if (playerPiece != null)
                {
                    // Debug the piece's properties
                    Debug.Log($"Piece: {piece.name}, Color: {color}, Active Color: {activeColor}");

                    // Check if the piece is in the safe zone
                    bool isSafeZone = playerPiece.IsSafeZone; // Ensure this is correctly set
                    Debug.Log($"Piece: {piece.name}, Is Safe Zone: {isSafeZone}"); // Debug information

                    // If the piece is in a safe zone, apply the smaller scale
                    if (isSafeZone)
                    {
                        scale = 0.25f; // Smaller scale when in the safe zone
                        height = 0.25f;
                        depth = 2f;
                    }

                    // Resize the piece
                    piece.transform.localScale = new Vector3(scale, height, depth);

                    // Update sorting order based on the y-position
                    SpriteRenderer spriteRenderer = piece.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        // The higher the y-position, the lower the sorting order
                        spriteRenderer.sortingOrder = Mathf.RoundToInt(-piece.transform.position.y * 100);

                        // Increase sorting order if the piece matches the active color
                        if (color == activeColor)
                        {
                            spriteRenderer.sortingOrder += 10;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"No PP component found on piece: {piece.name}");
                }
            }
        }
    }











    public void PlayCenterPathAudio()
    {
        if (centerPathAudioSource != null)
        {
            centerPathAudioSource.Play();
        }
    }
}
