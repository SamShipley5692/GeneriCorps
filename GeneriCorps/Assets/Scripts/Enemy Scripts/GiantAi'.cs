
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class GiantAI : MonoBehaviour, IDamage
{

    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;
    [SerializeField] Collider weaponCol;
    [SerializeField] GameObject itemToDrop;


    [SerializeField][Range(1,100)] int HP;
    [SerializeField][Range(1,50)] int faceTargetSpeed;
    [SerializeField][Range(1,80)] int FOV;
    [SerializeField][Range(1,15)] int roamDist;
    [SerializeField][Range(1,5)] int roamPauseTime;
    [SerializeField][Range(1,30)] int animTransSpeed;
    [SerializeField][Range(0.1f,2)] float attackRate;
    [SerializeField][Range(0.1f, 15)] float deathScaleDuration;

    [SerializeField] SoundModulator modulator;
    [SerializeField] AudioSource effectAudio;
    [SerializeField] private AudioClip battleMusic;
    [SerializeField] AudioClip[] audDeath;
    [Range(0, 1)][SerializeField] float audDeathVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audWalk;
    [Range(0, 1)][SerializeField] float audWalkVol;
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audAttackVol;
    [SerializeField] private AudioMixerSnapshot ambientSnapshot;
    [SerializeField] private AudioMixerSnapshot battleSnapshot;
    private bool battleMusicPlayed = false;

    Vector3 playerDir;
    Vector3 startingPos;
    Vector3 startScale;

    float attackTimer;
    float angleToPlayer;
    float roamTimer;
    float stoppingDistOrig;

    bool playerInRange;
    bool isPlayingStep;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (effectAudio == null)
            effectAudio = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
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

        if (battleMusicPlayed && !canSeePlayer())
        {
            ambientSnapshot.TransitionTo(1.0f);
            battleMusicPlayed = false;
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

        if (effectAudio != null && audWalk.Length > 0)
        {
            if (!isPlayingStep)
                StartCoroutine(PlayStep());
        }

        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
    }

    IEnumerator PlayStep()
    {
        isPlayingStep = true; // isPlayingFlight
        modulator.PlayOneShotModulated(audWalk[Random.Range(0, audWalk.Length)], audWalkVol);

        yield return new WaitForSeconds(0.4f);

        isPlayingStep = false;
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

                if (!battleMusicPlayed)
                {
                    battleSnapshot.TransitionTo(1.0f);    
                    battleMusicPlayed = true;
                }
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
            playerInRange = true;
            agent.stoppingDistance = 0;
        }

        if (battleMusicPlayed)
        {
            ambientSnapshot.TransitionTo(1.0f); 
            battleMusicPlayed = false;
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
                modulator.PlayOneShotModulated(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
            }
            gameManager.instance.updateGameGoal(-1);
            anim.SetTrigger("die");

            agent.isStopped = true;

            StartCoroutine(scaleDown());

            //OnDestroy();
        }
        else
        {
            if (effectAudio != null && audHurt.Length > 0)
            {
                modulator.PlayOneShotModulated(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
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

    private void OnDestroy()
    {
            if (itemToDrop)
                Instantiate(itemToDrop, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z), Quaternion.identity);
    }










}
