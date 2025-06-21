using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] float riseSpeed = 1f;
    [SerializeField] float lifetime = 3f;

    private TextMeshPro text;
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        text = GetComponent<TextMeshPro>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        transform.LookAt(cam);
        transform.Rotate(0, 180f, 0); 
    }

    public void SetDamage(int damage)
    {
        if (text != null)
            text.text = damage.ToString();
    }
}