using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get Input
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        // give option to invert mouse y (up and down)
        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;

        // clamp the camera on the x - axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        // rotate the camera on the x - axis to look up and down
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        // rotate the player on the y - axis to look left and right
        transform.parent.Rotate(Vector3.up * mouseX);

    }
}
