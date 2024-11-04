using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonSettingsPage : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("Home Page");
    }
}
