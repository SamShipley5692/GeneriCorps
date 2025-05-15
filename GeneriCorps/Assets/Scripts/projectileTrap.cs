using UnityEngine;

public class projectileTrap : MonoBehaviour
{
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject projectile;
    [SerializeField] float shootRate;

    float shootTimer;
    bool playerInRange;

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        
        if (playerInRange && shootTimer >= shootRate)
        {
            shoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void shoot()
    {
        shootTimer = 0;
        Instantiate(projectile, shootPos.position, transform.rotation);
    }

}
