using UnityEngine;
using UnityEngine.UI;

public class ButtonSpriteToggler : MonoBehaviour
{
    public static ButtonSpriteToggler Instance;

    public Sprite onSprite;
    public Sprite offSprite;
    public Image buttonImage;

    public bool isOn = true;

    private const string TOGGLE_PREF_KEY = "ButtonTogglerState";  // Key to save/load state in PlayerPrefs

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (buttonImage == null)
        {
            Debug.LogError("No Image component found on the button!");
            return;
        }

        // Load the saved state from PlayerPrefs
        isOn = PlayerPrefs.GetInt(TOGGLE_PREF_KEY, 1) == 1;  // Default to 'on' if no value is saved

        // Set the initial sprite based on the saved state
        buttonImage.sprite = isOn ? onSprite : offSprite;
    }

    public void ToggleSprite()
    {
        // Toggle the isOn state
        isOn = !isOn;

        // Update the sprite
        buttonImage.sprite = isOn ? onSprite : offSprite;

        // Save the state in PlayerPrefs
        PlayerPrefs.SetInt(TOGGLE_PREF_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
