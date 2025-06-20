using System.Collections;
using UnityEngine;

public class bossDoor : MonoBehaviour
{
    [SerializeField] GameObject doorModel;
    [SerializeField] GameObject button;
    [SerializeField] string text;
    [SerializeField] GameObject hinge;

    Animator hingeAnim;

    bool playerInTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hingeAnim = hinge.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetButtonDown("Interact"))
            {
                hingeAnim.SetTrigger("open");
                button.SetActive(false); // added this line
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IOpen openable = other.GetComponent<IOpen>();

        if (openable != null)
        {
            button.SetActive(true);
            playerInTrigger = true;
            gameManager.instance.textPopUpDescription.text = text;
            gameManager.instance.textPopUp.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IOpen openable = other.GetComponent<IOpen>();

        if (openable != null)
        {
            button.SetActive(false);
            playerInTrigger = false;
            StartCoroutine(closeDoor());
            gameManager.instance.textPopUp.SetActive(false);
        }
    }

    IEnumerator closeDoor()
    {
        yield return new WaitForSeconds(1f); // Wait for the door to close
        hingeAnim.SetTrigger("close");
    }
}
