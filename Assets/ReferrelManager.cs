using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
public class ReferralManager : MonoBehaviour
{
    public TextMeshProUGUI referralLinkText;
    private string referralLinkApiUrl = "https://s9g36m1m-5000.inc1.devtunnels.ms/api/auth/invite";

    // This token should be stored securely after login
    private string authToken;

    private void Start()
    {
        // Assuming you retrieve the token after login and store it in PlayerPrefs
        authToken = PlayerPrefs.GetString("authToken");
    }

    public void GetReferralLink()
    {
        StartCoroutine(FetchReferralLink());
    }

    private IEnumerator FetchReferralLink()
    {
        UnityWebRequest request = UnityWebRequest.Get(referralLinkApiUrl);

        // Adding the Authorization header with the Bearer token
        request.SetRequestHeader("Authorization", "Bearer " + authToken); // Ensure that "Bearer" is part of the value

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching referral link: " + request.error);
        }
        else
        {
            string response = request.downloadHandler.text;

            // Assuming the response contains the referral link in 'inviteLink' key
            string inviteLink = JsonUtility.FromJson<InviteResponse>(response).inviteLink;
            
            referralLinkText.text = inviteLink;
        }
    }

    [System.Serializable]
    public class InviteResponse
    {
        public string inviteLink;
    }
}
