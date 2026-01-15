using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forward Movement")]
    public float forwardSpeed = 8f;

    [Header("Lane Movement")]
    public Transform[] lanes;
    public float laneSwitchSpeed = 10f;

    private int currentLaneIndex = 1;

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
            currentLaneIndex--;

        if (Input.GetKeyDown(KeyCode.D))
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
}
