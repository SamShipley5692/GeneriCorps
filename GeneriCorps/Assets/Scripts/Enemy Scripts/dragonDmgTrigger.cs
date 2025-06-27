using Holistic3D.Inventory;
using UnityEngine;

public class dragonDmgTrigger : MonoBehaviour
{
    [SerializeField] GameObject dragon;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            //gameObject.GetComponentInParent<dragonBoss>().isInvulnerable = true;
            //this.gameObject.GetComponent<dragonBoss>().isInvulnerable = true; 
            dragon.GetComponent<dragonBoss>().isInvulnerable = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //gameObject.GetComponentInParent<dragonBoss>().isInvulnerable = true;
            //this.gameObject.GetComponent<dragonBoss>().isInvulnerable = true; 
            dragon.GetComponent<dragonBoss>().isInvulnerable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //gameObject.GetComponentInParent<dragonBoss>().isInvulnerable = false;
            dragon.GetComponent<dragonBoss>().isInvulnerable = true;

            //this.gameObject.GetComponent<dragonBoss>().isInvulnerable = false;
        }
    }

}
