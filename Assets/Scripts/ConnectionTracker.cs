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

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddRealConnection()
    {
        realWorldConnections++;
        UpdateUI();
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