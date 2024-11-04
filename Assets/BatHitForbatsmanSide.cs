using UnityEngine;
using System.Collections;

public class BatHitForbatsmanSide : MonoBehaviour
{
    public static BatHitForbatsmanSide instance;
    public float minHitForce = 50f;  // Minimum force applied to the ball
    public float maxHitForce = 100f; // Maximum force applied to the ball
    public Transform batTransform;   // Bat's transform to determine the hit direction
    public Camera ballCam;
    public float ballCamDuration = 2f;  // Duration in seconds for the ball cam to stay active
    public float ballCamDurationForStart = 1f;

    private void Awake()
    {
        instance = this;
    }

    // Function to manually trigger the ball hit
    public void HitBallManually(Collider ballCollider, int shotType)
    {
        Rigidbody ballRigidbody = ballCollider.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            // Play the corresponding batting animation
            BattingAnimationController.instance.PlayBattingAnimation(shotType);

            // Apply force based on the shot type
            if (shotType == BattingAnimationController.instance.legShotType)
            {
                ApplyLegShotForce(ballRigidbody);
            }
            else if (shotType == BattingAnimationController.instance.offShotType)
            {
                ApplyOffShotForce(ballRigidbody);
            }
            else
            {
                ApplyStraightShotForce(ballRigidbody);
            }

            // Play hit sound
            AudioManagerScript.instance.PlayBatHitAudio();

            // Activate ball camera for a brief period
            Debug.Log("Activating Ball Cam");
            StartCoroutine(ActivateBallCam());
        }
    }

    // Function to apply force for a straight shot
    public void ApplyStraightShotForce(Rigidbody ballRigidbody)
    {
        Vector3 hitDirection = batTransform.forward; // Straight hit
        float randomHitForce = Random.Range(minHitForce, maxHitForce);
        ballRigidbody.AddForce(hitDirection * randomHitForce, ForceMode.Impulse);
    }

    // Function to apply force for a leg-side shot
    public void ApplyLegShotForce(Rigidbody ballRigidbody)
    {
        Vector3 hitDirection = -batTransform.right; // Leg-side hit (player's left)
        float randomHitForce = Random.Range(minHitForce, maxHitForce);
        ballRigidbody.AddForce(hitDirection * randomHitForce, ForceMode.Impulse);
    }

    // Function to apply force for an off-side shot
    public void ApplyOffShotForce(Rigidbody ballRigidbody)
    {
        Vector3 hitDirection = batTransform.right; // Off-side hit (player's right)
        float randomHitForce = Random.Range(minHitForce, maxHitForce);
        ballRigidbody.AddForce(hitDirection * randomHitForce, ForceMode.Impulse);
    }

    public void ActivateBallCamera()
    {
        StartCoroutine(ActivateBallCam());
    }

    public IEnumerator ActivateBallCam()
    {
        yield return new WaitForSeconds(ballCamDurationForStart);
        // Activate ball camera
        ballCam.gameObject.SetActive(true);

        // Wait for the specified duration
        yield return new WaitForSeconds(ballCamDuration);

        // Deactivate the ball camera
        ballCam.gameObject.SetActive(false);
    }

    // Optional: Debugging tool to visualize the hit range
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(batTransform.position, 1.5f); // Visualize a hit range of 1.5f for reference
    }
}
