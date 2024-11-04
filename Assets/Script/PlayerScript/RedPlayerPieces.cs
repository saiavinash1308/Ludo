/*   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlayerPieces : PlayerPieces
{
   RollingDice redHomeRollingDice;
   void Start()
   {
       redHomeRollingDice = GetComponentInParent<RedHome>().rollingdice;
   }

   public void OnMouseDown()
   {
       if (GameManager.game.rolingDice != null)
       {
           if (!isready)
           {
               if (GameManager.game.rolingDice == redHomeRollingDice && GameManager.game.numberofstepstoMove == 6)
               {

                   GameManager.game.redOutPlayers += 1;
                   makeplayerreadytomove(pathparent.RedPlayerPathPoint);
                   GameManager.game.numberofstepstoMove = 0;
                   return;
               }
           }
           if (GameManager.game.rolingDice == redHomeRollingDice && isready && GameManager.game.canPlayermove)
           {
               GameManager.game.canPlayermove = false;
               movestep(pathparent.RedPlayerPathPoint);
           }
       }
   }
}
*/

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlayerPieces : PlayerPieces
{
    RollingDice redHomeRollingDice;
    void Start()
    {
        redHomeRollingDice = GetComponentInParent<RedHome>().rollingdice;


        GameManager.game.redOutPlayers = 4;
        makeplayerreadytomove(pathparent.RedPlayerPathPoint);
        GameManager.game.numberofstepstoMove = 0;
        return;


    }

    public void OnMouseDown()
    {

        if (GameManager.game.rolingDice == redHomeRollingDice && isready && GameManager.game.canPlayermove)
        {
            Debug.Log($"Token {gameObject.name} has been clicked and is moving.");
            GameManager.game.canPlayermove = false;
            movestep(pathparent.RedPlayerPathPoint);
        }

    }
}
*/




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlayerPieces : PlayerPieces
{
    RollingDice redHomeRollingDice;

    void Start()
    {
        redHomeRollingDice = GetComponentInParent<RedHome>().rollingdice;
        GameManager.game.redOutPlayers = 4;
        makeplayerreadytomove(pathparent.RedPlayerPathPoint);
        GameManager.game.numberofstepstoMove = 0;
    }

    void OnMouseUpAsButton()
    {
        // Check if it's the blue player's turn and the dice rolled corresponds to the blue player
        if (GameManager.game.rolingDice == GameManager.game.manageRolingDice[1] && !GameManager.game.canDiceRoll)
        {
            if (isready && GameManager.game.canPlayermove)
            {
                GameManager.game.canPlayermove = false;
                movestep(pathparent.RedPlayerPathPoint);
                GameManager.game.transferDice = false;
                GameManager.game.RolingDiceManager(); // After moving, transfer the dice
                GameManager.game.transferDice = true;
            }
            else if (!isready && GameManager.game.rolingDice == redHomeRollingDice)
            {
                GameManager.game.redOutPlayers = 4;
                makeplayerreadytomove(pathparent.RedPlayerPathPoint);

                // Allow the player to roll again if they rolled a six and moved out of the home
                GameManager.game.shouldRollAgain = true;
                GameManager.game.canDiceRoll = true;
            }
        }
    }
}


