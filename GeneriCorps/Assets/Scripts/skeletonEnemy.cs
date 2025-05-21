using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class skeletonEnemy : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] Collider weaponCol;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    [SerializeField] int animTransSpeed;
    [SerializeField] float attackRate;

    Vector3 playerDir;
    Vector3 startingPos;

    float attackTimer;
    float angleToPlayer;
    float roamTimer;
    float stoppingDistOrig;

    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        setAnimPara();

        attackTimer += Time.deltaTime;

        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
        }

        if (playerInRange && !canSeePlayer())
        {
            checkRoam();
        }
        else if (!playerInRange)
        {
            checkRoam();
        }
    }

    void setAnimPara()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("speed");

        anim.SetFloat("speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed));
    }

    void checkRoam()
    {
        if (roamTimer >= roamPauseTime && agent.remainingDistance < 0.01f)
        {
            roam();
        }
    }

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
    }

    bool canSeePlayer()
    {
        playerDir = (gameManager.instance.player.transform.position - headPos.position);
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        Debug.DrawRay(headPos.position, new Vector3(playerDir.x, 0, playerDir.z));

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (angleToPlayer <= FOV && hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (attackTimer >= attackRate)
                {
                    attack();
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }

                agent.stoppingDistance = stoppingDistOrig; 
                return true;
            }
        }

        agent.stoppingDistance = 0;
        return false;
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
            agent.stoppingDistance = 0;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        anim.SetTrigger("damage");

        agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            anim.SetTrigger("die");
            Destroy(gameObject);
        }

    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    public void attack()
    {
        anim.SetTrigger("attack");
        attackTimer = 0;
    }

    public void weaponColOn()
    {
        if (weaponCol != null)
            weaponCol.enabled = true;
    }

    public void weaponColOff()
    {
        if (weaponCol != null)
            weaponCol.enabled = false;
    }





    //[SerializeField] NavMeshAgent navAgent;
    //[SerializeField] int HP;
    //[SerializeField] int rotationSpeed;
    //[SerializeField] float attackCoolDown; // 3
    //[SerializeField] float attackRange; // 1
    //[SerializeField] float aggroRange; // 4

    //GameObject player;
    //Animator animator;
    //Vector3 lookDirection;
    //float attackTimer;
    //float newDestCoolDown;
    ////bool playerInRange;


    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    player = GameObject.FindWithTag("Player");
    //    animator = GetComponent<Animator>();
    //    gameManager.instance.updateGameGoal(1);

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    animator.SetFloat("speed", navAgent.velocity.magnitude / navAgent.speed);

    //    if (attackTimer >= attackCoolDown)
    //    {
    //        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
    //        {
    //            animator.SetTrigger("attack");
    //            attackTimer = 0;
    //        }
    //    }

    //    attackTimer += Time.deltaTime;

    //    if (newDestCoolDown <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
    //    {
    //        newDestCoolDown = 1f;
    //        navAgent.SetDestination(player.transform.position);
    //    }
    //    newDestCoolDown -= Time.deltaTime;
    //    lookDirection = gameManager.instance.player.transform.position - transform.position;
    //    Vector3 flat = new Vector3(lookDirection.x, 0f, lookDirection.z);
    //    Quaternion faceRot = Quaternion.LookRotation(flat);
    //    transform.rotation = Quaternion.Lerp(transform.rotation, faceRot, Time.deltaTime * rotationSpeed);

    //}


    //public void takeDamage(int damage)
    //{
    //    HP -= damage;
    //    animator.SetTrigger("damage");

    //    if (gameManager.instance != null && gameManager.instance.player != null)
    //    {
    //        navAgent.SetDestination(gameManager.instance.player.transform.position);
    //    }

    //    if (HP <= 0)
    //    {
    //        gameManager.instance.updateGameGoal(-1);
    //        animator.SetTrigger("die");
    //        Destroy(gameObject);
    //    }

    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, aggroRange);
    //}
}
