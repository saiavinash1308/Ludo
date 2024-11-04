
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backfast : MonoBehaviour
{
    public GameObject dialogBox; // Reference to the Panel (Dialog Box)
    public GameObject closeButton;
    public GameObject rulesPanel;
    public GameObject blurImage;

    void Start()
    {
        // Hide the dialog box at the start
        dialogBox.SetActive(false);
        rulesPanel.SetActive(false);
        blurImage.SetActive(false);
    }

    public void OpenDialogBox()
    {
        // Toggle dialog box visibility
        dialogBox.SetActive(true);
    }

    public void openRulesPanel()
    {
        rulesPanel.SetActive(true);
        blurImage.SetActive(true);

    }

    public void clostRulesPanel()
    {
        rulesPanel.SetActive(false);
        blurImage.SetActive(false);
        dialogBox.SetActive(false);
    }

    public void CloseButton()
    {
        dialogBox.SetActive(false);
    }
}