using UnityEngine;

public class projectileTrap : MonoBehaviour
{
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject projectile;
    [SerializeField] float shootRate;

    [SerializeField] AudioSource effectAudio;
    [SerializeField] AudioClip[] audShoot;
    [Range(0, 1)][SerializeField] float audShootVol;

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

        if (effectAudio != null && audShoot.Length > 0)
        {
            effectAudio.PlayOneShot(audShoot[Random.Range(0, audShoot.Length)], audShootVol);
        }

        Instantiate(projectile, shootPos.position, transform.rotation);
    }

}
