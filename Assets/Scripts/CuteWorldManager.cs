using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class CuteWorldManager : MonoBehaviour
{
    public static CuteWorldManager Instance;

    [Header("References")]
    public PlayerController player;      
    public Slider batterySlider;         
    public GameObject batteryUI;         
    public TMP_Text batteryAmmoText;     // UI showing how many extra batteries
    public CuteWorldObject[] cuteObjects;

    [Header("Settings")]
    public float maxBattery = 5f;        // seconds per battery
    public float batteryDrainRate = 1f;  // rate per second
    public KeyCode toggleKey = KeyCode.F;

    [Header("Cute World Stats")]
    public float cuteMoveSpeed = 10f;
    public float cuteJumpForce = 20f;

    [Header("Runtime Variables")]
    public float currentBattery = 0f;    // runtime timer
    public int batteryAmmo = 0;          // stored extra batteries
    public bool isCuteMode = false;

    public EventReference cuteModeEnabledSound;
    public EventReference cuteModeDisabledSound;

    // Original stats
    private float originalMoveSpeed;
    private float originalJumpForce;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        originalMoveSpeed = player.moveSpeed;
        originalJumpForce = player.jumpForce;

        currentBattery = 0f;     // starts empty
        batteryAmmo = 0;         // no batteries at start

        batteryUI.SetActive(false);
        UpdateBatteryUI();
    }

    void Update()
    {
        // Toggle cute world manually
        if (Input.GetKeyDown(toggleKey))
        {
            if (isCuteMode)
            {
                DisableCuteMode();   // player exits early
            }
            else
            {
                TryEnableCuteMode(); // only works if they have battery
            }
        }

        // Drain battery when cute mode is active
        if (isCuteMode)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;
            batterySlider.value = currentBattery / maxBattery;

            if (currentBattery <= 0)
            {
                ConsumeBattery();
            }
        }
    }

    // ============================
    //   CUTE MODE ON/OFF
    // ============================

    void TryEnableCuteMode()
    {
        // Can only enter if:
        // - they have ammo OR
        // - currentBattery already has something (rare case)
        if (currentBattery > 0 || batteryAmmo > 0)
        {
            EnableCuteMode();
        }
        else
        {
            // No battery → cannot transform
            Debug.Log("No battery to enter Cute World.");
        }
    }

    void EnableCuteMode()
    {
        isCuteMode = true;

        // If current battery expired before, refill it from ammo
        if (currentBattery <= 0)
        {
            if (batteryAmmo > 0)
            {
                batteryAmmo--;
                currentBattery = maxBattery;
            }
        }

        // Activate UI
        batteryUI.SetActive(true);

        // Boost stats
        player.moveSpeed = cuteMoveSpeed;
        player.jumpForce = cuteJumpForce;

        // Activate cute objects
        foreach (var obj in cuteObjects)
            obj.SetCuteMode(true);

        UpdateBatteryUI();

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CuteModeEnabled", 1);
        FMODUnity.RuntimeManager.PlayOneShot(cuteModeEnabledSound);

    }

    public void DisableCuteMode()
    {
        isCuteMode = false;

        // Restore stats
        player.moveSpeed = originalMoveSpeed;
        player.jumpForce = originalJumpForce;

        batteryUI.SetActive(false);

        // Deactivate cute objects
        foreach (var obj in cuteObjects)
            obj.SetCuteMode(false);

        UpdateBatteryUI();
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CuteModeEnabled", 0);
        FMODUnity.RuntimeManager.PlayOneShot(cuteModeDisabledSound);
    }

    // ============================
    //   BATTERY MECHANICS
    // ============================

    public void RefillBattery()
    {
        // Picking up a battery ALWAYS adds ammo
        batteryAmmo++;

        UpdateBatteryUI();
    }

    void ConsumeBattery()
    {
        if (batteryAmmo > 0)
        {
            batteryAmmo--;
            currentBattery = maxBattery;
            UpdateBatteryUI();
        }
        else
        {
            // Out of battery completely → forced real world → player dies
            DisableCuteMode();
            player.Respawn();
        }
    }

    // ============================
    //   UI UPDATES
    // ============================

    public void UpdateBatteryUI()
    {
        if (batteryAmmoText != null)
            batteryAmmoText.text = "x" + batteryAmmo;
    }
}
