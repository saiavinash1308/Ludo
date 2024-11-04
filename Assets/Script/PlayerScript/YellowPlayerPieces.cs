/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerPieces : PlayerPieces
{
    RollingDice yellowHomeRollingDice;
    void Start()
    {
        yellowHomeRollingDice = GetComponentInParent<YellowHome>().rollingdice;
    }

    public void OnMouseDown()
    {
        if (GameManager.game.rolingDice != null)
        {
            if (!isready)
            {
                if (GameManager.game.rolingDice == yellowHomeRollingDice && GameManager.game.numberofstepstoMove == 6)
                {
                    GameManager.game.yellowOutPlayers += 1;
                    makeplayerreadytomove(pathparent.YellowPlayerPathPoint);
                    GameManager.game.numberofstepstoMove = 0;
                    return;
                }
            }
            if (GameManager.game.rolingDice == yellowHomeRollingDice && isready && GameManager.game.canPlayermove)
            {
                GameManager.game.canPlayermove = false;
                movestep(pathparent.YellowPlayerPathPoint);

            }
        }
    }
}

*/


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerPieces : PlayerPieces
{
    RollingDice yellowHomeRollingDice;
    void Start()
    {
        yellowHomeRollingDice = GetComponentInParent<YellowHome>().rollingdice;


        GameManager.game.yellowOutPlayers = 4;
        makeplayerreadytomove(pathparent.YellowPlayerPathPoint);
        GameManager.game.numberofstepstoMove = 0;
        return;


    }

    public void OnMouseDown()
    {
        
            if (GameManager.game.rolingDice == yellowHomeRollingDice && isready && GameManager.game.canPlayermove)
            {
            Debug.Log($"Token {gameObject.name} has been clicked and is moving.");
            GameManager.game.canPlayermove = false;
                movestep(pathparent.YellowPlayerPathPoint);
            }
        
    }
}
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerPieces : PlayerPieces
{
    RollingDice yellowHomeRollingDice;

    void Start()
    {
        yellowHomeRollingDice = GetComponentInParent<YellowHome>().rollingdice;
        GameManager.game.yellowOutPlayers = 4;
        makeplayerreadytomove(pathparent.YellowPlayerPathPoint);
        GameManager.game.numberofstepstoMove = 0;
    }

    void OnMouseUpAsButton()
    {
        // Check if it's the blue player's turn and the dice rolled corresponds to the blue player
        if (GameManager.game.rolingDice == GameManager.game.manageRolingDice[3] && !GameManager.game.canDiceRoll)
        {
            if (isready && GameManager.game.canPlayermove)
            {
                GameManager.game.canPlayermove = false;
                movestep(pathparent.YellowPlayerPathPoint);
                GameManager.game.transferDice = false;
                GameManager.game.RolingDiceManager(); // After moving, transfer the dice
                GameManager.game.transferDice = true;
            }
            else if (!isready && GameManager.game.rolingDice == yellowHomeRollingDice)
            {
                GameManager.game.yellowOutPlayers = 4;
                makeplayerreadytomove(pathparent.YellowPlayerPathPoint);

                // Allow the player to roll again if they rolled a six and moved out of the home
                GameManager.game.shouldRollAgain = true;
                GameManager.game.canDiceRoll = true;
            }
        }
    }
}



