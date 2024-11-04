using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallTriggerHandler : MonoBehaviour
{
    public float resetDelay = 3f;  // Default delay before reset, adjustable in the editor

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the ball
        if (other.CompareTag("Ball"))  // Assuming your ball has the tag "Ball"
        {
            StartCoroutine(ResetAfterDelay());
        }
    }

    private IEnumerator ResetAfterDelay()
    {
        // Wait for the specified delay before resetting
        yield return new WaitForSeconds(resetDelay);

        // Call the reset function from CanvasManagerScript or other necessary logic
        

        // Get the active scene name
        string activeSceneName = SceneManager.GetActiveScene().name;

        // Update the score based on the active scene
        if (activeSceneName == "batsmanScene")
        {
            CanvasManagerScriptForBatsman.instance.OnReset();
        }
        else if (activeSceneName == "RightSpinBowler")
        {
            CanvasManagerScript.instance.OnReset();
        }
    }
}
