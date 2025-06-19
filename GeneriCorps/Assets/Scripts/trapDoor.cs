using UnityEngine;

public class trapDoor : MonoBehaviour
{
    [SerializeField] GameObject trapDoorPrefab;
    Animator hingeAnim;

    private void Start()
    {
        hingeAnim = trapDoorPrefab.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //trapDoorPrefab.GetComponent<Animation>().Play("TrapDoorAnim");
        hingeAnim.SetTrigger("open");
    }
    private void OnTriggerExit(Collider other)
    {
        hingeAnim.SetTrigger("close");
    }
}
