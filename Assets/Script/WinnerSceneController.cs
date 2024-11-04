using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinnerSceneController : MonoBehaviour
{
    public TextMeshProUGUI winnerText;

    void Start()
    {
        string winner = PlayerPrefs.GetString("Winner");
        winnerText.text = winner + " Player Wins!";
    }
}
