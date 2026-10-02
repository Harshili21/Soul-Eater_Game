using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipVideo : MonoBehaviour
{
    public void SkipToGame()
    {
        SceneManager.LoadScene("GamePlayScene 1");
    }
}
