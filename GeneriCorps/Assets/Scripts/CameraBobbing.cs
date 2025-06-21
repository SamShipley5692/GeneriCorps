using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    [SerializeField] float walkBobSpeed = 5f;
    [SerializeField] float runBobSpeed = 9f;
    [SerializeField] float bobAmount = 0.05f;
    [SerializeField] Transform player;

    private float defaultY;
    private float timer;

    void Start()
    {
        defaultY = transform.localPosition.y;
    }

    void Update()
    {
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc == null || !cc.isGrounded || cc.velocity.magnitude < 0.1f)
        {
            timer = 0;
            Vector3 pos = transform.localPosition;
            pos.y = Mathf.Lerp(pos.y, defaultY, Time.deltaTime * 5f);
            transform.localPosition = pos;
            return;
        }

        float speed = Input.GetButton("Sprint") ? runBobSpeed : walkBobSpeed;
        timer += Time.deltaTime * speed;
        float offsetY = Mathf.Sin(timer) * bobAmount;

        Vector3 newPos = transform.localPosition;
        newPos.y = defaultY + offsetY;
        transform.localPosition = newPos;
    }
}