using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] GameObject doorModel;
    [SerializeField] GameObject button;
    [SerializeField] string text;

    bool playerInTrigger = false;
    bool hasBeenOpened = false;

    void Update()
    {
        if (playerInTrigger && !hasBeenOpened && gameManager.instance.getGameGoalCount() <= 0)
        {
            if (Input.GetButtonDown("Interact"))
            {
                hasBeenOpened = true;
                doorModel.SetActive(false);
                button.SetActive(false); // added this line
                SceneManager.LoadScene("Level 3");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(hasBeenOpened)
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