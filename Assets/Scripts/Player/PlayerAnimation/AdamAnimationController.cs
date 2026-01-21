using UnityEngine;

public class AdamAnimationController : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;
    
    [Header("Ground Check (Optional)")]
    public Transform groundCheck;        // Optional: Empty child at feet for precise ground detection
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;        // Assign Ground layer
    
    private bool isGrounded;
    private bool wasGrounded;            // Track previous frame ground state

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        
        if (animator == null)
        {
            Debug.LogError("Animator component missing on " + gameObject.name);
        }
        
        if (controller == null)
        {
            Debug.LogError("CharacterController component missing on " + gameObject.name);
        }
        
        // Speed up the animation to make walk look like run
        /* if (animator != null)
        {
            animator.speed = 1f; // Try values: 1.5f, 2f, 2.5f, 3f
        }*/
    }

    void Update()
    {
        // Check if grounded
        CheckGround();
        
        // Update jump animation based on physics state
        UpdateJumpAnimation();
        
        // Store current ground state for next frame
        wasGrounded = isGrounded;
    }
    
    void CheckGround()
    {
        if (groundCheck != null)
        {
            // Use sphere check for precise ground detection
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            // Fallback to CharacterController's built-in check
            isGrounded = controller.isGrounded;
        }
    }
    
    void UpdateJumpAnimation()
    {
        // Set Jumpi bool based on physics state:
        // - If in the air AND moving upward → Jumpi = true (rising phase)
        // - If grounded → Jumpi = false (landed)
        
        if (!isGrounded)
        {
            // In the air - check if rising
            // controller.velocity.y > 0.1f means moving upward
            if (controller.velocity.y > 0.1f)
            {
                animator.SetBool("Jumpi", true);
            }
            // Optional: You can also handle falling separately if needed
            // else if (controller.velocity.y < -0.1f)
            // {
            //     // Falling - you could add a separate "Fall" animation state
            // }
        }
        else
        {
            // Grounded - clear jump animation
            animator.SetBool("Jumpi", false);
        }
    }
    
    // Visualize ground check in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
