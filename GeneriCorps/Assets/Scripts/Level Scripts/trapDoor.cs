using Unity.AI.Navigation;
using UnityEngine;

public class trapDoor : MonoBehaviour
{
    [SerializeField] GameObject trapDoorPrefab;
    [SerializeField] GameObject trapDoorPanel;
    Animator hingeAnim;

    private void Start()
    {
        hingeAnim = trapDoorPrefab.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        hingeAnim.SetTrigger("open");
        //trapDoorPanel.GetComponent<NavMeshSurface>().defaultArea = 0;
        //trapDoorPanel.GetComponent<NavMeshModifier>().enabled = false;
        //trapDoorPanel.GetComponent<NavMeshModifier>().area = 0;
        //trapDoorPanel.GetComponent<NavMeshModifier>().overrideArea = true;
    }
    private void OnTriggerExit(Collider other)
    {
        hingeAnim.SetTrigger("close");
        //trapDoorPanel.GetComponent<NavMeshSurface>().defaultArea = 1;

        //trapDoorPanel.GetComponent<NavMeshModifier>().enabled = true;
        //trapDoorPanel.GetComponent<NavMeshModifier>().area = 1;

    }
}
