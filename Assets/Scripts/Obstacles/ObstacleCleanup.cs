using UnityEngine;

public class ObstacleCleanup : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure Player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player == null) return;

        if (transform.position.z < player.position.z - 5f)
        {
            Destroy(gameObject);
        }
    }
}
