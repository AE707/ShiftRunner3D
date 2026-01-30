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
    private int currentLaneIndex = 0;    // Start at center lane

    [Header("Jump & Physics")]
    public float jumpForce = 10f;         // Initial upward velocity when jumping
    public float gravity = 20f;          // Gravity strength
    public float hangTimeGravity = 5f;   // Reduced gravity near peak for float feeling
    public float hangTimeThreshold = 1f; // If |velocityY| < this, use hang time gravity
    
    [Header("Ground Check")]
    public Transform groundCheck;        // Empty child at feet for ground detection
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;        // Assign Ground layer
    
    private CharacterController controller;
    private float velocityY = -0.5f;        // Vertical velocity
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
    
    // NEW: Auto-detect starting lane based on Adam's X position
    if (lanes != null && lanes.Length > 0)
    {
        float currentX = transform.position.x;
        float minDistance = float.MaxValue;
        
        // Find which lane Adam is closest to
        for (int i = 0; i < lanes.Length; i++)
        {
            float distance = Mathf.Abs(lanes[i].position.x - currentX);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentLaneIndex = i;
            }
        }
        
        Debug.Log($"Starting in lane {currentLaneIndex} at position X={currentX}");
    }

    if (transform.position.y < 0.5f) {
    transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
}

}


    void Update()
    {

            Debug.Log($"[UPDATE] Frame={Time.frameCount}, isGrounded={isGrounded}, velocityY={velocityY}, position={transform.position}");
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
            velocityY = -0.5f; // Small negative value to keep grounded
        }
    }

    void HandleLaneInput()
{
    Debug.Log($"[LANE INPUT] currentLaneIndex={currentLaneIndex}, lanes.Length={lanes.Length}");
    
    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
    {
        Debug.Log("★★★ A KEY DETECTED! ★★★");
        // Move to left lane
        if (currentLaneIndex > 0)
        {
            currentLaneIndex--;
            Debug.Log($"→ Switched to lane {currentLaneIndex}");
        }
        else
        {
                    // Play lane switch sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayLaneSwitch();
            Debug.Log("→ Already at leftmost lane!");
        }
    }
    else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
    {
        Debug.Log("★★★ D KEY DETECTED! ★★★");
        // Move to right lane
        if (currentLaneIndex < lanes.Length - 1)
        {
            currentLaneIndex++;
            Debug.Log($"→ Switched to lane {currentLaneIndex}");
        }
        else
        {
                    // Play lane switch sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayLaneSwitch();
            Debug.Log("→ Already at rightmost lane!");
        }
    }
}


void HandleJump()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        Debug.Log($"★★★ SPACE KEY DETECTED! isGrounded={isGrounded} ★★★");
    }
    
    // Jump on Space press (single press, no holding)
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
    {
        velocityY = jumpForce;
                // Play jump sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayJump();
        Debug.Log($"→ JUMP TRIGGERED! velocityY set to {jumpForce}");
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
    
    // Vertical movement (Y-axis) - physics jump
    Vector3 verticalMove = new Vector3(0, velocityY, 0);
    
    // Combine movement: forward + vertical scaled by deltaTime, lateral is already interpolated
    Vector3 move = (forwardMove + verticalMove) * Time.deltaTime;
    move.x = newX - currentX;  // Override X with interpolated lane position
    
    Debug.Log($"[MOVE PLAYER] targetX={targetX:F2}, currentX={currentX:F2}, newX={newX:F2}, move={move}");
    
    // Move using CharacterController
    controller.Move(move);
    
    Debug.Log($"[AFTER MOVE] New position={transform.position}");
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

        // Public getters for GameManager
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public float GetNormalizedSpeed()
    {
        // Returns 0 at startSpeed, 1 at maxSpeed
        if (maxSpeed <= startSpeed) return 0f;
        return Mathf.Clamp01((currentSpeed - startSpeed) / (maxSpeed - startSpeed));
    }
}
