/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DiceSelectionUI : MonoBehaviour
{
    public Button[] diceButtonPrefabs; // Array to hold different button prefabs for each dice value
    public Transform buttonContainer;  // Container to hold buttons

    public void ShowDiceOptions(List<int> rolledValues)
    {
        Debug.Log("Showing Dice Options");
        gameObject.SetActive(true); // Ensure the panel is active

        // Clear existing buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create buttons for each rolled value
        foreach (int value in rolledValues)
        {
            Debug.Log("Creating button for value: " + value); // This will log each dice value being processed
            if (value >= 1 && value <= 6)
            {
                // Instantiate the correct prefab based on the dice value
                Button newButton = Instantiate(diceButtonPrefabs[value - 1], buttonContainer);

                // Log the button's creation and its assigned value
                Debug.Log("Button instantiated: " + newButton.name + " with value: " + value);

                // Set the text for the button
                newButton.GetComponentInChildren<Text>().text = value.ToString();

                // Set up the button's click event
                newButton.onClick.AddListener(() => OnDiceSelected(value));
            }
        }

        // Make sure the panel is enabled and visible
        gameObject.SetActive(true);
    }

    void OnDiceSelected(int selectedValue)
    {
        Debug.Log("Dice value selected: " + selectedValue); // This logs the selected value
        GameManager.game.UseSelectedDiceValue(selectedValue);
        gameObject.SetActive(false); // Hide the UI after selection
    }
}
*/