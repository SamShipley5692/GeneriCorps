using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class BabyDragonMelee : MonoBehaviour
{
    [SerializeField] float flySpeed = 20f;
    [SerializeField] float diveCooldown = 4f;
    [SerializeField] float attackDistance = 5f;
    [SerializeField] float damage = 20f;
    
    
    [SerializeField] Transform player;
    [SerializeField] Rigidbody rb;

   
    
    private bool isDiving = false;
   
    private float timer;

    void Update()
    {
       
        timer -= Time.deltaTime;

        if (!isDiving && timer <= 0f && Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            StartCoroutine(DiveAttack());
        }
    }

    System.Collections.IEnumerator DiveAttack()
    {
        isDiving = true;
        Vector3 direction = (player.position - transform.position).normalized;

#pragma warning disable CS0618 // was getting warning for velocity
        rb.velocity = direction * flySpeed;
#pragma warning restore CS0618

        yield return new WaitForSeconds(0.5f);

#pragma warning disable CS0618
        rb.velocity = Vector3.zero;
#pragma warning restore CS0618

        isDiving = false;
        timer = diveCooldown;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDiving) return;

        if (collision.transform.CompareTag("Player"))
        {
            IDamage dmg = collision.transform.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(Mathf.RoundToInt(damage));
            }
        }
    }
}