


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveGameObjects : MonoBehaviour
{
    private Camera mainCamera;


    void start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y,0);
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero) * 100;
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height)) * 100;
        Vector3 screenSize = topRight - bottomLeft;
        float screenRatio = screenSize.x / screenSize.y;
        float desiredRatio = transform.localScale.x / transform.localScale.y;


    if (screenRatio > desiredRatio)
        {
            float height = screenSize.y;
            transform.localScale = new Vector3(height * desiredRatio, height);

        }
        else
        {
            float width = screenSize.x;
            transform.localScale = new Vector3 (width, width / desiredRatio);
        }
    }
}


/*
using UnityEngine;

public class ResponsiveCamera : MonoBehaviour
{
    public float targetAspectRatio = 16f / 9f;  // Default to 16:9 aspect ratio

    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        // Calculate the current aspect ratio of the screen
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        // Compare the current aspect ratio to the target
        if (currentAspectRatio >= targetAspectRatio)
        {
            // Screen is wider than the target aspect ratio (or equal)
            _camera.orthographicSize = 5f; // This value can be the default orthographic size or calculated as needed
        }
        else
        {
            // Screen is taller (narrower aspect ratio)
            float differenceInSize = targetAspectRatio / currentAspectRatio;
            _camera.orthographicSize = 5f * differenceInSize;
        }
    }

    void Update()
    {
        // Optionally update the camera size if the screen size changes during runtime
        AdjustCameraSize();
    }
}
*/