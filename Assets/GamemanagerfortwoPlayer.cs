

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManagerFortwoPlayers : MonoBehaviour
{
    public static GameManagerFortwoPlayers game;
    public int WinningText = 0;
    public int TotalMatches = 0;
    public TextMeshProUGUI userNameText; // Reference to the TextMeshProUGUI component for displaying the user's name
    public RollingDice rolingDice;
    public int numberofstepstoMove;
    public bool canPlayermove = true;
    public List<PathPoint> playeronpathpointList = new List<PathPoint>();
    public bool canDiceRoll = true;
    public bool transferDice = false;
    public bool selfDice = false;
    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;
    public int redpoints = 0;
    public int yellowpoints = 0;
    public int greenpoints = 0;
    public int bluepoints = 0;


    public float rollTime = 10f;  // Time to roll the dice
    public float moveTime = 10f;  // Time to move after rolling the dice
    private float currentRollTime;
    private float currentMoveTime;

    private Coroutine countdownCoroutine;
    private bool hasRolledDice = false;

    public RollingDice[] manageRolingDice;
    public bool shouldRollAgain = false;

    public int blueFinishedPlayers = 0;
    public int redFinishedPlayers = 0;
    public int greenFinishedPlayers = 0;
    public int yellowFinishedPlayers = 0;
    public AudioSource ads;
    public AudioSource centerPathAudioSource; // Add this reference

    public float countdownTime = 10f; // Set the countdown time (10 seconds)
    private float currentCountdown; // To track the remaining time
    public Image blueTimerImage;
    public Image redTimerImage;
    public Image greenTimerImage;
    public Image yellowTimerImage;




    // Declare individual arrows
    public GameObject bluearrow;
    public GameObject redarrow;
    public GameObject greenarrow;
    public GameObject yellowarrow;


    private void Awake()
    {
        game = this;
        ads = GetComponent<AudioSource>();
        rolingDice = manageRolingDice[0]; // Ensure it starts with the blue player's dice
        UpdateDiceOpacity(); // Update opacity at start
    }

    private void Start()
    {
        string savedName = PlayerPrefs.GetString("userName", "Guest");
        userNameText.text = savedName;

        // Load the saved winning count and total matches from PlayerPrefs
        WinningText = PlayerPrefs.GetInt("Win", 0);  // Default to 0 if no value is saved
        TotalMatches = PlayerPrefs.GetInt("TotalMatches", 0); // Default to 0 if no value is saved

        // Increment TotalMatches when the game starts
        TotalMatches++;
        PlayerPrefs.SetInt("TotalMatches", TotalMatches);
        PlayerPrefs.Save(); // Save the updated TotalMatches

        Debug.Log("Loaded Winning Text: " + WinningText);
        Debug.Log("Total Matches Updated: " + TotalMatches); // Updated total matches logging

        StartCoroutine(BotRoutine());
        StartCountdown();
        UpdateArrowVisibility();
        ResizePlayerPieces(); // Ensure initial resizing
    }



    public void AddPathPoint(PathPoint pathPoint)
    {
        playeronpathpointList.Add(pathPoint);
    }

    public void RemovePathPoint(PathPoint pathPoint)
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

        // Stop and reset the countdown timer when the dice is rolled
        ResetCountdown();
    }


    public void UpdatePlayerPoints(int points)
    {
        if (rolingDice == manageRolingDice[0])
        {
            redpoints += points;
            redpoints = Mathf.Max(0, redpoints); // Ensure score doesn't go negative
            Debug.Log("RedPoints: " + redpoints);
        }
        else if (rolingDice == manageRolingDice[1])
        {
            yellowpoints += points;
            yellowpoints = Mathf.Max(0, yellowpoints); // Ensure score doesn't go negative
            Debug.Log("YellowPoints: " + yellowpoints);
        }
    }

    private void UpdateDiceOpacity()
    {
        for (int i = 0; i < manageRolingDice.Length; i++)
        {
            SpriteRenderer diceRenderer = manageRolingDice[i].transform.Find("NumberSpriteHolder").GetComponent<SpriteRenderer>();
            SpriteRenderer newObjectRenderer = manageRolingDice[i].transform.Find("DiceBoard").GetComponent<SpriteRenderer>();

            if (diceRenderer != null)
            {
                Color diceColor = diceRenderer.color;
                Color newObjectColor = newObjectRenderer.color;

                if (manageRolingDice[i] == rolingDice)
                {
                    diceColor.a = 1f;
                    newObjectColor.a = 1f;
                }
                else
                {
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


    private void UpdateArrowVisibility()
    {
        // Set all arrows inactive initially

        redarrow.SetActive(false);
        yellowarrow.SetActive(false);

        // Activate the corresponding arrow based on the active dice
        if (rolingDice == manageRolingDice[0]) // Red player's turn
        {
            redarrow.SetActive(true);
        }
        else if (rolingDice == manageRolingDice[1]) // Yellow player's turn
        {
            yellowarrow.SetActive(true);
        }
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



    public void StartCountdown()
    {
        ResetCountdown();

        blueTimerImage.gameObject.SetActive(false);
        redTimerImage.gameObject.SetActive(false);
        greenTimerImage.gameObject.SetActive(false);
        yellowTimerImage.gameObject.SetActive(false);

        if (rolingDice == manageRolingDice[0])
        {
            redTimerImage.gameObject.SetActive(true);
            countdownCoroutine = StartCoroutine(CountdownCoroutine(redTimerImage));
        }
        else if (rolingDice == manageRolingDice[1])
        {
            yellowTimerImage.gameObject.SetActive(true);
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
        float startTime = Time.time;
        float endTime = startTime + countdownTime;

        while (Time.time < endTime)
        {
            float timeRemaining = endTime - Time.time;
            currentTimerImage.fillAmount = timeRemaining / countdownTime;

            yield return null;
        }

        currentTimerImage.fillAmount = 0;
        currentTimerImage.gameObject.SetActive(false);

        HandleTurnTimeout();
    }


    private void HandleTurnTimeout()
    {
        Debug.Log("Player's time is up!");

        transferDice = true;
        canDiceRoll = false;
        RolingDiceManager();
    }


    public void IncrementCenterCount(string playerColor)
    {
        switch (playerColor)
        {

            case "Red":
                redFinishedPlayers++;
                if (redFinishedPlayers == 4) LoadWinnerScene(playerColor);
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

            case "Red":
                return redFinishedPlayers == 4;
            case "Yellow":
                return yellowFinishedPlayers == 4;
            default:
                return false;
        }
    }

    public void CheckWinCondition()
    {

        if (HasPlayerWon("Red"))
        {
            Debug.Log("Red Player Wins!");
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
        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            piece.enabled = false;
        }

        foreach (RollingDice dice in manageRolingDice)
        {
            dice.enabled = false;
        }

        canDiceRoll = false;
        canPlayermove = false;
    }

    private void LoadWinnerScene(string winner)
    {
        PlayerPrefs.SetString("Winner", winner);

        // Increment the winning count and total matches
        WinningText++;
        TotalMatches++;

        // Save the updated winning count and total matches to PlayerPrefs
        PlayerPrefs.SetInt("Win", WinningText);
        PlayerPrefs.SetInt("TotalMatches", TotalMatches);
        PlayerPrefs.Save();  // Ensure the changes are saved to disk

        Debug.Log("Winning Text Updated: " + WinningText);
        Debug.Log("Total Matches Updated: " + TotalMatches);

        SceneManager.LoadScene("Winner");
    }


    public void ResetDiceRoll()
    {
        numberofstepstoMove = 0;
    }

    public List<PlayerPieces> GetPlayerPiecesForCurrentDice()
    {
        List<PlayerPieces> playerPieces = new List<PlayerPieces>();

        foreach (PlayerPieces piece in FindObjectsOfType<PlayerPieces>())
        {
            if (piece.name.Contains("Red") && rolingDice == manageRolingDice[0])
            {
                playerPieces.Add(piece);
            }
            else if (piece.name.Contains("Yellow") && rolingDice == manageRolingDice[1])
            {
                playerPieces.Add(piece);
            }
        }

        return playerPieces;
    }

    private IEnumerator BotRoutine()
    {
        while (true)
        {
            if (rolingDice == manageRolingDice[1] && canDiceRoll)
            {
                Handheld.Vibrate();
                manageRolingDice[1].RollDiceForBot();
                yield return new WaitForSeconds(1f);

                List<PlayerPieces> pieces = GetPlayerPiecesForCurrentDice();
                if (pieces.Count > 0)
                {
                    PlayerPieces pieceToMove = pieces[Random.Range(0, pieces.Count)];
                    PathPoint[] pathpoints = pieceToMove.GetPathPointsForColor();
                    pieceToMove.movestep(pathpoints);
                    yield return new WaitForSeconds(1f);
                }

                canDiceRoll = true;
                transferDice = true;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void ResizePlayerPieces()
    {
        // Get the active color
        string activeColor = "";
        if (rolingDice == manageRolingDice[0]) activeColor = "Red";
        else if (rolingDice == manageRolingDice[1]) activeColor = "Yellow";

        // Resize pieces based on the active color and safe zone status
        foreach (string color in new[] { "Red", "Yellow" })
        {
            GameObject[] pieces = GameObject.FindGameObjectsWithTag(color);

            foreach (GameObject piece in pieces)
            {
                PlayerPieces playerPiece = piece.GetComponent<PlayerPieces>();

                // Determine if the piece is in a safe zone
                bool isInSafeZone = playerPiece != null && playerPiece.IsInSafeZone;

                // Define scaling factors
                float scale, height, depth;

                // If the piece is in a safe zone, make it smaller
                if (isInSafeZone)
                {
                    scale = 7f;   // Smaller scale when in the safe zone
                    height = 7f;
                    depth = 2f;
                }
                else
                {
                    // If the piece is not in the safe zone, use the standard scaling
                    scale = color == activeColor ? 13f : 11f;
                    height = color == activeColor ? 13f : 11f;
                    depth = color == activeColor ? 3f : 1.5f;
                }

                // Resize the piece
                piece.transform.localScale = new Vector3(scale, height, depth);

                // Update sorting order based on the y-position to prevent hiding
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
