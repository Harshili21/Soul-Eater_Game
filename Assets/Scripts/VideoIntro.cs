using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoIntro : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.Play();
        videoPlayer.loopPointReached += EndReached; // Detect when video finishes
    }

    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene("GamePlayScene 1"); // Load your main game scene
    }
}
