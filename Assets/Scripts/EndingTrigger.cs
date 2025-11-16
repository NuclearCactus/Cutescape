using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTrigger : MonoBehaviour
{
    [Header("Ending Scenes")]
    public string goodEndingSceneName = "GoodEndingVideoScene Sound";  // Scene name for good ending
    public string sadEndingSceneName = "BadEndingVideoScene Sound";    // Scene name for sad ending

    [Header("Settings")]
    public bool requireMoreRealForGoodEnding = true;   // If true, more Real = Good, more Cute = Sad

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DetermineEnding();
        }
    }

    void DetermineEnding()
    {
        if (ConnectionTracker.Instance == null)
        {
            Debug.LogError("ConnectionTracker.Instance is null! Cannot determine ending.");
            return;
        }

        int realConnections = ConnectionTracker.Instance.realWorldConnections;
        int cuteConnections = ConnectionTracker.Instance.cuteWorldConnections;

        Debug.Log($"Ending triggered! Real: {realConnections}, Cute: {cuteConnections}");

        // Determine which ending to load
        if (requireMoreRealForGoodEnding)
        {
            // More Real connections = Good ending
            if (realConnections > cuteConnections)
            {
                LoadGoodEnding();
            }
            else
            {
                LoadSadEnding();
            }
        }
        else
        {
            // More Cute connections = Good ending
            if (cuteConnections > realConnections)
            {
                LoadGoodEnding();
            }
            else
            {
                LoadSadEnding();
            }
        }
    }

    void LoadGoodEnding()
    {
        Debug.Log("Loading Good Ending!");
        SceneManager.LoadScene(goodEndingSceneName);
    }

    void LoadSadEnding()
    {
        Debug.Log("Loading Sad Ending!");
        SceneManager.LoadScene(sadEndingSceneName);
    }
}