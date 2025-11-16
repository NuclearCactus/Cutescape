using UnityEngine;

public class BrainrotWorldTrigger : MonoBehaviour
{
    [Header("Settings")]
    public bool triggerOnce = true; // Only trigger the first time player enters

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if we should only trigger once
            if (triggerOnce && hasTriggered)
                return;

            // Unlock Brainrot World
            if (CuteWorldManager.Instance != null)
            {
                CuteWorldManager.Instance.UnlockBrainrotWorld();
                hasTriggered = true;
                Debug.Log("Player entered Brainrot World trigger - Brainrot World unlocked!");
            }
            else
            {
                Debug.LogError("CuteWorldManager.Instance not found!");
            }
        }
    }
}