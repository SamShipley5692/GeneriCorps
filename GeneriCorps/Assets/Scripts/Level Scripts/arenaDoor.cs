using System.Collections;
using UnityEngine;

public class arenaDoor : MonoBehaviour
{
    [SerializeField] GameObject doorModel;
    [SerializeField] GameObject button;
    [SerializeField] string text;
    [SerializeField] float rotationAmount = 90f;
    [SerializeField] float openSpeed = 2f;
    
    Vector3 player;
    Vector3 startRot;
    Vector3 forward;

    bool isOpen = false;

    float forwardDirection;

    Coroutine animCoroutine;

    void Awake()
    {
        startRot = transform.rotation.eulerAngles;
        forward = transform.right;
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {

            open(player);
            button.SetActive(false);
        }
    }

    public void open(Vector3 userPos)
    {
        if (!isOpen)
        {
            if (animCoroutine != null)
            {
                StopCoroutine(animCoroutine);
            }
            float dot = Vector3.Dot(forward, (userPos - transform.position).normalized);
            animCoroutine = StartCoroutine(doRotationOpen(dot));
        }
    }

    IEnumerator doRotationOpen(float forwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (forwardAmount >= forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRot.y - rotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRot.y + rotationAmount, 0));
        }

        isOpen = true;

        float time = 0f;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * openSpeed;
        }
    }

    public void close()
    {
        if (isOpen)
        {
            if (animCoroutine != null)
            {
                StopCoroutine(animCoroutine);
            }
            animCoroutine = StartCoroutine(doRotationClose());
        }
    }

    IEnumerator doRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(startRot);
        isOpen = false;
        float time = 0f;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * openSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            button.SetActive(true);
            gameManager.instance.textPopUpDescription.text = text;
            gameManager.instance.textPopUp.SetActive(true);
            player = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            button.SetActive(false);
            gameManager.instance.textPopUp.SetActive(false);

            close();
        }
    }

}

