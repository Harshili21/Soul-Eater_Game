using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RestartGame()
    {

        SceneManager.LoadScene("GamePlayScene 1"); // Change to your gameplay scene name
    }
}
