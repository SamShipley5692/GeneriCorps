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
    [SerializeField] Transform headPos;
    [SerializeField] Transform healerPos;
    [SerializeField] Transform fighterPos;
    [SerializeField] Transform arenaCenterPos;
    [SerializeField] Transform[] flyPos;
    [SerializeField] Transform[] groundPos;
    [SerializeField] Collider jawCol;
    [SerializeField] GameObject dragonFire;
    [SerializeField] GameObject spawnHealer;
    [SerializeField] GameObject spawnFighter;


    [SerializeField][Range(0.001f, 1)] float dissolveRate;
    [SerializeField][Range(0.001f, 2)] float refreshRate;
    [SerializeField][Range(1, 200)] int HP;
    [SerializeField][Range(1, 15)] int roamDist;
    [SerializeField][Range(1, 50)] int faceTargetSpeed;
    [SerializeField][Range(1, 30)] int animTransSpeed;
    [SerializeField][Range(0.1f, 10)] float enemyDestroyTime;
    [SerializeField][Range(0.1f, 10)] float restTime;


    Vector3 startingPos;

    float stoppingDistOrig;

    int maxHP;
    int percentHP;

    float restTimer;

    bool playerInRange;
    bool isAttacking;

    private Material[] skinnedMaterials;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        maxHP = HP;
        percentHP = (HP / maxHP) * 100;
        anim.SetBool("isSleeping", true);

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
        setAnimPara();
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

    void setAnimPara() // instead of changing speed by setting animation parameters, increase speed of the rigid body?
    {
        if (transform.position.y < flyPos[0].position.y)
        {
            float agentGroundSpeedCur = agent.velocity.normalized.magnitude;
            float animGroundSpeedCur = anim.GetFloat("groundSpeed");
            anim.SetFloat("groundSpeed", Mathf.Lerp(animGroundSpeedCur, agentGroundSpeedCur, Time.deltaTime * animTransSpeed));
        }

        if (transform.position.y >= flyPos[0].position.y)
        {
            float agentFlySpeedCur = agent.velocity.normalized.magnitude;
            float animFlySpeedCur = anim.GetFloat("flySpeed");
            anim.SetFloat("flySpeed", Mathf.Lerp(animFlySpeedCur, agentFlySpeedCur, Time.deltaTime * animTransSpeed));
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
        if (percentHP > 10)
            HP -= amount;
        else
            HP -= (amount / 2);

        if (HP <= 0)
        {
            playerInRange = false;
            anim.SetTrigger("die");
            Destroy(gameObject, enemyDestroyTime);
            StartCoroutine(dissolve());
            gameManager.instance.updateGameGoal(-1);
        }

        else
        {
            anim.SetTrigger("getHit");
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(arenaCenterPos.position.x, arenaCenterPos.position.y, arenaCenterPos.position.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    void attackRoutine()
    {
        wakeUp();


        if (percentHP >= 70)
        {
            anim.SetBool("isMoving", false);
            wakeUp();
            //anim.SetBool("isBiting", true);
            StartCoroutine(biteAttack());
            StartCoroutine(rest());
            //groundRest();
            StartCoroutine(jumpAttack());
            StartCoroutine(rest());

            //groundRest();
        }
        else if (percentHP < 70 && percentHP >= 40)
        {  // slightly increase movement speed
            if (transform.position.y < flyPos[0].position.y)
            {
                stageTransition();
                takeOff();
            }
            stageTwo();
        }
        else if (percentHP < 40 && percentHP > 0)
        { // increase movement speed and reduce damage taken
            if (transform.position.y > groundPos[0].position.y)
            {
                land();
            }
            stageThree();
        }
        if (gameObject.transform.position.y < flyPos[0].position.y)
        {
            changePos();
        }
        else if (gameObject.transform.position.y > groundPos[0].position.y)
        {
            fly();
        }

    }

    void stageOne()
    {
        biteAttack();
        //groundRest();
        //StartCoroutine(groundRest());
        jumpAttack();
        //groundRest();
        //StartCoroutine(groundRest());
    }

    void stageTwo()
    {
        fly();
        flyAttack();
        //StartCoroutine(flyRest());
        //flyRest();
    }

    void stageThree()
    {
        summon();
        //StartCoroutine(groundRest());
        //groundRest();
        changePos();
        jumpAttack();
        //StartCoroutine(groundRest());
        //groundRest();
        changePos();
        flameAttack();
        //StartCoroutine(groundRest());
        //groundRest();
        changePos();
        biteAttack();
        //StartCoroutine(groundRest());
        //groundRest();

    }

    void wakeUp()
    {
        anim.SetBool("isSleeping", false);
        playerInRange = true;
    }

    IEnumerator rest()
    {
        anim.SetBool("isResting", true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("isResting", false);
    }

    IEnumerator biteAttack()
    {
        faceTarget();
        anim.SetBool("isBiting", true);
        yield return new WaitForSeconds(1.2f);
        anim.SetBool("isBiting", false);
    }

    IEnumerator jumpAttack()
    {
        faceTarget();
        anim.SetBool("isJumping", true);
        yield return new WaitForSeconds(3f);
        anim.SetBool("isJumping", false);
    }

    IEnumerator flameAttack()
    {
        faceTarget();
        anim.SetBool("isFiring", true);
        yield return new WaitForSeconds(2.7f);
        anim.SetBool("isFiring", false);
    }

    IEnumerator summon()
    {
        faceTarget();
        anim.SetBool("isSummoning", true);
        spawnEnemies();
        yield return new WaitForSeconds(3.4f);
        anim.SetBool("isSummoning", false);
    }

    void spawnEnemies()
    {
        if (spawnHealer)
            Instantiate(spawnHealer, healerPos.position, healerPos.transform.rotation);
        if (spawnFighter)
            Instantiate(spawnFighter, fighterPos.position, fighterPos.transform.rotation);
    }

    IEnumerator stageTransition()
    {
        anim.SetBool("isRoaring", true);
        yield return new WaitForSeconds(3.4f);
        anim.SetBool("isRoaring", true);

    }

    IEnumerator flyAttack()
    {
        faceTarget();
        anim.SetBool("isFlyAttacking", true);
        yield return new WaitForSeconds(3f);
        anim.SetBool("isFlyAttacking", false);
    }

    public void takeOff()
    {
        anim.SetBool("isAscending", true);

        Vector3 newPos = new Vector3(gameObject.transform.position.x, flyPos[0].position.y, gameObject.transform.position.z);
        gameObject.transform.position = newPos;
        //gameObject.transform.position = Vector3.MoveTowards(transform.position, newPos, agent.speed * Time.deltaTime);

        anim.SetBool("isAscending", false);
    }

    public void fly()
    {
        anim.SetBool("isFlying", true);

        int randIndex = Random.Range(0, flyPos.Length);
        Transform randPos = flyPos[randIndex];
        gameObject.transform.position = randPos.position;
        //gameObject.transform.position = Vector3.MoveTowards(transform.position, randPos.position, agent.speed * Time.deltaTime);

        anim.SetBool("isFlying", false);
    }

    public void land()
    {
        anim.SetBool("isDescending", true);

        Vector3 newPos = new Vector3(gameObject.transform.position.x, groundPos[0].position.y, gameObject.transform.position.z);
        gameObject.transform.position = newPos;
        //gameObject.transform.position = Vector3.MoveTowards(transform.position, newPos, agent.speed * Time.deltaTime);

        anim.SetBool("isDescending", false);
    }

    void changePos()
    {
        anim.SetBool("isMoving", true);

        if (agent.remainingDistance < 0.01f)
        {
            int ranIndex = Random.Range(0, groundPos.Length);
            Transform ranPos = groundPos[ranIndex];
            agent.destination = ranPos.position;
        }

        anim.SetBool("isMoving", false);
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
}
