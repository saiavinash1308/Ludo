using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserDataLoader : MonoBehaviour
{
    public string APIUrl = "https://s9g36m1m-5000.inc1.devtunnels.ms/api/auth/me"; // Ensure this is correct and matches the Postman URL

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI emailText;
    public TextMeshProUGUI totalMatchesText;
    public TextMeshProUGUI wonMatchesText;
    public TextMeshProUGUI totalEarningsText;
    public Image avatarImage;

    [System.Serializable]
    public class User
    {
        public string name;
        public string email;
        public int totalMatches;
        public int wonMatch;
        public int totalEarning;
        public string avatar;
    }

    void Start()
    {
        StartCoroutine(FetchUserData());
    }

    IEnumerator FetchUserData()
    {
        string userToken = PlayerPrefs.GetString("authToken", null);

        if (string.IsNullOrEmpty(userToken))
        {
            Debug.LogError("User token is missing or invalid.");
            yield break;
        }

        // Log the token for debugging
        Debug.Log("User Token: " + userToken);

        // Create a new UnityWebRequest to fetch the user data
        UnityWebRequest request = UnityWebRequest.Get(APIUrl);

        // Set the Authorization header
        request.SetRequestHeader("Authorization", "Bearer " + userToken);
        Debug.Log("Authorization Header Set: Bearer " + userToken);

        // Set request timeout to 30 seconds
        request.timeout = 30;

        // Bypass SSL certificate validation (for testing purposes)
        request.certificateHandler = new BypassCertificate();

        // Send the web request and wait for a response
        yield return request.SendWebRequest();

        // Log the exact URL being used and response code for debugging
        Debug.Log("Request URL: " + APIUrl);
        Debug.Log("Response Code: " + request.responseCode);

        // Check for any connection errors or API issues
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching user data: " + request.error + " | Response Code: " + request.responseCode);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("API Response: " + jsonResponse);

            // Parse the user data from JSON
            User user = JsonUtility.FromJson<User>(jsonResponse);
            if (user != null)
            {
                UpdateUI(user);

                // Store user data in PlayerPrefs for future use
                PlayerPrefs.SetString("userName", user.name);
                PlayerPrefs.SetString("userEmail", user.email);
                PlayerPrefs.SetInt("totalMatches", user.totalMatches);
                PlayerPrefs.SetInt("wonMatches", user.wonMatch);
                PlayerPrefs.SetInt("totalEarnings", user.totalEarning);
                PlayerPrefs.SetString("avatar", user.avatar);
                PlayerPrefs.Save(); // Ensure data is saved
            }
            else
            {
                Debug.LogError("Failed to parse user data.");
            }
        }
    }

    void UpdateUI(User user)
    {
        Debug.Log("Updating UI with user data");
        nameText.text = user.name;
        emailText.text = user.email;
        totalMatchesText.text = " " + user.totalMatches.ToString();

        // Update the won matches text
        wonMatchesText.text = " " + user.wonMatch.ToString();

        totalEarningsText.text = " " + user.totalEarning.ToString();

        // Load avatar image if available
        if (!string.IsNullOrEmpty(user.avatar))
        {
            StartCoroutine(LoadAvatarImage(user.avatar));
        }
    }

    IEnumerator LoadAvatarImage(string url)
    {
        UnityWebRequest avatarRequest = UnityWebRequestTexture.GetTexture(url);
        yield return avatarRequest.SendWebRequest();

        if (avatarRequest.result != UnityWebRequest.Result.ConnectionError && avatarRequest.result != UnityWebRequest.Result.ProtocolError)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(avatarRequest);
            avatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError("Error loading avatar image: " + avatarRequest.error);
        }
    }

    // Custom certificate handler to bypass SSL validation (for testing only)
    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true; // Accept all certificates (do not use in production)
        }
    }
}
