using UnityEngine;
using UnityEngine.AI;

public class BabyDragonHealer : MonoBehaviour
{
    [SerializeField] float healAmount = 20f;
    [SerializeField] float healInterval = 5f;
    [SerializeField] Transform bossTarget;
    [SerializeField] Animator anim;
    [SerializeField] float safeDistance = 10f;
    [SerializeField] NavMeshAgent agent;

    float timer;

    void Start()
    {
        timer = healInterval;
    }

    void Update()
    {
        if (bossTarget == null) return;

        timer -= Time.deltaTime;

        
        float distance = Vector3.Distance(transform.position, bossTarget.position);
        if (distance < safeDistance)
        {
            Vector3 dir = (transform.position - bossTarget.position).normalized;
            agent.SetDestination(transform.position + dir * 2f); 
        }

        
        if (timer <= 0f)
        {
            HealBoss();
            timer = healInterval;
        }
    }

    void HealBoss()
    {
        IDamage healTarget = bossTarget.GetComponent<IDamage>();
        if (healTarget != null)
        {
            // Trigger healing animation
            if (anim != null) anim.SetTrigger("Heal");

            healTarget.takeDamage(-Mathf.RoundToInt(healAmount)); 
        }
    }
}