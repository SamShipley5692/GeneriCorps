using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject[] possibleDrops;
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;
    [SerializeField] GameObject doorModel;
    [SerializeField] GameObject Button;

    bool playerInTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetButtonDown("Interact"))
            {
                doorModel.SetActive(false);
                Button.SetActive(false);
                DropRandomItem();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        IOpen openable = other.GetComponent<IOpen>();

        if (openable != null)
        {
            Button.SetActive(true);
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IOpen openable = other.GetComponent<IOpen>();

        if (openable != null)
        {
            Button.SetActive(false);
            playerInTrigger = false;
            doorModel.SetActive(true);
            gameManager.instance.textPopUp.SetActive(false);

        }
    }

    private void DropRandomItem()
    {
        if (possibleDrops == null || possibleDrops.Length == 0) return;

        int idx = Random.Range(0, possibleDrops.Length);
        GameObject drop = possibleDrops[idx];
        if (drop != null)
            Instantiate(drop, transform.position + spawnOffset, Quaternion.identity);

    }
}
