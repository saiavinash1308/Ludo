/*using UnityEngine;
using TMPro;

public class StopwatchTimer : MonoBehaviour
{
    public TextMeshProUGUI stopwatchText;
    private float timer;
    private bool isRunning;

    void Start()
    {
        // Automatically find the TextMeshProUGUI object
        stopwatchText = GameObject.Find("StopwatchText").GetComponent<TextMeshProUGUI>();

        timer = 0f;
        isRunning = true; // Start the timer automatically
    }

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
            UpdateStopwatchText();
        }
    }

    void UpdateStopwatchText()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        int milliseconds = Mathf.FloorToInt((timer - minutes * 60 - seconds) * 1000);

        stopwatchText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartStopwatch()
    {
        isRunning = true;
    }

    public void StopStopwatch()
    {
        isRunning = false;
    }

    public void ResetStopwatch()
    {
        isRunning = false;
        timer = 0f;
        UpdateStopwatchText();
    }
}
*/