using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] Image hpFill;
    [SerializeField] Transform targetToFollow;
    [SerializeField] Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (targetToFollow == null || cam == null) return;

        transform.position = targetToFollow.position + offset;
        transform.LookAt(cam.transform);
        transform.Rotate(0, 180f, 0);
    }

    public void SetHealth(float percent)
    {
        hpFill.fillAmount = percent;
    }
}