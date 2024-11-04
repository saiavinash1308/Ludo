using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this to use TextMeshPro
using UnityEngine.SceneManagement;

public class HomePageManager : MonoBehaviour
{
    public TextMeshProUGUI userNameText; // Reference to the TextMeshProUGUI component for displaying the user's name

    void Start()
    {
        // Retrieve the user's name from PlayerPrefs and display it on the homepage
        string savedName = PlayerPrefs.GetString("userName", "Guest");
        userNameText.text = savedName;
    }

    public void OnClassicLudo()
    {
        SceneManager.LoadScene("classicLudoLoadingScene");
    }

    public void OnFastLudo()
    {
        SceneManager.LoadScene("FastLudoLoadingScene");
    }

    public void OnCricket()
    {
        SceneManager.LoadScene("CricketLoading");
    }

    public void onRummyClicked()
    {
        SceneManager.LoadScene("tournament");
    }

    public void onfastSelectClicked()
    {
        SceneManager.LoadScene("QuickSelectScene");
    }

    public void OnClassicSelectClicked()
    {
        SceneManager.LoadScene("ClassicSelectScene");
    }

    public void OnProfileClicked()
    {
        SceneManager.LoadScene("Profile");
    }

    public void On4PlayerClicked()
    {
        SceneManager.LoadScene("QuickSelect4Players");
    }

    public void On4PlayerClassicClicked()
    {
        SceneManager.LoadScene("ClassicSelect4Players");
    }
}
