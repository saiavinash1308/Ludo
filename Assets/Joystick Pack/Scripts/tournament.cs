using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class ContestLoader : MonoBehaviour
{
    private string apiUrl = "https://s9g36m1m-5000.inc1.devtunnels.ms/api/contest";

    public GameObject contestCardPrefab; // Prefab that represents a contest card UI
    public Transform contentPanel;       // The Content of the scroll panel to hold all contest cards

    [System.Serializable]
    public class Contest
    {
        public string contestName;
        public int firstPrize;
        public int maxEntries;
        public int currentEntries;
        public int prizePool;
        public int entryFee;
        public string closingTime;
        public string currency;
        public bool isActive;
    }

    [System.Serializable]
    public class ContestList
    {
        public Contest[] contests;
    }

    void Start()
    {
        StartCoroutine(LoadContestsFromAPI());
    }

    IEnumerator LoadContestsFromAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error while fetching contests: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("API Response: " + jsonResponse);

            Contest[] contests = JsonUtility.FromJson<ContestList>("{\"contests\":" + jsonResponse + "}").contests;

            // Loop through the contest data and create cards for active contests only
            foreach (Contest contest in contests)
            {
                if (contest.isActive)
                {
                    Debug.Log("Creating card for contest: " + contest.contestName);  // Added log for clarity
                    CreateContestCard(contest);
                }
            }
        }
    }

    private void CreateContestCard(Contest contest)
    {
        if (contestCardPrefab == null)
        {
            Debug.LogError("Contest Card Prefab is not assigned.");
            return;
        }

        // Check if a card for the same contest already exists (optional check for duplicates)
        foreach (Transform child in contentPanel)
        {
            TextMeshProUGUI[] texts = child.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in texts)
            {
                if (text.name == "ContestNameText" && text.text == contest.contestName)
                {
                    Debug.LogWarning("A card for this contest already exists: " + contest.contestName);
                    return;  // Skip creating another card
                }
            }
        }

        GameObject newCard = Instantiate(contestCardPrefab, contentPanel);
        if (newCard == null)
        {
            Debug.LogError("Failed to instantiate contest card prefab.");
            return;
        }

        TextMeshProUGUI[] textComponents = newCard.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI textComponent in textComponents)
        {
            if (textComponent.name == "ContestNameText")
                textComponent.text = contest.contestName;
            else if (textComponent.name == "FirstPrizeText")
                textComponent.text = "1st Prize:   " + contest.firstPrize.ToString();
            else if (textComponent.name == "MaxEntriesText")
                textComponent.text = "Max Entries: " + contest.maxEntries.ToString();
            else if (textComponent.name == "CurrentEntriesText")
                textComponent.text = "Entries: " + contest.currentEntries.ToString();
            else if (textComponent.name == "PrizePoolText")
                textComponent.text = " " + contest.prizePool.ToString();
            else if (textComponent.name == "EntryFeeText")
                textComponent.text = "Entry Fee: " + contest.entryFee.ToString();
            else if (textComponent.name == "ClosingTimeText")
                textComponent.text = " " + FormatClosingTime(contest.closingTime);
        }

        Button[] buttons = newCard.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (button.name == "EnterContestButton")
            {
                button.onClick.AddListener(() => EnterContest(contest));
            }
        }

        Image[] images = newCard.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            if (image.name == "ContestImage")
            {
                // Set contest image logic (if applicable)
            }
        }
    }

    private void EnterContest(Contest contest)
    {
        Debug.Log("Entering contest: " + contest.contestName);
    }

    private string FormatClosingTime(string closingTime)
    {
        DateTime closingDateTime = DateTime.Parse(closingTime);
        TimeSpan timeLeft = closingDateTime - DateTime.Now;

        if (timeLeft.TotalDays >= 1)
            return $"{(int)timeLeft.TotalDays}d {(int)timeLeft.Hours}h";
        else
            return $"{(int)timeLeft.Hours}h {(int)timeLeft.Minutes}m";
    }
}
