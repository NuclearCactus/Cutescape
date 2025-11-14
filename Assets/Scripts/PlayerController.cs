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
        // Get input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRange);
            foreach (var hit in hits)
            {
                NPC npc = hit.GetComponent<NPC>();
                if (npc != null)
                {
                    npc.ShowDialogue();
                    break;
                }
            }
        }
    }

    void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
    
    public void Respawn()
    {
        if (Checkpoint.current != null)
        {
            transform.position = Checkpoint.current.transform.position;

            // Restore battery pickups
            foreach (var pickup in BatteryPickup.allPickups)
                pickup.gameObject.SetActive(true);

            // Restore battery
            CuteWorldManager.Instance.currentBattery = CuteWorldManager.Instance.maxBattery;
            CuteWorldManager.Instance.batterySlider.value = 1f;
        }
    }
}
