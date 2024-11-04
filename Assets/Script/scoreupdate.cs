using UnityEngine;
using TMPro;

public class scoreupdate : MonoBehaviour
{
    public TextMeshProUGUI greenscore;
    public TextMeshProUGUI yellowscore;
    public TextMeshProUGUI redscore;
    public TextMeshProUGUI bluescore;

    void Start()
    {
        // Automatically find the TextMeshProUGUI objects by name
        greenscore = GameObject.Find("greenscore").GetComponent<TextMeshProUGUI>();
        yellowscore = GameObject.Find("yellowscore").GetComponent<TextMeshProUGUI>();
        redscore = GameObject.Find("redscore").GetComponent<TextMeshProUGUI>();
        bluescore = GameObject.Find("bluescore").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        UpdateScores();
    }

    void UpdateScores()
    {
        // Convert the integer to a string and assign it to the text property
        greenscore.text = GameManager.game.greenpoints.ToString();
        yellowscore.text = GameManager.game.yellowpoints.ToString();
        redscore.text = GameManager.game.redpoints.ToString();
        bluescore.text = GameManager.game.bluepoints.ToString();
    }
}