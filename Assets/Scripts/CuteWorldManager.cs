using UnityEngine;
using UnityEngine.UI;

public class CuteWorldManager : MonoBehaviour
{
    public static CuteWorldManager Instance;
    
    [Header("References")]
    public PlayerController player;      // drag your player controller
    public Slider batterySlider;         // drag the UI slider
    public GameObject batteryUI;         // the slider’s parent

    [Header("Settings")]
    public float maxBattery = 5f;        // seconds
    public float batteryDrainRate = 1f;  // drains per second
    public KeyCode toggleKey = KeyCode.F;

    [Header("Player Cute Stats")]
    public float cuteMoveSpeed = 10f;
    public float cuteJumpForce = 20f;

    public float currentBattery;
    public bool isCuteMode = false;

    // Cache original stats
    private float originalMoveSpeed;
    private float originalJumpForce;

    [SerializeField]private CuteWorldObject[] cuteObjects;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // optional: ensure only one manager exists
            return;
        }

        Instance = this;
    }
    
    void Start()
    {
        originalMoveSpeed = player.moveSpeed;
        originalJumpForce = player.jumpForce;

        // cuteObjects = FindObjectsByType<CuteWorldObject>(FindObjectsSortMode.None);

        batteryUI.SetActive(false); // hidden initially
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            ToggleCuteMode();

        if (isCuteMode)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;
            batterySlider.value = currentBattery / maxBattery;

            if (currentBattery <= 0)
            {
                DisableCuteMode();
                // Call respawn on player
                PlayerController player = FindObjectOfType<PlayerController>();
                if (player != null)
                    player.Respawn();
            }
        }
    }

    void ToggleCuteMode()
    {
        if (isCuteMode)
            DisableCuteMode();
        else
            EnableCuteMode();
    }

    void EnableCuteMode()
    {
        isCuteMode = true;
        currentBattery = maxBattery;

        player.moveSpeed = cuteMoveSpeed;
        player.jumpForce = cuteJumpForce;

        batteryUI.SetActive(true);

        foreach (var obj in cuteObjects)
            obj.SetCuteMode(true);

        // TODO hook visual/audio changes:
        // - Change skybox / post processing / sprite material
        // - Change music
    }

    void DisableCuteMode()
    {
        isCuteMode = false;

        player.moveSpeed = originalMoveSpeed;
        player.jumpForce = originalJumpForce;

        batteryUI.SetActive(false);

        foreach (var obj in cuteObjects)
            obj.SetCuteMode(false);

        // TODO: revert visuals/audio
    }
    
    public void RefillBattery()
    {
        currentBattery = maxBattery;
        batterySlider.value = maxBattery;
    }
}
