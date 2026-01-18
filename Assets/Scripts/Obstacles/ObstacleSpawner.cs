using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;

    public float spawnZ = 40f;
    public float spawnInterval = 0.5f;
    public float laneDistance = 2.5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        int lane = Random.Range(-1, 2); // -1, 0, 1
        Vector3 spawnPos = new Vector3(lane * laneDistance, 0.5f, spawnZ);

        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }
}
