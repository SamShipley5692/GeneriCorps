using UnityEngine;

public class pendulum : MonoBehaviour
{
    [SerializeField] float speed = 1.5f;
    [SerializeField] float limit = 75f;
    [SerializeField] bool randomStart = false;
    private float random = 0;

    void Awake()
    {
        if(randomStart)
        {
            random = Random.Range(0f, 1f);
        }
    }
    void Update()
    {
        float angle = limit * Mathf.Sin(Time.time + random * speed);
        transform.localRotation = Quaternion.Euler(0,0,angle);
    }
}
