

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPP : PP
{
    RD redHomeRollingDice;
    void Start()
    {
        redHomeRollingDice = GetComponentInParent<Red>().rollingdice;
    }

    public void OnMouseDown()
    {
        if (GM.game.rolingDice == GM.game.manageRolingDice[1])
        {
            if (isready && GM.game.canPlayermove)
            {
                GM.game.canPlayermove = false;
                movestep(pathparent.RedPlayerPathPoint);
                GM.game.transferDice = false;
                GM.game.RolingDiceManager(); // After moving, transfer the dice
                GM.game.transferDice = true;
            }
            else if (!isready && GM.game.rolingDice == redHomeRollingDice)
            {
                GM.game.redOutPlayers += 1;
                makeplayerreadytomove(pathparent.RedPlayerPathPoint);
                GM.game.numberofstepstoMove = 0;

                // Allow the player to roll again if they rolled a six and moved out of the home
                GM.game.shouldRollAgain = true;
                GM.game.canDiceRoll = true;
            }
        }
    }


}