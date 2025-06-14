using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class dragonBoss : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform arenaCenterPos;
    [SerializeField] Collider jawCol;
    [SerializeField] GameObject dragonFire;
    //[SerializeField] GameObject spawnHealer;
    //[SerializeField] GameObject spawnFighter;


    [SerializeField][Range(0.001f, 1)] float dissolveRate;
    [SerializeField][Range(0.001f, 2)] float refreshRate;
    [SerializeField][Range(1, 200)] int HP;
    [SerializeField][Range(1, 50)] int faceTargetSpeed;
    [SerializeField][Range(0.1f, 10)] float enemyDestroyTime;

    [SerializeField] AudioSource effectAudio;
    [SerializeField] AudioClip[] audRoar;
    [Range(0, 1)][SerializeField] float audRoarVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audDeath;
    [Range(0, 1)][SerializeField] float audDeathVol;
    [SerializeField] AudioClip[] audFlame;
    [Range(0, 1)][SerializeField] float audFlameVol;
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audAttackVol;
    //[SerializeField] AudioClip[] audFly;
    //[Range(0, 1)][SerializeField] float audFlyVol;

    int maxHP;
    int percentHP;

    bool playerInRange;
    bool isInvulnerable;
    bool coroutinePlayed;

    Material[] skinnedMaterials;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        maxHP = HP;
        percentHP = (HP / maxHP) * 100;
        anim.SetBool("isSleeping", true);
        isInvulnerable = false;
        coroutinePlayed = false;

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
        percentHP = (HP / maxHP) * 100;

        if (playerInRange)
        {
            attackRoutine();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void takeDamage(int amount)
    {
        if (!isInvulnerable)
        {
            if (percentHP > 10)
                HP -= amount;
            else
                HP -= (amount / 2);

            if (HP <= 0)
            {
                playerInRange = false;
                anim.SetTrigger("die");
                if (effectAudio != null && audHurt.Length > 0)
                {
                    effectAudio.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
                }
                Destroy(gameObject, enemyDestroyTime);
                StartCoroutine(dissolve());
                gameManager.instance.updateGameGoal(-1);
            }

            else
            {
                if (effectAudio != null && audHurt.Length > 0)
                {
                    effectAudio.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
                }
                anim.SetTrigger("getHit");
            }
        }
    }

    void faceTarget()
    {
        if (arenaCenterPos)
        {
            Vector3 direction = arenaCenterPos.position - transform.position;
            direction.Normalize();
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
        }
    }

    void attackRoutine()
    {
        faceTarget();

        if (coroutinePlayed == false)
        {
            StartCoroutine(wakeUp());
        }

        if (HP > 0)
        {
            StartCoroutine(AttackCycle());
        }



        //if (percentHP >= 70)
        //{
        //    anim.SetBool("isMoving", false);
        //    StartCoroutine(StageOne());
            
        //}

        //else if (percentHP < 70 && percentHP >= 40) // slightly increase movement speed
        //{  
        //    anim.SetBool("isMoving", false);
        //    StartCoroutine(StageTwo());
        //}

        //else if (percentHP < 40 && percentHP > 0) // increase movement speed and reduce damage taken
        //{ 
        //    StartCoroutine(StageThree());
        //}

    }

    IEnumerator AttackCycle()
    {
        // bite attack
        isInvulnerable = true;

        anim.SetBool("isBiting", true);
        if (effectAudio != null && audAttack.Length > 0)
        {
            effectAudio.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
        }
        yield return new WaitForSeconds(1.167f);
        anim.SetBool("isBiting", false);
        isInvulnerable = false;

        // rest animations for damage from player
        //anim.SetBool("isResting", true);
        //yield return new WaitForSeconds(2f);
        //anim.SetBool("isResting", false);

        // jump attack
        isInvulnerable = true;

        anim.SetBool("isJumping", true);
        if (effectAudio && audAttack.Length > 0)
        {
            effectAudio.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
        }
        yield return new WaitForSeconds(3f);
        anim.SetBool("isJumping", false);
        isInvulnerable = false;

        // rest animations for damage from player
        //anim.SetBool("isResting", true);
        //yield return new WaitForSeconds(2f);
        //anim.SetBool("isResting", false);

        // flame attack
        isInvulnerable = true;

        anim.SetBool("isFiring", true);
        if (effectAudio != null && audFlame.Length > 0)
        {
            effectAudio.PlayOneShot(audFlame[Random.Range(0, audFlame.Length)], audFlameVol);
        }
        yield return new WaitForSeconds(2.667f);
        anim.SetBool("isFiring", false);
        isInvulnerable = false;

        // rest animations for damage from player
        //anim.SetBool("isResting", true);
        //yield return new WaitForSeconds(2f);
        //anim.SetBool("isResting", false);
    }
   
    IEnumerator wakeUp()
    {
        anim.SetBool("isSleeping", false);
        yield return new WaitForSeconds(1f);
        anim.SetBool("isRoaring", true);
        if (effectAudio && audRoar.Length > 0)
        {
            effectAudio.PlayOneShot(audRoar[Random.Range(0, audRoar.Length)], audRoarVol);
        }
        yield return new WaitForSeconds(3.4f);
        anim.SetBool("isRoaring", false);
        playerInRange = true;
        coroutinePlayed = true;
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

    public void enableFlame()
    {
        if (dragonFire != null)
            dragonFire.SetActive(true);
    }

    void disableFlame()
    {
        if (dragonFire != null)
            dragonFire.SetActive(false);
    }


    //void setAnimPara() 
    //{
    //    if (transform.position.y < flyPos[0].position.y)
    //    {
    //        float agentGroundSpeedCur = agent.velocity.normalized.magnitude;
    //        float animGroundSpeedCur = anim.GetFloat("groundSpeed");
    //        anim.SetFloat("groundSpeed", Mathf.Lerp(animGroundSpeedCur, agentGroundSpeedCur, Time.deltaTime * animTransSpeed));
    //    }

    //    if (transform.position.y >= flyPos[0].position.y)
    //    {
    //        float agentFlySpeedCur = agent.velocity.normalized.magnitude;
    //        float animFlySpeedCur = anim.GetFloat("flySpeed");
    //        anim.SetFloat("flySpeed", Mathf.Lerp(animFlySpeedCur, agentFlySpeedCur, Time.deltaTime * animTransSpeed));
    //    }

    //}

    //IEnumerator biteAttack()
    //{
    //    isInvulnerable = true;

    //    if (effectAudio != null && audAttack.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
    //    }

    //    faceTarget();
    //    anim.SetBool("isBiting", true);
    //    yield return new WaitForSeconds(8.2f); // 1.2f is the time it takes for the bite animation to finish, 8.2f for the first stage
    //    anim.SetBool("isBiting", false);
    //    isInvulnerable = false;
    //}

    //IEnumerator jumpAttack()
    //{
    //    isInvulnerable = true;

    //    if (effectAudio != null && audAttack.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
    //    }

    //    faceTarget();
    //    anim.SetBool("isJumping", true);
    //    yield return new WaitForSeconds(3f);
    //    anim.SetBool("isJumping", false);
    //    isInvulnerable = false;

    //}

    //IEnumerator flameAttack()
    //{
    //    isInvulnerable = true;

    //    if (effectAudio != null && audFlame.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audFlame[Random.Range(0, audFlame.Length)], audFlameVol);
    //    }

    //    faceTarget();
    //    anim.SetBool("isFiring", true);
    //    yield return new WaitForSeconds(2.7f);
    //    anim.SetBool("isFiring", false);
    //    isInvulnerable = false;
    //}

    //IEnumerator summon()
    //{
    //    isInvulnerable = true;

    //    if (effectAudio != null && audRoar.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audRoar[Random.Range(0, audRoar.Length)], audRoarVol);
    //    }

    //    faceTarget();
    //    anim.SetBool("isSummoning", true);
    //    spawnEnemies();
    //    yield return new WaitForSeconds(3.4f);
    //    anim.SetBool("isSummoning", false);
    //    isInvulnerable = false;
    //}

    //IEnumerator stageTransition()
    //{
    //    isInvulnerable = true;

    //    anim.SetBool("isRoaring", true);
    //    yield return new WaitForSeconds(3.4f);
    //    anim.SetBool("isRoaring", false);

    //    isInvulnerable = false;
    //}

    //IEnumerator flyAttack()
    //{
    //    isInvulnerable = true;

    //    if (effectAudio != null && audFlame.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audFlame[Random.Range(0, audFlame.Length)], audFlameVol);
    //    }

    //    faceTarget();
    //    anim.SetBool("isFlyAttacking", true);
    //    yield return new WaitForSeconds(3f);
    //    anim.SetBool("isFlyAttacking", false);

    //    isInvulnerable = false;
    //}

    //IEnumerator rest()
    //{
    //    anim.SetBool("isResting", true);
    //    yield return new WaitForSeconds(2f);
    //    anim.SetBool("isResting", false);
    //}

    //void spawnEnemies()
    //{
    //    if (spawnHealer)
    //        spawnHealer.SetActive(true);

    //    if (spawnFighter)
    //        spawnFighter.SetActive(true);
    //}

    //public void takeOff()
    //{
    //    isInvulnerable = true;

    //    if (effectAudio != null && audFly.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audFly[Random.Range(0, audFly.Length)], audFlyVol);
    //    }

    //    anim.SetBool("isAscending", true);

    //    Vector3 newPos = new Vector3(gameObject.transform.position.x, flyPos[0].position.y, gameObject.transform.position.z);
    //    gameObject.transform.position = newPos;
    //    //gameObject.transform.position = Vector3.MoveTowards(transform.position, newPos, agent.speed * Time.deltaTime);

    //    anim.SetBool("isAscending", false);
    //    isInvulnerable = false;

    //    anim.SetBool("isFlying", true);

    //}

    //public void fly()
    //{
    //    if (effectAudio != null && audFly.Length > 0)
    //    {
    //        if (!isPlayingFlight)
    //            StartCoroutine(PlayFlight());
    //    }

    //    int randIndex = Random.Range(0, flyPos.Length);
    //    Transform randPos = flyPos[randIndex];
    //    gameObject.transform.position = randPos.position;
    //    //gameObject.transform.position = Vector3.MoveTowards(transform.position, randPos.position, agent.speed * Time.deltaTime);

    //    anim.SetBool("isFlying", false);
    //}

    //public void land()
    //{
    //    isInvulnerable = true;

    //    if (effectAudio != null && audFly.Length > 0)
    //    {
    //        if (!isPlayingFlight)
    //            StartCoroutine(PlayFlight());
    //    }

    //    anim.SetBool("isDescending", true);

    //    Vector3 newPos = new Vector3(gameObject.transform.position.x, groundPos[0].position.y, gameObject.transform.position.z);
    //    gameObject.transform.position = newPos;
    //    //gameObject.transform.position = Vector3.MoveTowards(transform.position, newPos, agent.speed * Time.deltaTime);

    //    anim.SetBool("isDescending", false);

    //    isInvulnerable = false;
    //}

    //void changePos()
    //{
    //    isInvulnerable = true;

    //    if (effectAudio != null && audWalk.Length > 0)
    //    {
    //        if (!isPlayingStep)
    //            StartCoroutine(PlayStep());
    //    }

    //    if (agent.remainingDistance < 0.01f)
    //    {
    //        int ranIndex = Random.Range(0, groundPos.Length);
    //        Transform ranPos = groundPos[ranIndex];
    //        //agent.destination = ranPos.position;
    //        gameObject.transform.position = Vector3.MoveTowards(transform.position, ranPos.position, agent.speed * Time.deltaTime);

    //    }

    //    anim.SetBool("isMoving", false);
    //    isInvulnerable = false;
    //}

    //IEnumerator PlayStep()
    //{
    //    isPlayingStep = true; // isPlayingFlight
    //    effectAudio.PlayOneShot(audWalk[Random.Range(0, audWalk.Length)], audWalkVol);

    //    yield return new WaitForSeconds(0.4f);

    //    isPlayingStep = false;
    //}

    //IEnumerator PlayFlight()
    //{
    //    isPlayingFlight = true;
    //    effectAudio.PlayOneShot(audFly[Random.Range(0, audFly.Length)], audFlyVol);

    //    yield return new WaitForSeconds(0.7f);

    //    isPlayingFlight = false;
    //}

    //    IEnumerator StageOne()
    //    {
    //    isInvulnerable = true;
    //    faceTarget();

    //    if (effectAudio != null && audAttack.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
    //    }

    //    anim.SetBool("isBiting", true);
    //    yield return new WaitForSeconds(1.2f); 
    //    anim.SetBool("isBiting", false);
    //    isInvulnerable = false;

    //    anim.SetBool("isResting", true);
    //    yield return new WaitForSeconds(2f);
    //    anim.SetBool("isResting", false);

    //    isInvulnerable = true;

    //    if (effectAudio != null && audAttack.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
    //    }

    //    anim.SetBool("isJumping", true);
    //    yield return new WaitForSeconds(3f);
    //    anim.SetBool("isJumping", false);
    //    isInvulnerable = false;

    //    anim.SetBool("isResting", true);
    //    yield return new WaitForSeconds(2f);
    //    anim.SetBool("isResting", false);

    //    //if (anim.GetBool("isMoving") == true)
    //    //    changePos();
    //    //isInvulnerable = true;
    //    //anim.SetBool("isMoving", true);

    //    //if (effectAudio != null && audWalk.Length > 0)
    //    //{
    //    //    if (!isPlayingStep)
    //    //        StartCoroutine(PlayStep());
    //    //}

    //    //if (agent.remainingDistance < 0.01f)
    //    //{
    //    //    int ranIndex = Random.Range(0, groundPos.Length);
    //    //    Transform ranPos = groundPos[ranIndex];
    //    //    //agent.destination = ranPos.position;
    //    //    gameObject.transform.position = Vector3.MoveTowards(transform.position, ranPos.position, agent.speed * Time.deltaTime);

    //    //}

    //    //anim.SetBool("isMoving", false);
    //    //isInvulnerable = false;

    //}

    //IEnumerator StageTwo()
    //{
    //    if (gameObject.transform.position.y < flyPos[0].position.y)
    //    {
    //        //StartCoroutine(stageTransition());
    //        isInvulnerable = true;

    //        anim.SetBool("isRoaring", true);
    //        yield return new WaitForSeconds(3.4f);
    //        anim.SetBool("isRoaring", false);

    //        isInvulnerable = false;

    //        takeOff();
    //    }

    //    if (anim.GetBool("isFlying") == true)
    //        fly();

    //    //StartCoroutine(flyAttack());
    //    isInvulnerable = true;

    //    if (effectAudio != null && audFlame.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audFlame[Random.Range(0, audFlame.Length)], audFlameVol);
    //    }

    //    faceTarget();
    //    anim.SetBool("isFlyAttacking", true);
    //    yield return new WaitForSeconds(3f);
    //    anim.SetBool("isFlyAttacking", false);

    //    isInvulnerable = false;

    //    //StartCoroutine(rest());
    //    anim.SetBool("isResting", true);
    //    yield return new WaitForSeconds(2f);
    //    anim.SetBool("isResting", false);

    //    anim.SetBool("isFlying", true);

    //}

    //IEnumerator StageThree()
    //{
    //    if (gameObject.transform.position.y > groundPos[0].position.y)
    //    {
    //        land();

    //        //StartCoroutine(summon());
    //        isInvulnerable = true;

    //        if (effectAudio != null && audRoar.Length > 0)
    //        {
    //            effectAudio.PlayOneShot(audRoar[Random.Range(0, audRoar.Length)], audRoarVol);
    //        }

    //        agent.speed = agent.speed * 1.5f; // increase movement speed

    //        faceTarget();
    //        anim.SetBool("isSummoning", true);
    //        spawnEnemies();
    //        yield return new WaitForSeconds(3.4f);
    //        anim.SetBool("isSummoning", false);
    //        isInvulnerable = false;

    //        //StartCoroutine(rest());
    //        anim.SetBool("isResting", true);
    //        yield return new WaitForSeconds(2f);
    //        anim.SetBool("isResting", false);

    //    }
    //    anim.SetBool("isMoving", true);

    //    if (anim.GetBool("isMoving") == true)
    //        changePos();

    //    //StartCoroutine(biteAttack());
    //    isInvulnerable = true;

    //    if (effectAudio != null && audAttack.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
    //    }

    //    anim.SetBool("isBiting", true);
    //    yield return new WaitForSeconds(1.2f);
    //    anim.SetBool("isBiting", false);
    //    isInvulnerable = false;

    //    //StartCoroutine(rest());
    //    anim.SetBool("isResting", true);
    //    yield return new WaitForSeconds(2f);
    //    anim.SetBool("isResting", false);

    //    anim.SetBool("isMoving", true);

    //    if (anim.GetBool("isMoving") == true)
    //        changePos();

    //    faceTarget();

    //    isInvulnerable = true;

    //    if (effectAudio != null && audAttack.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
    //    }

    //    //StartCoroutine(jumpAttack());
    //    anim.SetBool("isJumping", true);
    //    yield return new WaitForSeconds(3f);
    //    anim.SetBool("isJumping", false);
    //    isInvulnerable = false;

    //    //StartCoroutine(rest());
    //    anim.SetBool("isResting", true);
    //    yield return new WaitForSeconds(2f);
    //    anim.SetBool("isResting", false);

    //    anim.SetBool("isMoving", true);

    //    if (anim.GetBool("isMoving") == true)
    //        changePos();

    //    faceTarget();

    //    //StartCoroutine(flameAttack());
    //    isInvulnerable = true;

    //    if (effectAudio != null && audFlame.Length > 0)
    //    {
    //        effectAudio.PlayOneShot(audFlame[Random.Range(0, audFlame.Length)], audFlameVol);
    //    }

    //    anim.SetBool("isFiring", true);
    //    yield return new WaitForSeconds(2.7f);
    //    anim.SetBool("isFiring", false);
    //    isInvulnerable = false;

    //    //StartCoroutine(rest());
    //    anim.SetBool("isResting", true);
    //    yield return new WaitForSeconds(2f);
    //    anim.SetBool("isResting", false);

    //    if (anim.GetBool("isMoving") == true)
    //        changePos();

    //    faceTarget();
    //}

}
