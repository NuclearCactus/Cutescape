using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionTracker : MonoBehaviour
{
    public static ConnectionTracker Instance;

    public int realWorldConnections = 0;
    public int cuteWorldConnections = 0;

    [Header("UI References")]
    public TextMeshProUGUI realWorldTextUI;
    public TextMeshProUGUI cuteWorldTextUI;
    
    [Header("Bridge")]
    public GameObject bridge; // assign the bridge object in inspector
    public int realConnectionsRequiredForBridge = 10;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        // Ensure bridge is inactive initially
        if (bridge != null)
            bridge.SetActive(false);
    }

    public void AddRealConnection()
    {
        realWorldConnections++;
        UpdateUI();
        
        // Check if we can activate the bridge
        if (bridge != null && realWorldConnections >= realConnectionsRequiredForBridge)
        {
            bridge.SetActive(true);
        }
    }

    public void AddCuteConnection()
    {
        cuteWorldConnections++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (realWorldTextUI != null)
            realWorldTextUI.text = "Real: " + realWorldConnections;

        if (cuteWorldTextUI != null)
            cuteWorldTextUI.text = "Cute: " + cuteWorldConnections;
    }
}