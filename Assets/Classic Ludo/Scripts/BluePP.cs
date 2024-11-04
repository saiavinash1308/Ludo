

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePP : PP
{
    RD blueHomeRollingDice;
    void Start()
    {
        blueHomeRollingDice = GetComponentInParent<Blue>().rollingdice;
    }
    public void OnMouseDown()
    {
        if (GM.game.rolingDice == GM.game.manageRolingDice[0])
        {
            if (isready && GM.game.canPlayermove)
            {
                GM.game.canPlayermove = false;
                movestep(pathparent.BluePlayerPathPoint);
                GM.game.transferDice = false;
                GM.game.RolingDiceManager(); // After moving, transfer the dice
                GM.game.transferDice = true;
            }
            else if (!isready && GM.game.rolingDice == blueHomeRollingDice)
            {
                GM.game.blueOutPlayers += 1;
                makeplayerreadytomove(pathparent.BluePlayerPathPoint);
                GM.game.numberofstepstoMove = 0;

                // Allow the player to roll again if they rolled a six and moved out of the home
                GM.game.shouldRollAgain = true;
                GM.game.canDiceRoll = true;
            }
        }
    }
}
