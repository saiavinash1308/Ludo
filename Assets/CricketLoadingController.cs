using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CricketLoadingController : MonoBehaviour
{
    // Duration the splash screen will be visible
    public float splashDuration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to wait for 3 seconds
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    // Coroutine to handle the delay
    IEnumerator LoadNextSceneAfterDelay()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(splashDuration);

        // Set the orientation to landscape
        

        // Load the next scene (MainMenu or any other scene)
        SceneManager.LoadScene("ChooseCric"); // Ensure this matches the name of your next scene
        
    }
}
