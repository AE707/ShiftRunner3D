using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPosition;
    private bool isShaking = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    /// <summary>
    /// Trigger camera shake effect
    /// </summary>
    /// <param name="duration">How long the shake lasts (seconds)</param>
    /// <param name="magnitude">How strong the shake is</param>
    public void Shake(float duration = 0.15f, float magnitude = 0.2f)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine(duration, magnitude));
        }
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0f);

            elapsed += Time.unscaledDeltaTime; // Use unscaled time for game over shake
            yield return null;
        }

        transform.localPosition = originalPosition;
        isShaking = false;
    }
}
