using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCamera : MonoBehaviour
{
    // List of cameras to switch between
    public List<Camera> cameras; // Assign these cameras in the Inspector
    private Canvas canvasComponent;
    private int currentCameraIndex = 0;

    void Start()
    {
        // Get the Canvas component attached to this GameObject
        canvasComponent = GetComponent<Canvas>();

        // Check if cameras are assigned and set the initial render camera
        if (cameras != null && cameras.Count > 0)
        {
            canvasComponent.worldCamera = cameras[currentCameraIndex]; // Set the initial camera
        }
        else
        {
            Debug.LogError("No cameras assigned to the cameras list.");
        }
    }

    // Method to switch cameras using the camera index
    public void SwitchCamera(int cameraIndex)
    {
        // Check if the camera index is valid
        if (cameraIndex >= 0 && cameraIndex < cameras.Count)
        {
            currentCameraIndex = cameraIndex;

            // Update the canvas's render camera
            canvasComponent.worldCamera = cameras[currentCameraIndex];

            // Optional: Log the name of the camera that was switched to
            Debug.Log("Switched to camera: " + cameras[currentCameraIndex].name);
        }
        else
        {
            Debug.LogError("Invalid camera index: " + cameraIndex);
        }
    }

    // Method to reset all cameras and switch to the main camera after a delay
    public IEnumerator ResetToMainCamera(float delay)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // Reset all cameras and activate the main camera (index 0)
        foreach (var cam in cameras)
        {
            cam.gameObject.SetActive(false); // Disable all cameras
        }

        // Activate the main camera (assuming index 0 is the main camera)
        cameras[0].gameObject.SetActive(true);
        canvasComponent.worldCamera = cameras[0]; // Set the main camera as the render camera
    }

    // Call this method to start the coroutine and reset the cameras after a delay
    public void ResetCamerasAfterDelay(float delay)
    {
        StartCoroutine(ResetToMainCamera(delay));
    }
}
