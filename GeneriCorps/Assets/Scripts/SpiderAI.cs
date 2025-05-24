using UnityEngine;
using System.Collections;
using UnityEngine.AI;




public class SpiderAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer Model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPOS;


    [SerializeField] Collider leftLeg;
    [SerializeField] Collider rightLeg;

    [SerializeField][Range(1, 50)] int HP;
    [SerializeField][Range(1, 30)]int animTransSpeed;

    [SerializeField][Range(1, 50)] float faceTargetSpeed;
    [SerializeField][Range(1, 90)] int FOV;
    [SerializeField][Range(1, 20)] float roamDist;
    [SerializeField][Range(1,5)] float roamPause;
    [SerializeField][Range(0.1f, 2)] float attackRate;
    [SerializeField][Range(0.1f, 5)] int enemyDestroyTime;

    Color colorOrig;
   
    Vector3 startingPOS;
    Vector3 playerDir;

    float attackTimer;
    float roamTimer;
    float stoppingDistOrig;
    float angleToPlayer;

    bool playerInRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = Model.material.color;
        //COMMENTED CODE GIVING ERRORS 
        anim = GetComponent<Animator>();
        startingPOS = transform.position;
        stoppingDistOrig = agent.stoppingDistance;

        if (leftLeg != null) leftLeg.enabled = false;
        if (rightLeg != null) rightLeg.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        setAnimPara();

        attackTimer += Time.deltaTime;
        if (playerInRange && CanSeePlayer())
        {
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (attackTimer >= attackRate && agent.remainingDistance <= agent.stoppingDistance){

                DoAttack();

            }

            FaceTarget();
        
        }
        else
        {
            DoRoam();
        }
    }

    void setAnimPara()
    {
        float agentSpeedCurr = agent.velocity.normalized.magnitude;
        //COMMENTED CODE GIVING ERRORS
        float animSpeedCurr = anim.GetFloat("speed");
        //INCOMPLETE CODE, DID NOT WORK WHEN COMPLETED
        anim.SetFloat("speed", Mathf.Lerp(animSpeedCurr, agentSpeedCurr, Time.deltaTime * animTransSpeed));
    }

    private void DoRoam()
    {
        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
            if (roamTimer >= roamPause)
            {
                Vector3 randomPOS = Random.insideUnitSphere * roamDist;
                randomPOS += startingPOS;
                NavMeshHit hit;
                if(NavMesh.SamplePosition(randomPOS, out hit, roamDist, NavMesh.AllAreas))
                {
                    agent.stoppingDistance = 0f;
                    agent.SetDestination(hit.position);
                }
                roamTimer = 0f;
            }
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 playerDir = gameManager.instance.player.transform.position - headPOS.position;
        float angle = Vector3.Angle(playerDir, transform.forward);
        if (angle > FOV) return false;

        RaycastHit hit;
        if (Physics.Raycast(headPOS.position, playerDir.normalized, out hit))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    private void DoAttack()
    {

        anim.SetTrigger("attack");
        attackTimer = 0f;

      

        EnableLegs();

       Invoke(nameof(DisableLegs), 0.5f);

    }

    private void EnableLegs()
    {
        if (leftLeg != null) leftLeg.enabled = true;
        if (rightLeg != null) rightLeg.enabled = false;
    }

    private void DisableLegs()
    {
        if (leftLeg != null) leftLeg.enabled = false;
        if (rightLeg != null) rightLeg.enabled = false;
    }

    private void FaceTarget()
    {
        Vector3 lookDir = gameManager.instance.player.transform.position - transform.position;
        lookDir.y = 0f;
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * faceTargetSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            agent.stoppingDistance = stoppingDistOrig;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = stoppingDistOrig;
        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashRed());
        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            anim.SetTrigger("die");
            Destroy(gameObject, enemyDestroyTime);
        }
        else 
        {
            anim.SetTrigger("damage");
        }
    }

    IEnumerator flashRed()
    {
        Model.material.color = Color.red;
        yield return null;
        Model.material.color = colorOrig;
    }
}
