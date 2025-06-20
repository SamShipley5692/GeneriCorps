using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] GameObject visualOnActivation;

    bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;

            if (visualOnActivation != null)
                visualOnActivation.SetActive(true);

            gameManager.instance.SetCheckpoint(transform.position, other.gameObject);
        }
    }
}