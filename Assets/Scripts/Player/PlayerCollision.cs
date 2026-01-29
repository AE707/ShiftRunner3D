using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
{
    Debug.Log("TRIGGER ENTERED! Hit: " + other.gameObject.name);
    
    if (other.CompareTag("Obstacle"))
    {
        Debug.Log("Game Over!");
        
                // Trigger camera shake
                if (CameraShake.Instance != null)
                    CameraShake.Instance.Shake(0.2f, 0.3f);
                
                // Play collision sound
                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlayCollision();
                    GameManager.Instance.GameOver();
            Time.timeScale = 0f;
    }
    else
    {
        Debug.Log("Hit something but tag is: " + other.tag);
    }
}

void OnCollisionEnter(Collision collision)
{
    Debug.Log("COLLISION ENTERED! Hit: " + collision.gameObject.name);
}

   /* private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle – Game Over");
            GameManager.Instance.GameOver();
            Time.timeScale = 0f;
        }
    }*/

}
