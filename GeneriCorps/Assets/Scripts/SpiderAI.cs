using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;




public class SpiderAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer Model;
    [SerializeField] NavMeshAgent agent;
    //[SerializeField] Animation anim;
    [SerializeField] Transform headPOS;


    [SerializeField] Collider leftLeg;
    [SerializeField] Collider rightLeg;

    [SerializeField] int HP;
    //[SerializeField] int animTransSpeed;

    [SerializeField] float faceTargetSpeed;
    [SerializeField] float FOV;
    [SerializeField] float roamDist;
    [SerializeField] float roamPause;
    //[SerializeField] float attackRate

   
    Vector3 startingPOS;
    Vector3 playerDir;

    //float attackTimer;
    float roamTimer;
    float stoppingDistOrig;

    bool playerInRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      startingPOS = transform.position;
        stoppingDistOrig = agent.stoppingDistance;

        if (leftLeg != null) leftLeg.enabled = false;
        if (rightLeg != null) rightLeg.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        //attackTimer += Time.deltaTime;
        if (playerInRange && CanSeePlayer())
        {
            agent.SetDestination(gameManager.instance.player.transform.position);

            //if (attackTimer >= attackRate && agent.remainingDistance <= agent.stoppingDistance){

            //DoAttack():

            //}

            FaceTarget();
        
        }
        else
        {
            DoRoam();
        }
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

    //private void DoAttack()
    //{
       // attackTimer = 0f;

       // ATTACK ANIMATIONS GO HERE

       // EnableLegs();

       // TURN THEM OFF AFTER

       //Invoke(nameof(DisabledLegs), 0.5f);

    //}

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
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
