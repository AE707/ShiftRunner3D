using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle – Game Over");
            GameManager.Instance.GameOver();
            Time.timeScale = 0f;
        }
    }

}
