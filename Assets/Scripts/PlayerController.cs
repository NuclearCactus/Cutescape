using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 15f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Wall Jump")]
    public float wallCheckDistance = 0.6f;
    public float wallJumpForce = 15f;
    public LayerMask wallLayer; 
    public bool wallJumpUnlocked = false;

    private bool isTouchingWall = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    
    public float interactionRange = 2f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movement input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // WALL CHECK (left and right)
        RaycastHit2D wallHitLeft  = Physics2D.Raycast(transform.position, Vector2.left,  wallCheckDistance, wallLayer);
        RaycastHit2D wallHitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        isTouchingWall = (wallHitLeft.collider != null || wallHitRight.collider != null);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // normal jump
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            // WALL JUMP
            else if (wallJumpUnlocked && isTouchingWall)
            {
                float wallDir = 0f;

                if (wallHitLeft.collider != null) wallDir = -1f;  
                if (wallHitRight.collider != null) wallDir = 1f;

                // push away from wall
                rb.linearVelocity = new Vector2(-wallDir * moveSpeed, wallJumpForce);
            }
        }

        // Interaction
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRange);
            foreach (var hit in hits)
            {
                NPCDialogue npc = hit.GetComponent<NPCDialogue>();
                if (npc != null)
                {
                    npc.HandleInteraction();
                    break;
                }
            }
        }
    }

    void FixedUpdate()
    {
        // Horizontal movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        // Interaction radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        // Wall checks
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.left * wallCheckDistance);
        Gizmos.DrawRay(transform.position, Vector2.right * wallCheckDistance);
    }
    
    public void Respawn()
    {
        if (Checkpoint.current != null)
        {
            transform.position = Checkpoint.current.transform.position;

            foreach (var pickup in BatteryPickup.allPickups)
                pickup.gameObject.SetActive(true);

            CuteWorldManager.Instance.currentBattery = 0f;
            CuteWorldManager.Instance.batterySlider.value = 0f;
        }
    }
}