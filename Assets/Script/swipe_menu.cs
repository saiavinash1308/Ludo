using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject scrollbar;
    public float swipeDelay = 2f; // Delay before the next swipe
    public float swipeSpeed = 1f; // Speed of the scrolling effect
    private float[] positions;
    private int currentIndex = 0;

    void Start()
    {
        // Initialize positions based on the number of children
        positions = new float[transform.childCount];
        float distance = 1f / (positions.Length - 1f);
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = distance * i;
        }

        // Start the auto-swipe coroutine
        StartCoroutine(AutoSwipe());
    }

    void Update()
    {
        // Update the scrollbar value based on user input
        if (Input.GetMouseButton(0))
        {
            scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, scrollbar.GetComponent<Scrollbar>().value, swipeSpeed);
        }
        else
        {
            // Update current index based on scrollbar position
            UpdateCurrentIndex();
            SmoothScale();
        }
    }

    private void UpdateCurrentIndex()
    {
        float scrollPos = scrollbar.GetComponent<Scrollbar>().value;

        for (int i = 0; i < positions.Length; i++)
        {
            if (scrollPos < positions[i] + (1f / (positions.Length - 1f)) / 2 && scrollPos > positions[i] - (1f / (positions.Length - 1f)) / 2)
            {
                currentIndex = i;
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, positions[i], swipeSpeed);
                break;
            }
        }
    }

    private void SmoothScale()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == currentIndex)
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), swipeSpeed);
            }
            else
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(0.8f, 0.8f), swipeSpeed);
            }
        }
    }

    private IEnumerator AutoSwipe()
    {
        while (true)
        {
            // Wait for the specified delay
            yield return new WaitForSeconds(swipeDelay);

            // Move to the next index
            currentIndex = (currentIndex + 1) % positions.Length;

            // Smoothly transition to the next position
            while (Mathf.Abs(scrollbar.GetComponent<Scrollbar>().value - positions[currentIndex]) > 0.01f)
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, positions[currentIndex], swipeSpeed);
                yield return null; // Wait for the next frame
            }

            // Ensure it snaps to the correct position
            scrollbar.GetComponent<Scrollbar>().value = positions[currentIndex];
        }
    }
}
