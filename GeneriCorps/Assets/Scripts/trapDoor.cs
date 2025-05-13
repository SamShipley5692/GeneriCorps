using UnityEngine;

public class trapDoor : MonoBehaviour
{
    [SerializeField] GameObject trapDoorPrefab;
    private void OnTriggerEnter(Collider other)
    {
        trapDoorPrefab.GetComponent<Animation>().Play("TrapDoorAnim");
    }
}
