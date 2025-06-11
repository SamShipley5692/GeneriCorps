using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class dragonBoss : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] Transform flamePos;
    [SerializeField] Collider jawCol;
    [SerializeField] GameObject dragonFire;

    [SerializeField][Range(0.001f, 1)] float dissolveRate;
    [SerializeField][Range(0.001f, 2)] float refreshRate;
    [SerializeField][Range(1, 200)] int HP;
    [SerializeField][Range(1, 50)] int faceTargetSpeed;
    [SerializeField][Range(1, 90)] int FOV;
    [SerializeField][Range(1, 30)] int animTransSpeed;
    [SerializeField][Range(0.1f, 2)] float attackRate; // don't need
    [SerializeField][Range(0.1f, 10)] int enemyDestroyTime;

    Vector3 playerDir;
    Vector3 startingPos;

    float attackTimer; // don't need
    float angleToPlayer;
    float stoppingDistOrig;

    bool playerInRange;

    private Material[] skinnedMaterials; 



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;

        if (jawCol)
            jawCol.enabled = false;

        if (model)
        {
            skinnedMaterials = model.materials;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // for testing purposes, remove later
        {
            StartCoroutine(dissolve());
        }

        setAnimPara();

        attackTimer += Time.deltaTime;

        if (playerInRange)
        {

        }

    }

    IEnumerator dissolve()
    {
        if (skinnedMaterials.Length > 0)
        {
            float counter = 0;

            while (skinnedMaterials[0].GetFloat("_Dissolve_Amount") < 1f) 
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_Dissolve_Amount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }

    void setAnimPara()
    {
        float agentGroundSpeedCur = agent.velocity.normalized.magnitude;
        float animGroundSpeedCur = anim.GetFloat("groundSpeed");

        anim.SetFloat("groundSpeed", Mathf.Lerp(animGroundSpeedCur, agentGroundSpeedCur, Time.deltaTime * animTransSpeed));

        //float agentFlySpeedCur = agent.velocity.normalized.magnitude;
        //float animFlySpeedCur = anim.GetFloat("flySpeed");

        //anim.SetFloat("flySpeed", Mathf.Lerp(animFlySpeedCur, agentFlySpeedCur, Time.deltaTime * animTransSpeed));

    }

    bool canSeePlayer()
    {
        playerDir = (gameManager.instance.player.transform.position - headPos.position);
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        Debug.DrawRay(headPos.position, new Vector3(playerDir.x, 0, playerDir.z));

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (angleToPlayer <= FOV && hit.collider.CompareTag("Player") && HP > 0)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (attackTimer >= attackRate)
                {
                    groundAttack();
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

    public void takeDamage(int amount)
    {
        HP -= amount;

        //agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            playerInRange = false;
            anim.SetTrigger("die");
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(gameObject, enemyDestroyTime);
            StartCoroutine(dissolve());
        }
        else
        {
            anim.SetTrigger("getHit");
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    public void groundAttack()
    {
        string[] attackStates = { "biteAttack", "jumpAttack", "flameAttack" };
        int attackType = Random.Range(0, attackStates.Length); 

        switch(attackType)
        {
            case 0:
                anim.SetTrigger("biteAttack");
                break;
            case 1:
                anim.SetTrigger("jumpAttack");
                break;
            case 2:
                anim.SetTrigger("flameAttack");
                break;
            default:
                break;
        }
        float agentGroundSpeedCur = agent.velocity.normalized.magnitude;
        float animGroundSpeedCur = anim.GetFloat("groundSpeed");
        anim.SetFloat("groundSpeed", Mathf.Lerp(animGroundSpeedCur, agentGroundSpeedCur, Time.deltaTime * animTransSpeed));
    }

    public void flyAttack()
    {

        anim.SetTrigger("flyAttack");
    }

    public void defend()
    {
        anim.SetTrigger("defend");
    }
    public void flyTakeOff()
    {
        anim.SetTrigger("takeOff");
    }
    public void fly()
    {
        anim.SetTrigger("fly");
    }

    public void flyLand()
    {
        anim.SetTrigger("land");
    }

    public void jawColOn()
    {
        if (jawCol != null)
            jawCol.enabled = true;
    }

    public void jawColOff()
    {
        if (jawCol != null)
            jawCol.enabled = false;
    }

    public void instantiateFlame()
    {
        if (dragonFire != null)
            Instantiate(dragonFire, flamePos.position, headPos.transform.rotation);
    }


}
