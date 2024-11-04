
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPP : PP
{
    RD greenHomeRollingDice;
    void Start()
    {
        greenHomeRollingDice = GetComponentInParent<Green>().rollingdice;
    }

    public void OnMouseDown()
    {
        if (GM.game.rolingDice == GM.game.manageRolingDice[2])
        {
            if (isready && GM.game.canPlayermove)
            {
                GM.game.canPlayermove = false;
                movestep(pathparent.GreenPlayerPathPoint);
                GM.game.transferDice = false;
                GM.game.RolingDiceManager(); // After moving, transfer the dice
                GM.game.transferDice = true;
            }
            else if (!isready && GM.game.rolingDice == greenHomeRollingDice)
            {
                GM.game.greenOutPlayers += 1;
                makeplayerreadytomove(pathparent.GreenPlayerPathPoint);
                GM.game.numberofstepstoMove = 0;

                // Allow the player to roll again if they rolled a six and moved out of the home
                GM.game.shouldRollAgain = true;
                GM.game.canDiceRoll = true;
            }
        }
    }



}