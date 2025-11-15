using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [TextArea(3, 5)] public string realWorldText;
    [TextArea(3, 5)] public string cuteWorldText;

    public float displayDuration = 2.5f;
    public float detectionRange = 2f;
    public Vector3 dialogueOffset = new Vector3(0, 1.5f, 0);
    
    [Header("Special NPC Abilities")]
    public bool unlocksWallJump = false;  // ← Add this

    private Transform player;
    private bool isPlayerNearby = false;

    // Track if interacted already
    private bool hasInteracted = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        isPlayerNearby = distance <= detectionRange;

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }
    }

    public void HandleInteraction()
    {
        bool cute = CuteWorldManager.Instance.isCuteMode;

        string textToShow = cute ? cuteWorldText : realWorldText;
        
        if (cute)
            SFXManager.Instance.PlayRandomSFX(SFXManager.Instance.cuteNPCSounds);
        else
            SFXManager.Instance.PlayRandomSFX(SFXManager.Instance.realNPCSounds);
        
        DialogueManager.Instance.ShowDialogue(
            textToShow,
            transform.position + dialogueOffset,
            displayDuration
        );

        // Count interaction only the FIRST time
        if (!hasInteracted)
        {
            hasInteracted = true;

            if (unlocksWallJump)
            {
                // Find the player instance in the scene
                PlayerController player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    player.wallJumpUnlocked = true;  // unlock wall jump
                }
            }

            if (cute)
                ConnectionTracker.Instance.AddCuteConnection();
            else
                ConnectionTracker.Instance.AddRealConnection();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}