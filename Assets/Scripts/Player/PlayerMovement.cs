using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forward Movement")]
    public float startSpeed = 5f;        // Starting speed (slower, more playable)
    public float maxSpeed = 12f;         // Maximum speed cap
    public float speedIncreaseRate = 0.1f; // Speed increases by this every second
    private float currentSpeed;

    [Header("Lane Movement")]
    public Transform[] lanes;            // Array of lane transforms (Left, Center, Right)
    public float laneSwitchSpeed = 10f;  // Speed of lane switching
    private int currentLaneIndex = 1;    // Start at center lane

    [Header("Jump & Physics")]
    public float jumpForce = 8f;         // Initial upward velocity when jumping
    public float gravity = 20f;          // Gravity strength
    public float hangTimeGravity = 5f;   // Reduced gravity near peak for float feeling
    public float hangTimeThreshold = 1f; // If |velocityY| < this, use hang time gravity
    
    [Header("Ground Check")]
    public Transform groundCheck;        // Empty child at feet for ground detection
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;        // Assign Ground layer
    
    private CharacterController controller;
    private float velocityY = 0f;        // Vertical velocity
    private bool isGrounded;

    [Header("Speed Info (Read-Only)")]
    public float displaySpeed; // Shows current speed in Inspector

    void Start()
    {
        // Initialize speed
        currentSpeed = startSpeed;
        
        // Get CharacterController
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController component missing on " + gameObject.name);
        }
    }

    void Update()
    {
        // Check if grounded
        CheckGround();
        
        // Handle lane switching input
        HandleLaneInput();
        
        // Handle jump input
        HandleJump();
        
        // Apply gravity
        ApplyGravity();
        
        // Move player
        MovePlayer();
        
        // Increase speed over time
        IncreaseSpeed();
        
        // Update display speed
        displaySpeed = currentSpeed;
    }

    void CheckGround()
    {
        if (groundCheck != null)
        {
            // Check if overlapping with ground layer
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            // Fallback to CharacterController's built-in check
            isGrounded = controller.isGrounded;
        }
        
        // Reset vertical velocity when grounded
        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f; // Small negative value to keep grounded
        }
    }

    void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Move to left lane
            if (currentLaneIndex > 0)
            {
                currentLaneIndex--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Move to right lane
            if (currentLaneIndex < lanes.Length - 1)
            {
                currentLaneIndex++;
            }
        }
    }

    void HandleJump()
    {
        // Jump on Space press (single press, no holding)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocityY = jumpForce;
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            // Apply hang time effect near peak of jump
            if (Mathf.Abs(velocityY) < hangTimeThreshold)
            {
                velocityY -= hangTimeGravity * Time.deltaTime; // Reduced gravity
            }
            else
            {
                velocityY -= gravity * Time.deltaTime; // Normal gravity
            }
        }
    }

    void MovePlayer()
    {
        // Forward movement (Z-axis)
        Vector3 forwardMove = transform.forward * currentSpeed;
        
        // Lateral movement (X-axis) - smooth lerp to target lane
        float targetX = lanes[currentLaneIndex].position.x;
        float currentX = transform.position.x;
        float newX = Mathf.Lerp(currentX, targetX, laneSwitchSpeed * Time.deltaTime);
        Vector3 lateralMove = new Vector3(newX - currentX, 0, 0);
        
        // Vertical movement (Y-axis) - physics jump
        Vector3 verticalMove = new Vector3(0, velocityY, 0);
        
        // Combine all movement
        Vector3 totalMove = (forwardMove + lateralMove + verticalMove) * Time.deltaTime;
        
        // Move using CharacterController
        controller.Move(totalMove);
    }

    void IncreaseSpeed()
    {
        // Gradually increase speed
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += speedIncreaseRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
    }
    
    // Visualize ground check in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
