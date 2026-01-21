using UnityEngine;

public class JumpHeightBooster : StateMachineBehaviour
{
    [Header("Jump Height Settings")]
    [Tooltip("How much higher to boost the jump (in meters)")]
    public float jumpHeightMultiplier = 3f;
    
    [Header("Smooth Movement Settings")]
    [Tooltip("Lower = snappier (0.05), Higher = smoother (0.3)")]
    public float smoothTime = 0.15f;
    
    [Header("Jump Timing (Advanced)")]
    [Tooltip("When to finish rising phase (0-1)")]
    [Range(0.3f, 0.5f)]
    public float riseEndTime = 0.4f;
    
    [Tooltip("When to start falling phase (0-1)")]
    [Range(0.5f, 0.7f)]
    public float fallStartTime = 0.6f;
    
    // Private variables
    private Transform playerTransform;
    private float startY;
    private float targetY;
    private float currentVelocity = 0f;

    // Called when entering the jump animation state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get player's transform
        playerTransform = animator.transform;
        
        // Store starting Y position
        startY = playerTransform.position.y;
        
        // Calculate target peak height
        targetY = startY + jumpHeightMultiplier;
        
        // Reset velocity for smooth transitions
        currentVelocity = 0f;
        
        Debug.Log($"[JumpBooster] Jump started! Y: {startY} → Peak: {targetY}");
    }

    // Called every frame while in jump animation state
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerTransform == null) return;
        
        // Get normalized time (0 to 1) of animation
        float normalizedTime = stateInfo.normalizedTime % 1f;
        float targetHeight;
        
        // ===== THREE-PHASE JUMP ARC =====
        
        if (normalizedTime < riseEndTime)
        {
            // PHASE 1: RISING (0% to riseEndTime%)
            // Accelerate upward with ease-out curve
            float t = normalizedTime / riseEndTime;
            float easedT = 1f - Mathf.Pow(1f - t, 2f); // Quadratic ease-out
            targetHeight = Mathf.Lerp(startY, targetY, easedT);
        }
        else if (normalizedTime < fallStartTime)
        {
            // PHASE 2: HANG TIME AT PEAK (riseEndTime% to fallStartTime%)
            // Hold at maximum height for realistic float feeling
            targetHeight = targetY;
        }
        else
        {
            // PHASE 3: FALLING (fallStartTime% to 100%)
            // Descend with ease-in curve (gravity effect)
            float t = (normalizedTime - fallStartTime) / (1f - fallStartTime);
            float easedT = Mathf.Pow(t, 2f); // Quadratic ease-in
            targetHeight = Mathf.Lerp(targetY, startY, easedT);
        }
        
        // Apply smooth movement using SmoothDamp
        float smoothY = Mathf.SmoothDamp(
            playerTransform.position.y,
            targetHeight,
            ref currentVelocity,
            smoothTime
        );
        
        // Update player position
        playerTransform.position = new Vector3(
            playerTransform.position.x,
            smoothY,
            playerTransform.position.z
        );
    }

    // Called when exiting the jump animation state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerTransform == null) return;
        
        // Ensure player lands exactly at starting height
        playerTransform.position = new Vector3(
            playerTransform.position.x,
            startY,
            playerTransform.position.z
        );
        
        Debug.Log($"[JumpBooster] Jump complete! Landed at Y: {startY}");
        
        // Clean up
        playerTransform = null;
        currentVelocity = 0f;
    }
}
