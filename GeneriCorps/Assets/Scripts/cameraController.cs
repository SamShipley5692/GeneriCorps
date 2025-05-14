using UnityEngine;


// still need to attach camera to player to test functionality 
public class cameraController : MonoBehaviour
{
    // settings for camera 
    [SerializeField] int sensity;
    [SerializeField] int lockVertMinimum, lockVertMaximum;

    float rotateXAxis;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // receive mouse movement 
        float mouseX = Input.GetAxis("Mouse X") * sensity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensity * Time.deltaTime;

        // limit camera from breaking character's neck 
        rotateXAxis = Mathf.Clamp(rotateXAxis, lockVertMinimum, lockVertMaximum);

        // look up look down
        transform.localRotation = Quaternion.Euler(rotateXAxis, 0, 0);

        // rotate character with camera 

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
