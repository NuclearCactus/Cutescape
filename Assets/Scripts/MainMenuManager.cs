using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene to load when starting game")]
    public string finalSceneName = "FinalScene";

    // Called by the "Start Game" button
    public void StartGame()
    {
        SceneManager.LoadScene(finalSceneName);
    }

    // Called by the "Quit Game" button
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}