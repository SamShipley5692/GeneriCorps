using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class skeletonEnemy : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] int HP;
    [SerializeField] int rotationSpeed;
    [SerializeField] float attackCoolDown; // 3
    [SerializeField] float attackRange; // 1
    [SerializeField] float aggroRange; // 4

    GameObject player;
    Animator animator;
    Vector3 lookDirection;
    float attackTimer;
    float newDestCoolDown;
    float deathTimer;
    //bool playerInRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        gameManager.instance.updateGameGoal(1);

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", navAgent.velocity.magnitude / navAgent.speed);

        if (attackTimer >= attackCoolDown)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {
                animator.SetTrigger("attack");
                attackTimer = 0;
            }
        }

        attackTimer += Time.deltaTime;

        if (newDestCoolDown <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
        {
            newDestCoolDown = 1f;
            navAgent.SetDestination(player.transform.position);
        }
        newDestCoolDown -= Time.deltaTime;
        //transform.LookAt(player.transform);
        lookDirection = gameManager.instance.player.transform.position - transform.position;
        Vector3 flat = new Vector3(lookDirection.x, 0f, lookDirection.z);
        Quaternion faceRot = Quaternion.LookRotation(flat);
        transform.rotation = Quaternion.Lerp(transform.rotation, faceRot, Time.deltaTime * rotationSpeed);





        //animator.SetFloat("speed", navAgent.velocity.magnitude / navAgent.speed);
        //attackTimer += Time.deltaTime;

        //if (!playerInRange || gameManager.instance == null || gameManager.instance.player == null)
        //    return;

        //lookDirection = gameManager.instance.player.transform.position - transform.position;

        //navAgent.SetDestination(gameManager.instance.player.transform.position);

        //if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        //{
        //    turnToFace();
        //}

        //if (attackTimer >= attackCoolDown)
        //{
        //    attackTimer = 0;
        //    animator.SetTrigger("attack");
        //}

    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = true;
    //    }

    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = false;
    //    }

    //}

    public void takeDamage(int damage)
    {
        HP -= damage;
        animator.SetTrigger("damage");

        if (gameManager.instance != null && gameManager.instance.player != null)
        {
            navAgent.SetDestination(gameManager.instance.player.transform.position);
        }

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            animator.SetTrigger("die");
            deathTimer += Time.deltaTime;
            Destroy(gameObject);
        }

    }


    //void turnToFace()
    //{
    //    Vector3 flat = new Vector3(lookDirection.x, 0f, lookDirection.z);
    //    Quaternion faceRot = Quaternion.LookRotation(flat);
    //    transform.rotation = Quaternion.Lerp(transform.rotation, faceRot, Time.deltaTime * rotationSpeed);
    //}

    //IEnumerator die()
    //{
    //    animator.SetTrigger("die");
    //    yield return new WaitForSeconds(4f);
    //    Destroy(gameObject);
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
