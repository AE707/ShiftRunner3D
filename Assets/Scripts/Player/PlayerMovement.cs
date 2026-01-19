using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forward Movement")]
    public float startSpeed = 5f;          // Starting speed (slower, more playable)
    public float maxSpeed = 12f;           // Maximum speed cap
    public float speedIncreaseRate = 0.1f; // Speed increases by this every second
    private float currentSpeed;
    
    [Header("Lane Movement")]
    public Transform[] lanes;
    public float laneSwitchSpeed = 10f;
    private int currentLaneIndex = 1;
    
    [Header("Speed Info (Read-Only)")]
    public float displaySpeed; // Shows current speed in inspector
    
    void Start()
    {
        // Initialize speed
        currentSpeed = startSpeed;
    }
    
    void Update()
    {
        MoveForward();
        HandleLaneInput();
        MoveToLane();
        IncreaseSpeed();
        
        // Update display for inspector
        displaySpeed = currentSpeed;
    }
    
    void MoveForward()
    {
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }
    
    void IncreaseSpeed()
    {
        // Gradually increase speed over time, but cap at maxSpeed
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += speedIncreaseRate * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
    }
    
    void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            currentLaneIndex--;
        
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            currentLaneIndex++;
        
        currentLaneIndex = Mathf.Clamp(currentLaneIndex, 0, lanes.Length - 1);
    }
    
    void MoveToLane()
    {
        Vector3 targetPos = new Vector3(
            lanes[currentLaneIndex].position.x,
            transform.position.y,
            transform.position.z
        );
        
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * laneSwitchSpeed
        );
    }
    
    // Public method to reset speed (useful for game restart)
    public void ResetSpeed()
    {
        currentSpeed = startSpeed;
    }
    
    // Public method to get current speed (useful for UI display)
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
