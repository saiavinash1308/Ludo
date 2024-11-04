using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{
    public static CameraControllerScript instance;

    public Vector3 defaultPosition; // default position of the camera
    public float resetSpeed = 2.0f; // speed at which the camera resets to the default position

    // Adding references to the cameras
    public GameObject bowlerCamera;
    public Camera mainCamera;
    public GameObject outfitCamera;
    public GameObject ballCam;

    private bool isBallHit; // whether the ball was hit

    public bool IsBallHit
    {
        set
        {
            isBallHit = value;
        }
    }

    void Awake()
    {
        instance = this;
        ResetCamera(); // reset camera to default position
    }

    void Start()
    {
        // Assign references from Bowlermanager and Gamemanager
        bowlerCamera = Bowlermanager.instance.BowlerCamera;
        mainCamera = Camera.main;
        outfitCamera = Gamemanager.instance.Outfitcam;

        // Set default camera to mainCamera
        ActivateMainCamera();
    }

    void Update()
    {
        if (isBallHit)
        {
            // Smoothly move the camera back to the default position after the ball is hit
            transform.position = Vector3.Lerp(transform.position, defaultPosition, resetSpeed * Time.deltaTime);
        }
    }

    // Method to toggle to Bowler Camera
    public void ActivateBowlerCamera()
    {
        bowlerCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        outfitCamera.gameObject.SetActive(false);
        ballCam.gameObject.SetActive(false);
    }

    // Method to toggle to Main Camera
    public void ActivateMainCamera()
    {
        bowlerCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        outfitCamera.gameObject.SetActive(false);
        ballCam.gameObject.SetActive(false);
    }

    // Method to toggle to Outfit Camera
    public void ActivateOutfitCamera()
    {
        bowlerCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        outfitCamera.gameObject.SetActive(true);
        ballCam.gameObject.SetActive(false);
    }

    // Method to toggle to Ball Camera
    public void ActivateBallCam()
    {
        bowlerCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        outfitCamera.gameObject.SetActive(false);
        ballCam.gameObject.SetActive(true);
    }

    // UI button callbacks to toggle cameras
    public void OnBowlerCameraButtonPressed()
    {
        ActivateBowlerCamera();
    }

    public void OnMainCameraButtonPressed()
    {
        ActivateMainCamera();
    }

    public void OnOutfitCameraButtonPressed()
    {
        ActivateOutfitCamera();
    }

    // Resets the camera to the default position
    public void ResetCamera()
    {
        isBallHit = false; // reset isBallHit to false
        transform.position = defaultPosition; // move camera to default position
    }
}
