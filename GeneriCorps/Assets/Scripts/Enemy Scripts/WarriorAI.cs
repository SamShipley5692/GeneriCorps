using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WarriorAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;

    [SerializeField] NavMeshAgent agent;

    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] Collider axeCollider;
    [SerializeField] GameObject itemToDrop;

    [SerializeField][Range(1, 200)] public int HP;
    [SerializeField][Range(1, 50)] public int faceTargetSpeed;
    [SerializeField][Range(1, 80)] public int FOV;
    [SerializeField][Range(1, 15)] public int roamDist;
    [SerializeField][Range(1, 5)] public int roamPauseTime;
    [SerializeField][Range(1, 30)] public int animTransSpeed;
    [SerializeField][Range(0.1f, 2)] public float attackRate;
    [SerializeField][Range(0.1f, 5)] public int enemyDestroyTime;
    [SerializeField][Range(0, 10)] public int minKillCount;

    Color colorOrig;

   
    Vector3 playerDir;

    Vector3 startingPos;

    float attackTimer;
    float angleToPlayer;
    float roamTimer;
    float stoppingDistOrig;
    float dropTimer;

    int goalCountOrig;

    bool playerInRange;

    void Start()
    {
        colorOrig = model.material.color;
        anim = GetComponent<Animator>();
        startingPos = transform.position;
       
        stoppingDistOrig = agent.stoppingDistance;
        goalCountOrig = gameManager.instance.getGameGoalCount();

        if (axeCollider)
            axeCollider.enabled = false;
    }


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

        Vector3 ranPos = Random.insideUnitSphere * roamDist + startingPos;

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

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    public void attack()
    {
        anim.SetTrigger("attack");
       
        attackTimer = 0;
    }

    public void weaponColOn()
    {
        if (axeCollider != null)
            axeCollider.enabled = true;
    }

    public void weaponColOff()
    {
        if (axeCollider != null)
            axeCollider.enabled = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position);

        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            dropTimer += Time.deltaTime;
            gameManager.instance.updateGameGoal(-1);
            playerInRange = false;
            anim.SetTrigger("die");
            Destroy(gameObject, enemyDestroyTime);

            if (dropTimer > enemyDestroyTime)
                OnDestroy();
        }
        else
        {
            anim.SetTrigger("damage");
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        model.material.color = colorOrig;
    }

    void OnDestroy()
    {
        if (gameManager.instance.getGameGoalCount() <= (goalCountOrig - minKillCount))
        {
            Instantiate(itemToDrop, transform.position + Vector3.up * 4f, Quaternion.identity);
        }
    }

}
