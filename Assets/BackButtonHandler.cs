
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButtonHandler : MonoBehaviour
{


    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackButton();
        }
    }

    private void HandleBackButton()
    {
        
            string currentScene = SceneManager.GetActiveScene().name;

            switch (currentScene)
            {
                
                case "fastludo":
                case "classicludo":
                case "RightSpinBowler":
                case "Winner":
                case "tournament":
                case "QuickSelectScene":
                case "ClassicSelectScene":
                case "Profile":
                case "ChooseCric":
                case "AddCash":
                case "Wallet":
                case "Referrel":
                SceneManager.LoadScene("Home Page");
                    break;
                case "Sign Up":
                case "OTP":
                    SceneManager.LoadScene("Sign In");
                    break;

                case "EditProfile":
                    SceneManager.LoadScene("Profile");
                    break;

                default:
                    SceneManager.LoadScene("Home Page");
                    break;
            }
    }
}