using UnityEngine;
using System.Collections;

public class BatHitScript : MonoBehaviour
{
    public static BatHitScript instance;
    public float minHitForce = 50f;  // Minimum force applied to the ball
    public float maxHitForce = 100f; // Maximum force applied to the ball
    public float hitRange = 1.5f;    // Range in which the bat can hit the ball
    public Transform batTransform;   // Bat's transform to determine the hit direction
    public Transform playerTransform; // Player's transform to determine ball side
    public float hitDelay = 0.5f;    // Delay before the bat hits the ball (in seconds)
    public Camera ballCam;
    public float ballCamDuration = 2f;  // Duration in seconds for the ball cam to stay active

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        CheckBallInRange();
    }

    private void CheckBallInRange()
    {
        // Find all colliders within the hit range
        Collider[] hitColliders = Physics.OverlapSphere(batTransform.position, hitRange);
        foreach (Collider collider in hitColliders)
        {
            // If a ball is in range
            if (collider.CompareTag("Ball"))
            {
                // Start a coroutine to hit the ball after a delay
                StartCoroutine(HitBallAfterDelay(collider));
            }
        }
    }

    private IEnumerator HitBallAfterDelay(Collider ballCollider)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(hitDelay);

        // Perform the hit after the delay
        Rigidbody ballRigidbody = ballCollider.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            // Get the ball's x position
            Vector3 ballPosition = ballCollider.transform.position;
            float ballX = ballPosition.x;

            // Decide the shot type based on the ball's x position in relation to 530
            int shotType;

            if (ballX > 530) // Ball is near the player, opt for leg-side shot
            {
                shotType = BattingAnimationController.instance.legShotType;
            }
            else if (ballX < 530) // Ball is far from the player, opt for off-side shot
            {
                shotType = BattingAnimationController.instance.offShotType;
            }
            else
            {
                shotType = BattingAnimationController.instance.straightShotType; // Ball is close enough, opt for straight shot
            }

            // Play the corresponding batting animation
            BattingAnimationController.instance.PlayBattingAnimation(shotType);

            // Determine the hit direction based on the shot type
            Vector3 hitDirection;
            if (shotType == BattingAnimationController.instance.legShotType)
            {
                hitDirection = -batTransform.right; // Leg-side hit (player's left)
            }
            else if (shotType == BattingAnimationController.instance.offShotType)
            {
                hitDirection = batTransform.right; // Off-side hit (player's right)
            }
            else
            {
                hitDirection = batTransform.forward; // Straight hit
            }

            // Generate a random force between minHitForce and maxHitForce
            float randomHitForce = Random.Range(minHitForce, maxHitForce);

            // Apply the random force to the ball
            ballRigidbody.AddForce(hitDirection * randomHitForce, ForceMode.Impulse);

            // Play hit sound
            AudioManagerScript.instance.PlayBatHitAudio();

            // Wait for 1 second before activating the ball camera
            yield return new WaitForSeconds(1f);

            // Activate ball camera
            ballCam.gameObject.SetActive(true);

            // Start coroutine to deactivate the camera after a delay
            StartCoroutine(DeactivateBallCamAfterDelay());
        }
    }

    private IEnumerator DeactivateBallCamAfterDelay()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(ballCamDuration);

        // Deactivate the ball camera
        ballCam.gameObject.SetActive(false);
    }

    // Optional: Debugging tool to visualize the hit range
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(batTransform.position, hitRange);
    }
}
