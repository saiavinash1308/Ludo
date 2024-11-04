using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BannerLoader : MonoBehaviour
{
    private string apiUrl = "https://s9g36m1m-5000.inc1.devtunnels.ms/api/banners";
    private string baseImageUrl = "https://s9g36m1m-5000.inc1.devtunnels.ms"; // Base URL for image

    public GameObject imagePrefab; // Prefab that contains an Image component
    public Transform contentPanel; // The Content of the horizontal scroll bar

    public float fixedHeight = 100f; // Fixed height for the banner
    public float spacing = 10f; // Spacing between banners

    [System.Serializable]
    public class Banner
    {
        public string imageUrl;
    }

    [System.Serializable]
    public class BannerList
    {
        public Banner[] banners;
    }

    void Start()
    {
        StartCoroutine(LoadBannersFromAPI());
    }

    IEnumerator LoadBannersFromAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error while fetching banners: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("API Response: " + jsonResponse); // Log the API response to check the structure

            Banner[] banners = JsonUtility.FromJson<BannerList>("{\"banners\":" + jsonResponse + "}").banners;

            // Calculate the total width needed for the content panel
            float totalWidth = 0;
            Texture2D[] textures = new Texture2D[banners.Length]; // Array to hold textures
            for (int i = 0; i < banners.Length; i++)
            {
                string fullImageUrl = baseImageUrl + banners[i].imageUrl; // Concatenate base URL with image URL
                Debug.Log("Downloading image from: " + fullImageUrl); // Log each image URL

                yield return StartCoroutine(DownloadTexture(fullImageUrl, (texture) =>
                {
                    textures[i] = texture;
                    if (texture != null)
                    {
                        float aspectRatio = (float)texture.width / texture.height;
                        float width = fixedHeight * aspectRatio; // Calculate width based on fixed height and aspect ratio
                        totalWidth += width + spacing; // Accumulate width and add spacing
                    }
                }));
            }

            // Set the size of the content panel to fit all banners
            RectTransform contentPanelRect = contentPanel.GetComponent<RectTransform>();
            contentPanelRect.sizeDelta = new Vector2(totalWidth, fixedHeight);

            // Now instantiate the banners
            float currentX = 0; // Start position for banners
            for (int i = 0; i < banners.Length; i++)
            {
                Texture2D texture = textures[i];
                string fullImageUrl = baseImageUrl + banners[i].imageUrl; // Concatenate base URL with image URL
                Debug.Log("Downloading image from: " + fullImageUrl); // Log each image URL
                yield return StartCoroutine(DownloadAndDisplayBanner(fullImageUrl, texture, currentX));
                currentX += fixedHeight * (float)texture.width / texture.height + spacing; // Update position for next banner
            }
        }
    }

    IEnumerator DownloadTexture(string imageUrl, System.Action<Texture2D> onTextureDownloaded)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error while downloading image: " + request.error + " from URL: " + imageUrl);
            onTextureDownloaded(null);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            onTextureDownloaded(texture);
        }
    }

    IEnumerator DownloadAndDisplayBanner(string imageUrl, Texture2D texture, float xPosition)
    {
        if (texture == null)
        {
            Debug.LogError("Texture is null. Skipping display.");
            yield break;
        }

        if (imagePrefab == null)
        {
            Debug.LogError("Image Prefab is not assigned.");
            yield break;
        }

        GameObject newBanner = Instantiate(imagePrefab, contentPanel);

        if (newBanner == null)
        {
            Debug.LogError("Failed to instantiate banner prefab.");
            yield break;
        }

        // Ensure RectTransform is set correctly
        RectTransform rectTransform = newBanner.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform component not found on the instantiated banner.");
            yield break;
        }

        // Set size and position of the RectTransform
        float aspectRatio = (float)texture.width / texture.height;
        float width = fixedHeight * aspectRatio; // Calculate width based on fixed height and aspect ratio

        rectTransform.sizeDelta = new Vector2(width, fixedHeight);
        rectTransform.anchoredPosition = new Vector2(xPosition, 0); // Set position within the content panel

        // Assuming the Image component is on the root of the prefab
        Image bannerImage = newBanner.GetComponent<Image>();
        if (bannerImage == null)
        {
            Debug.LogError("Image component not found in prefab.");
            yield break;
        }
        bannerImage.sprite = SpriteFromTexture2D(texture);
    }

    private Sprite SpriteFromTexture2D(Texture2D texture)
    {
        // Create a sprite from the texture and use its dimensions
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
