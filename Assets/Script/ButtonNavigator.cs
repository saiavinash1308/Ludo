using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonNavigator : MonoBehaviour
{
    public void OnClassicButton()
    {
        SceneManager.LoadScene("classicludo");
    }

    public void OnFastButton()
    {
        SceneManager.LoadScene("fastludo");
    }
}
