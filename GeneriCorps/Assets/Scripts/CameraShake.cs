using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private float shakeDuration = 0f;


    private float shakeIntensity = 0.1f;

    private float timeElapsed = 0f;

    private bool isShaking = false;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }
    void Update()
    {
        if (isShaking)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed < shakeDuration)
            {
                Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
                transform.localPosition = originalPosition + randomOffset;

                
                transform.localRotation = originalRotation * Quaternion.Euler(
                    Random.Range(-shakeIntensity, shakeIntensity),
                    Random.Range(-shakeIntensity, shakeIntensity),
                    Random.Range(-shakeIntensity, shakeIntensity)
                );
            }
            else
            {
                StopShake();
            }
        }
    }

    public void TriggerShake(float duration, float intensity)
    {
        if (!isShaking)
        {
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
        }

        shakeDuration = duration;
        shakeIntensity = intensity;
        timeElapsed = 0f;
        isShaking = true;
    }

    private void StopShake()
    {
        isShaking = false;
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
}
