using UnityEngine;
using System.Collections;
using UnityEngine.AI;




public class BabySpider : MonoBehaviour, IDamage
{
    [SerializeField] Renderer Model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPOS;
    [SerializeField] GameObject itemToDrop;



    [SerializeField][Range(1, 50)] int HP;
    [SerializeField][Range(1, 30)] int animTransSpeed;

    [SerializeField][Range(1, 50)] float faceTargetSpeed;
    [SerializeField][Range(1, 360)] int FOV;
    [SerializeField][Range(1, 20)] float roamDist;
    [SerializeField][Range(1, 5)] float roamPause;
    [SerializeField] int damageAmount = 1;
    [SerializeField][Range(0.1f, 15)] float deathScaleDuration;

    [SerializeField] AudioSource effectAudio;
    [SerializeField] AudioClip[] audDeath;
    [Range(0, 1)][SerializeField] float audDeathVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audWalk;
    [Range(0, 1)][SerializeField] float audWalkVol;
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audAttackVol;

    Vector3 startingPOS;
    Vector3 playerDir;
    Vector3 startScale;


    float roamTimer;
    float stoppingDistOrig;
 
    float dropTimer;

    bool playerInRange;
    bool isPlayingStep;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //COMMENTED CODE GIVING ERRORS 
        anim = GetComponent<Animator>();
        startingPOS = transform.position;
        stoppingDistOrig = agent.stoppingDistance;

        startScale = transform.localScale;


    }

    // Update is called once per frame
    void Update()
    {
        setAnimPara();

        
        if (playerInRange && CanSeePlayer())
        {
            agent.SetDestination(gameManager.instance.player.transform.position);

      

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
        if (effectAudio != null && audWalk.Length > 0)
        {
            if (!isPlayingStep)
                StartCoroutine(PlayStep());
        }

        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
            if (roamTimer >= roamPause)
            {
                Vector3 randomPOS = Random.insideUnitSphere * roamDist;
                randomPOS += startingPOS;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPOS, out hit, roamDist, NavMesh.AllAreas))
                {
                    agent.stoppingDistance = 0f;
                    agent.SetDestination(hit.position);
                }
                roamTimer = 0f;
            }
        }
    }

    IEnumerator PlayStep()
    {
        isPlayingStep = true; // isPlayingFlight
        effectAudio.PlayOneShot(audWalk[Random.Range(0, audWalk.Length)], audWalkVol);

        yield return new WaitForSeconds(0.4f);

        isPlayingStep = false;
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
            var dmg = other.GetComponent<IDamage>();
            if (dmg != null) dmg.takeDamage(damageAmount);

            
            if (itemToDrop) Instantiate(itemToDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
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

        if (HP <= 0)
        {
            if (effectAudio != null && audDeath.Length > 0)
            {
                effectAudio.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
            }
            dropTimer += Time.deltaTime;
            gameManager.instance.updateGameGoal(-1);
            anim.SetTrigger("die");

            StartCoroutine(scaleDown());
            OnDestroy();
        }
        else
        {
            if (effectAudio != null && audHurt.Length > 0)
            {
                effectAudio.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
            }
            anim.SetTrigger("damage");
        }
    }

    IEnumerator scaleDown()
    {
        float time = 0;

        while (time < deathScaleDuration)
        {
            float scaleY = Mathf.Lerp(startScale.y, 0f, time / deathScaleDuration);
            transform.localScale = new Vector3(startScale.x, scaleY, startScale.z);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (itemToDrop)
            Instantiate(itemToDrop, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z), Quaternion.identity);
    }
}
