using System.Collections;
using UnityEngine;

public class arenaDoor : MonoBehaviour
{
    [SerializeField] GameObject doorModel;
    [SerializeField] GameObject button;
    [SerializeField] string text;
    [SerializeField] GameObject hinge;

    Animator hingeAnim;

    bool playerInTrigger;

    void Start()
    {
        hingeAnim = hinge.GetComponent<Animator>();
    }

    void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetButtonDown("Interact"))
            {
                hingeAnim.SetTrigger("open");
                button.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IOpen openable = other.GetComponent<IOpen>();


        if (other.CompareTag("Player") && openable != null)
        {
            button.SetActive(true);
            gameManager.instance.textPopUpDescription.text = text;
            gameManager.instance.textPopUp.SetActive(true);

            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IOpen openable = other.GetComponent<IOpen>();

        if (openable != null)
        {
            button.SetActive(false);
            playerInTrigger = false;
            hingeAnim.SetTrigger("close");

            //StartCoroutine(closeDoor());
            gameManager.instance.textPopUp.SetActive(false);
        }
    }

    IEnumerator closeDoor()
    {
        yield return new WaitForSeconds(1f); // Wait for the door to close
        hingeAnim.SetTrigger("close");
    }
}
