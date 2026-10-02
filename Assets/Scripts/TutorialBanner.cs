using UnityEngine;
using UnityEngine.UI;

public class TutorialBanner : MonoBehaviour
{
    public GameObject banner; // Assign in the Inspector

    void Start()
    {
        banner.SetActive(true); // Make sure the banner is visible at the start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Example: Hide banner when player jumps
        {
            HideBanner();
        }
    }

    public void HideBanner()
    {
        banner.SetActive(false); // Hide the banner
    }
}
