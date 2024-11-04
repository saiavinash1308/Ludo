using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManagerScriptForBatsman : MonoBehaviour
{
    public static CanvasManagerScriptForBatsman instance;

    public GameObject batSwipePanel;
    public Text ballTypeButtonText;
    public Text trajectoryButtonText;
    public Text ballBounceAngleText;

    private int ballType;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateDefaultValues();
    }

    // Update UI to default values
    private void UpdateDefaultValues()
    {
        SetBallSpeedBasedOnType();
    }

    // Set the ball speed based on the selected ball type
    private void SetBallSpeedBasedOnType()
    {
        float ballSpeed = 0f;
        switch (ballType)
        {
            case 0: // Straight
                ballSpeed = Random.Range(35, 45f); // Speed range for straight ball
                break;
            case 1: // Leg Spin
            case 2: // Off Spin
                ballSpeed = Random.Range(15f, 20f); // Speed range for spin balls
                break;
        }
        BallControllerScriptForBatsman.instance.BallSpeed = ballSpeed; // Set the ball speed directly
    }

    // Called when the "Straight" ball button is pressed
    public void OnStraightBallButton()
    {
        ballType = 0;
        ballTypeButtonText.text = "Straight";
        BallControllerScriptForBatsman.instance.BallType = ballType;
        SetBallSpeedBasedOnType();
    }

    // Called when the "Leg Spin" ball button is pressed
    public void OnLegSpinBallButton()
    {
        ballType = 1;
        BallControllerScriptForBatsman.instance.BallType = ballType;
        SetBallSpeedBasedOnType();
    }

    // Called when the "Off Spin" ball button is pressed
    public void OnOffSpinBallButton()
    {
        ballType = 2;
        BallControllerScriptForBatsman.instance.BallType = ballType;
        SetBallSpeedBasedOnType();
    }

    // Called when the enable trajectory button is pressed
    public void OnTrajectoryButton()
    {
        if (BallControllerScriptForBatsman.instance.IsTrajectoryEnabled)
        {
            BallControllerScriptForBatsman.instance.IsTrajectoryEnabled = false;
            trajectoryButtonText.text = "Trajectory: Disabled";
        }
        else
        {
            BallControllerScriptForBatsman.instance.IsTrajectoryEnabled = true;
            trajectoryButtonText.text = "Trajectory: Enabled";
        }
    }

    // Enable the bat swipe panel
    public void EnableBatSwipePanel()
    {
        batSwipePanel.SetActive(true);
    }

    // Update the ball's bounce text
    public void UpdateBallsBounceAngleUI(float angle)
    {
        ballBounceAngleText.text = "After Bounce Angle: " + angle.ToString("##.##");
    }

    // Reset everything to default except UI
    public void OnReset()
    {
        BallControllerScriptForBatsman.instance.ResetBall();
        cameraControllerScriptForBatsman.instance.ResetCamera();
        StumpsControllerScript.instance.ResetStumps();
        UpdateDefaultValues();
        batSwipePanel.SetActive(false);
    }

    // Called when the switch side of the ball button is pressed
    public void OnSwitchSide()
    {
        BallControllerScript.instance.SwitchSide();
    }
}
