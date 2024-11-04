using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerDraggerForBatsman : MonoBehaviour
{
    public GameObject marker; // marker game object
    public float boundaryPointX; // max x value the marker can cover
    public float customX = 529.97f;
    public float minBoundaryPointZ; // min z value the marker can cover
    public float maxBoundaryPointZ; // max z value the marker can cover

    public float scaleDownDragBy = 0.1f; // scale down input by this value

    public static MarkerDraggerForBatsman instance;

    void Awake()
    {
        instance = this;
        Input.multiTouchEnabled = false; // disable multitouch

#if PLATFORM_ANDROID // if the platform is android change the scaleDownDragBy value
        scaleDownDragBy = 0.2f;
#endif

        // Randomly assign the marker's initial position within the defined boundaries
        SetRandomMarkerPosition();
    }

    public void SetRandomMarkerPosition()
    {
        // Generate a random position within the boundary limits
        float randomX = Random.Range(customX, boundaryPointX);
        float randomZ = Random.Range(minBoundaryPointZ, maxBoundaryPointZ);

        // Set the marker's position to the random values
        marker.transform.position = new Vector3(randomX, marker.transform.position.y, randomZ);
    }

    void Update()
    {
        // You can still check for conditions here if needed, 
        // but the marker's position is now set randomly in Awake()
    }
}
