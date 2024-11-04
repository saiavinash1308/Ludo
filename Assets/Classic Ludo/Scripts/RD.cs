
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RD : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites;  // Array to hold the sprites for each dice face (1 to 6)
    [SerializeField] SpriteRenderer numberSpriteHolder;  // The SpriteRenderer that will display the dice face
    [SerializeField] SpriteRenderer rolingdiceanimation;  // SpriteRenderer for the rolling animation
    [SerializeField] int numberGot;  // The result of the dice roll

    Coroutine generateRandomNumberonDice;
    public int outpieces;
    public DA diceSound;

    private void Awake()
    {
        // Find and assign the SpriteRenderer on the NumberSpriteHolder child object
        numberSpriteHolder = transform.Find("NSH").GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        if (GM.game.canDiceRoll && GM.game.rolingDice == this)
        {
            GM.game.ResetCountdown(); // Stop the countdown when the player rolls the dice
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }

    public void RollDiceForBot()
    {
        if (GM.game.canDiceRoll && GM.game.rolingDice == this)
        {
            generateRandomNumberonDice = StartCoroutine(RollingDiceCoroutine());
        }
    }


    IEnumerator RollingDiceCoroutine()
    {
        GM.game.transferDice = false;
        yield return new WaitForEndOfFrame();
        if (GM.game.canDiceRoll)
        {
            GM.game.canDiceRoll = false;
            diceSound.PlaySound();
            numberSpriteHolder.gameObject.SetActive(false);
            rolingdiceanimation.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            numberGot = Random.Range(0, 6);
            numberSpriteHolder.sprite = numberSprites[numberGot];
            numberGot += 1;

            // Automatically set the number of steps to move in the GM
            GM.game.numberofstepstoMove = numberGot;
            GM.game.rolingDice = this;
            numberSpriteHolder.gameObject.SetActive(true);
            rolingdiceanimation.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            // Check if the current player has any tokens out of the home
            bool hasOutPlayers = (GM.game.rolingDice == GM.game.manageRolingDice[0] && GM.game.blueOutPlayers > 0) ||
                                 (GM.game.rolingDice == GM.game.manageRolingDice[1] && GM.game.redOutPlayers > 0) ||
                                 (GM.game.rolingDice == GM.game.manageRolingDice[2] && GM.game.greenOutPlayers > 0) ||
                                 (GM.game.rolingDice == GM.game.manageRolingDice[3] && GM.game.yellowOutPlayers > 0);

            // If the player rolled a six or has tokens out of the home, they should move their token
            GM.game.canDiceRoll = false; // Prevent rolling until move is made
            GM.game.canPlayermove = true; // Always allow the player to move

        if (generateRandomNumberonDice != null)
            {
                StopCoroutine(RollingDiceCoroutine());
            }
        }
    }


    bool CanPlayerMove()
    {
        GM gm = GM.game;
        List<PP> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PP piece in pieces)
        {
            if (piece.CanMove(gm.numberofstepstoMove))
            {
                return true;
            }
        }
        return false;
    }

    bool AllPlayersCloseToCenterPath()
    {
        GM gm = GM.game;
        List<PP> pieces = gm.GetPlayerPiecesForCurrentDice();

        foreach (PP piece in pieces)
        {
            if (!piece.IsCloseToCenterPath())
            {
                return false;
            }
        }
        return true;
    }
}
