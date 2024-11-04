using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallControllerScript : MonoBehaviour
{
    public static BallControllerScript instance;

    public Vector3 defaultPosition; // ball's default beginning position
    public GameObject marker; // stores the marker game object
    public float ballSpeed; // speed of the ball
    public GameObject ball; // stores the ball game object
    public float bounceScalar; // the bounce scalar value to scale the bounce angle after the ball hits the ground
    public float spinScalar; // the ball's spin scalar value
    public float realWorldBallSpeed; // the ball's speed to display on the UI which corresponds to the real world units(kmph)
    public GameObject trajectoryHolder; // the holder game object to parent each trajectory ball object to

    public int ballType; // the balls type; 0 - straight, 1 - leg spin, 2 - off spin

    private float angle; // the bounce angle of the ball after the ball hits the ground for the first time
    private Vector3 startPosition; // ball's startPosition for the lerp function
    private Vector3 targetPosition; // ball's targetPosition for the lerp function
    private Vector3 direction; // the direction vector the ball is going in
    private Rigidbody rb; // rigidbody of the ball
    private float spinBy; // value to spin the ball by

    private bool firstBounce; // whether ball's hit the ground once or not
    private bool isBallThrown; // whether the ball is thrown or not
    private bool isBallHit; // whether the bat hit the ball
    private bool isTrajectoryEnabled; // whether the trajectory is enabled or disabled

    private bool isLeftSide = true; // track which side the ball is on, default to left side

    private Vector3 leftBowlerPosition; // initial position of the bowler on the left side
    private Vector3 rightBowlerPosition; // initial position of the bowler on the right side

    public float BallSpeed { set { ballSpeed = value; } }

    // Public property with a getter
    public int BallType
    {
        get { return ballType; }
        set { ballType = value; }
    }
    public bool IsBallThrown { get { return isBallThrown; } }
    public bool IsTrajectoryEnabled { set { isTrajectoryEnabled = value; } get { return isTrajectoryEnabled; } }
    public bool IsBallHit { get { return isBallHit; } }

    private Vector3 initialScale; // To store the original scale of the ball
    public GameObject Bowler; // Reference to the bowler GameObject

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        defaultPosition = transform.position; // set defaultPosition to the ball's beginning position
        rb = gameObject.GetComponent<Rigidbody>();
        startPosition = transform.position; // set the startPosition to the ball's beginning position

        // Set the initial scale of the ball
        initialScale = transform.localScale;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // Scale down to 0.1 in x, y, z

        // Initialize bowler positions
        leftBowlerPosition = Bowler.transform.position;
        rightBowlerPosition = new Vector3(-leftBowlerPosition.x, leftBowlerPosition.y, leftBowlerPosition.z);
    }

    void Update()
    {
        // Toggle trajectory when space bar is pressed and the ball has already been thrown
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsBallThrown)
            {
                IsTrajectoryEnabled = !IsTrajectoryEnabled;
            }
            else
            {
                StartCoroutine(ThrowBallAfterDelay(GetDelayBasedOnButton())); // Start coroutine to throw ball after the determined delay
            }
        }

        // Check if left or right arrow key is pressed to switch sides
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!isLeftSide)
            {
                SwitchSide();
                isLeftSide = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isLeftSide)
            {
                SwitchSide();
                isLeftSide = false;
            }
        }

        // If the isTrajectoryEnabled is set to true and the ball's velocity is greater than 0
        // i.e., it's in motion then instantiate trajectory balls at each frame
        if (rb.velocity.magnitude > 0 && isTrajectoryEnabled)
        {
            GameObject trajectoryBall = Instantiate(ball, transform.position, Quaternion.identity) as GameObject;
            trajectoryBall.transform.SetParent(trajectoryHolder.transform); // set the instantiated trajectory ball's parent to the trajectoryHolder object

            // Reset the trajectory ball's scale to the original ball's scale
            trajectoryBall.transform.localScale = initialScale;
        }
    }

    private float GetDelayBasedOnButton()
    {
        if (Bowlermanager.instance.straightButtonEnable)
        {
            return 4.3f; // Shorter delay for scenes with "Fast" in the name
        }
        else if (Bowlermanager.instance.LegspinnButtonEnable)
        {
            return 2.3f; // Medium delay for scenes with "Flow" in the name
        }
        else if (Bowlermanager.instance.OffspinButtonEnable)
        {
            return 1.8994f; // Medium delay for scenes with "Flow" in the name
        }
        else
        {
            return 1.85f; // Default delay
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isBallHit && collision.gameObject.CompareTag("Ground"))
        {
            switch (ballType)
            {
                case 0:
                    spinBy = direction.x; // don't change spinBy 
                    break;
                case 1:
                    spinBy = spinScalar / ballSpeed; // change spinBy to a positive value based on the spinScalar value and the ball's speed
                    break;
                case 2:
                    spinBy = -spinScalar / ballSpeed; // change spinBy to a negative value based on the spinScalar value and the ball's speed
                    break;
            }

            if (!firstBounce)
            {
                firstBounce = true;
                rb.useGravity = true;

                direction = new Vector3(spinBy, -direction.y * (bounceScalar * ballSpeed), direction.z);
                direction = Vector3.Normalize(direction);

                angle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;

                rb.AddForce(direction * ballSpeed, ForceMode.Impulse);
                CanvasManagerScript.instance.UpdateBallsBounceAngleUI(angle);
            }
            AudioManagerScript.instance.PlayBounceAudio();
        }

        if (collision.gameObject.CompareTag("Stump"))
        {
            AudioManagerScript.instance.PlayBatHitAudio();
            collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void ThrowBall()
    {
        if (!IsBallThrown)
        {
            isBallThrown = true;
            CanvasManagerScript.instance.EnableBatSwipePanel();
            targetPosition = marker.transform.position;
            direction = Vector3.Normalize(targetPosition - startPosition);
            rb.AddForce(direction * ballSpeed, ForceMode.Impulse);

            // Reset the ball's scale to its original size
            transform.localScale = initialScale;

            // Notify GameManager that a ball has been thrown
            Gamemanager.instance.RegisterThrow();
        }
    }

    public void HitTheBall(Vector3 hitDirection, float batSpeed)
    {
        CameraControllerScript.instance.IsBallHit = true;
        isBallHit = true;
        rb.velocity = Vector3.zero;
        direction = Vector3.Normalize(hitDirection);
        float hitSpeed = ballSpeed + batSpeed;
        rb.AddForce(-direction * hitSpeed, ForceMode.Impulse);
        if (!firstBounce)
        {
            rb.useGravity = true;
        }

        BattingAnimationController.instance.PlayBattingAnimation(1);
    }

    public void SwitchSide()
    {
        // Switch the ball's position
        transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        defaultPosition = transform.position;
        startPosition = transform.position;

        // Switch the bowler's position
        if (isLeftSide)
        {
            Bowler.transform.position = rightBowlerPosition;
        }
        else
        {
            Bowler.transform.position = leftBowlerPosition;
        }
    }

    public void OnThrowButtonClick()
    {
        StartCoroutine(ThrowBallAfterDelay(GetDelayBasedOnButton()));
    }

    private IEnumerator ThrowBallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ThrowBall();
    }

    public void ResetBall()
    {
        firstBounce = false;
        isBallHit = false;
        isBallThrown = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        transform.position = defaultPosition;

        // Reset the ball's scale to 0.1 in x, y, z
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Gamemanager.instance.ResetHighestBoundaryAndFinalizeScore();


    }
}
