/* 
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI StopwatchText;
    private float remainingTime;
    private bool isRunning;
    private const float initialTime = 300f; // 5 minutes in seconds

    void Start()
    {
        // Automatically find the TextMeshProUGUI object by name
        StopwatchText = GameObject.Find("StopwatchText").GetComponent<TextMeshProUGUI>();

        remainingTime = initialTime;
        isRunning = true; // Start the timer automatically
    }

    void Update()
    {
        if (isRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                UpdateStopwatchText();
            }
            else
            {
                isRunning = false;
                remainingTime = 0;
                UpdateStopwatchText();
                // Optionally, you can add a callback or event here when the timer reaches zero
                OnTimerEnd();
            }
        }
    }

    void UpdateStopwatchText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60F);
        int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);
        int milliseconds = Mathf.FloorToInt((remainingTime - minutes * 60 - seconds) * 1000);

        StopwatchText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        isRunning = false;
        remainingTime = initialTime;
        UpdateStopwatchText();
    }

    void OnTimerEnd()
    {
        // This function is called when the timer reaches zero
        Debug.Log("Timer has ended!");
        // Add any additional actions here
    }
}
*/

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Add this namespace

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI stopwatchText;
    private float remainingTime;
    private bool isRunning;
    private const float initialTime = 300f; // 5 minutes in seconds

    void Start()
    {
        // Automatically find the TextMeshProUGUI object by name
        stopwatchText = GameObject.Find("StopwatchText").GetComponent<TextMeshProUGUI>();

        remainingTime = initialTime;
        isRunning = true; // Start the timer automatically
    }

    void Update()
    {
        if (isRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                UpdateStopwatchText();
            }
            else
            {
                isRunning = false;
                remainingTime = 0;
                UpdateStopwatchText();
                // Switch to another scene when the timer ends
                OnTimerEnd();
            }
        }
    }

    void UpdateStopwatchText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60F);
        int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);
        //int milliseconds = Mathf.FloorToInt((remainingTime - minutes * 60 - seconds) * 1000);

        stopwatchText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        isRunning = false;
        remainingTime = initialTime;
        UpdateStopwatchText();
    }

    void OnTimerEnd()
    {
        // This function is called when the timer reaches zero
        Debug.Log("Timer has ended!");
        // Load the specified scene (replace "NextScene" with your scene name)
        SceneManager.LoadScene("Winner");
    }
}
