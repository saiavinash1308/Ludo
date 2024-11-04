using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CricketOpens : MonoBehaviour
{
    public void BattingSide()
    {
        SceneManager.LoadScene("batsmanScene");
    }

    public void BowlingSide()
    {
        SceneManager.LoadScene("RightSpinBowler");
    }


}
