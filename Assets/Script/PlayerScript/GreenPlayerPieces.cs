/* 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GreenPlayerPieces : PlayerPieces
{
    RollingDice greenHomeRollingDice;
    void Start()
    {
        greenHomeRollingDice = GetComponentInParent<GreenHome>().rollingdice;
    }

    public void OnMouseDown()
    {
        if (GameManager.game.rolingDice != null)
        {
            if (!isready)
            {
                if (GameManager.game.rolingDice == greenHomeRollingDice && GameManager.game.numberofstepstoMove == 6)
                {
                    GameManager.game.greenOutPlayers += 1;
                    makeplayerreadytomove(pathparent.GreenPlayerPathPoint);
                    GameManager.game.numberofstepstoMove = 0;
                    return;
                }
            }
            if (GameManager.game.rolingDice == greenHomeRollingDice && isready && GameManager.game.canPlayermove)
            {
                GameManager.game.canPlayermove = false;
                movestep(pathparent.GreenPlayerPathPoint);
            }
        }
    }
}

*/

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerPieces : PlayerPieces
{
    RollingDice greenHomeRollingDice;
    void Start()
    {
        greenHomeRollingDice = GetComponentInParent<GreenHome>().rollingdice;


        GameManager.game.greenOutPlayers = 4;
        makeplayerreadytomove(pathparent.GreenPlayerPathPoint);
        GameManager.game.numberofstepstoMove = 0;
        return;


    }

    public void OnMouseDown()
    {

        if (GameManager.game.rolingDice == greenHomeRollingDice && isready && GameManager.game.canPlayermove)
        {
            Debug.Log($"Token {gameObject.name} has been clicked and is moving.");
            GameManager.game.canPlayermove = false;
            movestep(pathparent.GreenPlayerPathPoint);
        }

    }
}
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPlayerPieces : PlayerPieces
{
    RollingDice greenHomeRollingDice;

    void Start()
    {
        greenHomeRollingDice = GetComponentInParent<GreenHome>().rollingdice;
        GameManager.game.greenOutPlayers = 4;
        makeplayerreadytomove(pathparent.GreenPlayerPathPoint);
        GameManager.game.numberofstepstoMove = 0;
    }

    void OnMouseUpAsButton()
    {
        // Check if it's the green player's turn and the dice rolled corresponds to the green player
        if (GameManager.game.rolingDice == GameManager.game.manageRolingDice[2] && !GameManager.game.canDiceRoll)
        {
            if (isready && GameManager.game.canPlayermove)
            {
                GameManager.game.canPlayermove = false;
                movestep(pathparent.GreenPlayerPathPoint);
                GameManager.game.transferDice = false;
                GameManager.game.RolingDiceManager(); // After moving, transfer the dice
                GameManager.game.transferDice = true;
            }
            else if (!isready && GameManager.game.rolingDice == greenHomeRollingDice)
            {
                GameManager.game.greenOutPlayers = 4;
                makeplayerreadytomove(pathparent.GreenPlayerPathPoint);

                // Allow the player to roll again if they rolled a six and moved out of the home
                GameManager.game.shouldRollAgain = true;
                GameManager.game.canDiceRoll = true;
            }
        }
    }
}

