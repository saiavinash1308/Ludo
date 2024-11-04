using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FooterScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onHomeButtonClicked()
    {
        SceneManager.LoadScene("Home Page");
    }

    public void onAddCashClicked()
    {
        SceneManager.LoadScene("AddCash");
    }

    public void onWalletPressed()
    {
        SceneManager.LoadScene("Wallet");
    }
    
    public void onReferrel()
    {
        SceneManager.LoadScene("Referrel");
    }
}
