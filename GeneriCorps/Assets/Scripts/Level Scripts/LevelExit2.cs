using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit2 : MonoBehaviour
{ 
[SerializeField] GameObject doorModel;
[SerializeField] GameObject button;
[SerializeField] string text;

bool playerInTrigger;
bool hasBeenOpened = false;

    void Update()
{
    if (playerInTrigger && !hasBeenOpened)
    {
        if (Input.GetButtonDown("Interact"))
        {
            doorModel.SetActive(false);
            button.SetActive(false); // added this line
            SceneManager.LoadScene("Level 4 - Boss");
        }
    }
}

private void OnTriggerEnter(Collider other)
{
        if (hasBeenOpened)
        {
            return;
        }

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
        if (hasBeenOpened)
        {
            return;
        }

        IOpen openable = other.GetComponent<IOpen>();

    if (openable != null)
    {
        button.SetActive(false);
        playerInTrigger = false;
        doorModel.SetActive(true);
        gameManager.instance.textPopUp.SetActive(false);
    }
}
}
