using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public GameObject blueArrow;
    public Transform blueArrowPosition;

    public GameObject redArrow;
    public Transform redArrowPosition;

    public GameObject greenArrow;
    public Transform greenArrowPosition;

    public GameObject yellowArrow;
    public Transform yellowArrowPosition;

    public void SetArrowVisibility(RollingDice currentDice)
    {
        // Disable all arrows initially
        blueArrow.SetActive(false);
        redArrow.SetActive(false);
        greenArrow.SetActive(false);
        yellowArrow.SetActive(false);

        // Enable the arrow corresponding to the current player's dice and set its position
        if (currentDice == GameManager.game.manageRolingDice[0])
        {
            blueArrow.transform.position = blueArrowPosition.position;
            blueArrow.SetActive(true);
        }
        else if (currentDice == GameManager.game.manageRolingDice[1])
        {
            redArrow.transform.position = redArrowPosition.position;
            redArrow.SetActive(true);
        }
        else if (currentDice == GameManager.game.manageRolingDice[2])
        {
            greenArrow.transform.position = greenArrowPosition.position;
            greenArrow.SetActive(true);
        }
        else if (currentDice == GameManager.game.manageRolingDice[3])
        {
            yellowArrow.transform.position = yellowArrowPosition.position;
            yellowArrow.SetActive(true);
        }
    }
}
