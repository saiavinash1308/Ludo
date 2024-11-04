using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundaryScoreHandler : MonoBehaviour
{
    [Header("Score Value")]
    public int boundaryScoreValue;  // Set this to 2, 3, 4, etc., in the inspector for each boundary object

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the boundary is the ball
        if (other.CompareTag("Ball"))  // Ensure the ball is tagged as "Ball"
        {
            // Get the active scene name
            string activeSceneName = SceneManager.GetActiveScene().name;

            // Update the score based on the active scene
            if (activeSceneName == "batsmanScene")
            {
                GamemanagerForBatsMan.instance.UpdateHighestBoundary(boundaryScoreValue);
            }
            else if (activeSceneName == "RightSpinBowler")
            {
                Gamemanager.instance.UpdateHighestBoundary(boundaryScoreValue);
            }
        }
    }
}
