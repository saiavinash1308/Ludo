using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Redirector : MonoBehaviour
{

    public GameObject Home;



    public void GotoHome()
    {
        SceneManager.LoadScene("press");
    }
}
