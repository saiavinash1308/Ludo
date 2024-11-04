

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPP : PP
{
    RD yellowHomeRollingDice;
    void Start()
    {
        yellowHomeRollingDice = GetComponentInParent<Yellow>().rollingdice;
    }

    public void OnMouseDown()
    {
        if (GM.game.rolingDice == GM.game.manageRolingDice[3])
        {
            if (isready && GM.game.canPlayermove)
            {
                GM.game.canPlayermove = false;
                movestep(pathparent.YellowPlayerPathPoint);
                GM.game.transferDice = false;
                GM.game.RolingDiceManager(); // After moving, transfer the dice
                GM.game.transferDice = true;
            }
            else if (!isready && GM.game.rolingDice == yellowHomeRollingDice)
            {
                GM.game.yellowOutPlayers += 1;
                makeplayerreadytomove(pathparent.YellowPlayerPathPoint);
                GM.game.numberofstepstoMove = 0;

                // Allow the player to roll again if they rolled a six and moved out of the home
                GM.game.shouldRollAgain = true;
                GM.game.canDiceRoll = true;
            }
        }
    }



}