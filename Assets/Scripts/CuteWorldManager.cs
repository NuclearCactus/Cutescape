using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using Unity.Cinemachine;
using System.Collections;

public class CuteWorldManager : MonoBehaviour
{
    public static CuteWorldManager Instance;

    [Header("References")]
    public PlayerController player;      
    public Slider batterySlider;         
    public GameObject batteryUI;         
    public TMP_Text batteryAmmoText;     // UI showing how many extra batteries
    public CuteWorldObject[] cuteObjects;

    [Header("Background Arrays")]
    public GameObject[] realWorldBackgrounds;   // Real world backgrounds
    public GameObject[] cuteWorldBackgrounds;   // Cute world backgrounds
    public GameObject[] brainrotWorldBackgrounds; // Brainrot world backgrounds

    [Header("Platform Parents")]
    public GameObject realWorldPlatforms;      // Parent object with real world platform visuals
    public GameObject visibleCuteWorldPlatforms; // Parent object with cute world platform visuals

    [Header("Tilemaps")]
    public UnityEngine.Tilemaps.Tilemap realWorldTilemap;        // Real world tilemap
    public UnityEngine.Tilemaps.Tilemap cuteWorldTilemap;        // Cute world tilemap

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
    public bool isBrainrotUnlocked = false; // Set to true when player enters trigger

    [Header("Cinemachine Camera")]
    public CinemachineCamera cinemachineCamera;

    public float realWorldZoom = 5f;   // Original camera size
    public float cuteWorldZoom = 8f; // Zoomed-out value
    public float zoomDuration = 0.6f;  // Smooth transition time

    private Coroutine zoomCoroutine;

    public EventReference cuteModeEnabledSound;
    public EventReference cuteModeDisabledSound;

    // Original stats
    private float originalMoveSpeed;
    private float originalJumpForce;

    // This is used to disable the CuteModeDisabled sound when the game is first launched.
    // Then it is set to false immediately and should never be touched again (in the respective scene)
    private bool firstLaunch = true;

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

        // Initialize backgrounds - only Real World active at start
        SetActiveBackgrounds(realWorldBackgrounds, true);
        SetActiveBackgrounds(cuteWorldBackgrounds, false);
        SetActiveBackgrounds(brainrotWorldBackgrounds, false);

        // Initialize tilemaps - only Real World active at start
        realWorldTilemap.gameObject.SetActive(true);
        cuteWorldTilemap.gameObject.SetActive(false);

        // Initialize platforms - only Real World visible at start
        if (realWorldPlatforms != null)
            realWorldPlatforms.SetActive(true);
        if (visibleCuteWorldPlatforms != null)
            visibleCuteWorldPlatforms.SetActive(false);

        if (cinemachineCamera != null)
        {
            realWorldZoom = cinemachineCamera.Lens.OrthographicSize;
        }
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

        // Switch backgrounds based on brainrot unlock status
        SwitchToAlternateWorld();

        UpdateBatteryUI();

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CuteModeEnabled", 1);
        FMODUnity.RuntimeManager.PlayOneShot(cuteModeEnabledSound);
        
        SFXManager.Instance.PlaySFX(SFXManager.Instance.phoneOnSound);

        // Set camera zoom
        SetCameraZoom(cuteWorldZoom);
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

        // Switch back to Real World backgrounds
        SwitchToRealWorld();

        UpdateBatteryUI();

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("CuteModeEnabled", 0);
        if (!firstLaunch)
        {
            FMODUnity.RuntimeManager.PlayOneShot(cuteModeDisabledSound);
        }
        firstLaunch = false;
        SFXManager.Instance.PlaySFX(SFXManager.Instance.phoneOffSound);

        // Disable camera zoon
        SetCameraZoom(realWorldZoom);
    }

    // ============================
    //   BACKGROUND SWITCHING
    // ============================

    void SwitchToAlternateWorld()
    {
        // Disable Real World
        SetActiveBackgrounds(realWorldBackgrounds, false);

        // Enable either Cute World or Brainrot World
        if (isBrainrotUnlocked)
        {
            SetActiveBackgrounds(cuteWorldBackgrounds, false);
            SetActiveBackgrounds(brainrotWorldBackgrounds, true);
            Debug.Log("Switched to Brainrot World backgrounds");
        }
        else
        {
            SetActiveBackgrounds(cuteWorldBackgrounds, true);
            SetActiveBackgrounds(brainrotWorldBackgrounds, false);
            Debug.Log("Switched to Cute World backgrounds");
        }

        // Switch platform visuals to Cute World
        if (realWorldPlatforms != null)
            realWorldPlatforms.SetActive(false);
        if (visibleCuteWorldPlatforms != null)
            visibleCuteWorldPlatforms.SetActive(true);

        // Switch tilemaps
        realWorldTilemap.gameObject.SetActive(false);
        cuteWorldTilemap.gameObject.SetActive(true);
    }   

    void SwitchToRealWorld()
    {
        // Enable Real World, disable everything else
        SetActiveBackgrounds(realWorldBackgrounds, true);
        SetActiveBackgrounds(cuteWorldBackgrounds, false);
        SetActiveBackgrounds(brainrotWorldBackgrounds, false);
        Debug.Log("Switched to Real World backgrounds");

        // Switch platform visuals back to Real World
        if (realWorldPlatforms != null)
            realWorldPlatforms.SetActive(true);
        if (visibleCuteWorldPlatforms != null)
            visibleCuteWorldPlatforms.SetActive(false);

        // Switch tilemaps
        realWorldTilemap.gameObject.SetActive(true);
        cuteWorldTilemap.gameObject.SetActive(false);
    }

    void SetActiveBackgrounds(GameObject[] backgrounds, bool active)
    {
        if (backgrounds == null) return;

        foreach (GameObject bg in backgrounds)
        {
            if (bg != null)
            {
                bg.SetActive(active);
            }
        }
    }

    // ============================
    //   PUBLIC METHOD FOR TRIGGER
    // ============================

    public void UnlockBrainrotWorld()
    {
        isBrainrotUnlocked = true;
        Debug.Log("Brainrot World unlocked!");
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
            SFXManager.Instance.PlaySFX(SFXManager.Instance.batteryEmptySound);
            currentBattery = maxBattery;
            UpdateBatteryUI();
        }
        else
        {
            // Out of battery completely → forced real world → player dies
            DisableCuteMode();
            SFXManager.Instance.PlaySFX(SFXManager.Instance.batteryFullDeathSound);
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

    // Camera Zooming

    IEnumerator SmoothZoom(float targetZoom)
    {
        float startZoom = cinemachineCamera.Lens.OrthographicSize;
        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            // float t = elapsed / zoomDuration;
            float t = Mathf.SmoothStep(0, 1, elapsed / zoomDuration);


            cinemachineCamera.Lens.OrthographicSize =
                Mathf.Lerp(startZoom, targetZoom, t);

            yield return null;
        }

        cinemachineCamera.Lens.OrthographicSize = targetZoom;
    }

    void SetCameraZoom(float targetZoom)
    {
        if (cinemachineCamera == null) return;

        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(SmoothZoom(targetZoom));
    }

}