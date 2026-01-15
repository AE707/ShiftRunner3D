using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forward Movement")]
    public float forwardSpeed = 8f;

    [Header("Lane Movement")]
    public float laneDistance = 2.5f;
    public float laneSwitchSpeed = 10f;

    private int currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        MoveForward();
        HandleLaneInput();
        MoveToLane();
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.D))
            ChangeLane(1);
    }

    void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);

        targetPosition = new Vector3(
            (currentLane - 1) * laneDistance,
            transform.position.y,
            transform.position.z
        );
    }

    void MoveToLane()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            laneSwitchSpeed * Time.deltaTime
        );
    }
}
