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
    [SerializeField] Transform flamePos;
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
    [SerializeField][Range(1, 50)] int faceTargetSpeed;
    [SerializeField][Range(1, 30)] int animTransSpeed;
    [SerializeField][Range(0.1f, 10)] int enemyDestroyTime;
    [SerializeField][Range(0.1f, 10)] float restTime;
    [SerializeField][Range(0.1f, 10)] float spawnDelay;


    Vector3 startingPos;

    float stoppingDistOrig;

    int maxHP;
    int percentHP;

    bool playerInRange;
    bool isWoken;
    bool isDamageable;

    private Material[] skinnedMaterials;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        maxHP = HP;
        percentHP = (HP / maxHP) * 100;

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

        if (isWoken)
        {
            StartCoroutine(wakeUp());
        }
        if (playerInRange)
        {
            if (percentHP >= 70)
            {
                stageOne();
                changePos();
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
            isWoken = true;
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
        Quaternion rot = Quaternion.LookRotation(new Vector3(arenaCenterPos.transform.position.x, arenaCenterPos.transform.position.y, arenaCenterPos.transform.position.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    void stageOne()
    {
        biteAttack();
        StartCoroutine(groundRest());
        jumpAttack();
        StartCoroutine(groundRest());
    }

    void stageTwo()
    {
        fly();
        flyAttack();
        StartCoroutine(flyRest());
    }

    void stageThree()
    {
        roar();
        StartCoroutine(groundRest());
        changePos();
        jumpAttack();
        StartCoroutine(groundRest());
        changePos();
        flameAttack();
        StartCoroutine(groundRest());
        changePos();
        biteAttack();
        StartCoroutine(groundRest());
    }

    IEnumerator wakeUp()
    {
        faceTarget();
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("roar");
        yield return new WaitForSeconds(3f);
        isWoken = false;
        playerInRange = true;
    }

    IEnumerator groundRest()
    { // change length of rest time depending on enemy health
      // anim.SetTrigger(“idle”);
        yield return new WaitForSeconds(restTime);
    }
    IEnumerator flyRest()
    { // change length of rest time depending on enemy health
      // anim.SetTrigger(“flyIdle”);
        yield return new WaitForSeconds(restTime);
    }

    void biteAttack()
    {
        faceTarget();
        anim.SetTrigger("biteAttack");
    }

    void jumpAttack()
    {
        faceTarget();
        anim.SetTrigger("clawAttack");
    }

    void flameAttack()
    {
        faceTarget();
        anim.SetTrigger("flameAttack");
    }

    void roar()
    {
        anim.SetTrigger("roar");
        StartCoroutine(spawnEnemies());
        faceTarget();
    }

    IEnumerator spawnEnemies()
    {
        yield return new WaitForSeconds(spawnDelay);
        if (spawnHealer)
            Instantiate(spawnHealer, healerPos.position, healerPos.transform.rotation);
        if (spawnFighter)
            Instantiate(spawnFighter, fighterPos.position, fighterPos.transform.rotation);

        yield return new WaitForSeconds(restTime);
    }

    void stageTransition()
    {
        anim.SetTrigger("fireRoar");
    }

    public void flyAttack()
    {
        faceTarget();
        anim.SetTrigger("flyAttack");
    }

    public void takeOff()
    { 
        anim.SetTrigger("takeOff");
        Vector3 newPos = new Vector3(gameObject.transform.position.x, flyPos[0].position.y, gameObject.transform.position.z);
        gameObject.transform.position = newPos;
        //transform.position = Vector3.MoveTowards(transform.position, newPos, movementSpeed * Time.deltaTime); 
        // get movement speed from rb? Or make it a variable I can control.
    }

    public void fly()
    {
        anim.SetTrigger("fly"); // may need to specify flying animation though it’s a float, not a trigger. Maybe set anim parameters here instead?
        int randIndex = Random.Range(0, flyPos.Length);
        Transform randPos = flyPos[randIndex];
        gameObject.transform.position = randPos.position;
        //transform.position = Vector3.MoveTowards(transform.position, ranPos, movementSpeed * Time.deltaTime); 
        // get movement speed from rb? Or make it a variable I can control.
    }

    public void land()
    { 
        anim.SetTrigger("land");
        Vector3 newPos = new Vector3(gameObject.transform.position.x, groundPos[0].position.y, gameObject.transform.position.z);
        gameObject.transform.position = newPos;
        //transform.position = Vector3.MoveTowards(transform.position, newPos, movementSpeed * Time.deltaTime); 
        // get movement speed from rb? Or make it a variable I can control.
    }

    void changePos()
    {
        //anim.SetTrigger("walk"); // may need to specify moving animation though it’s a float, not a trigger. Maybe set anim parameters here instead?
        int ranIndex = Random.Range(0, groundPos.Length);
        Transform ranPos = groundPos[ranIndex];
        gameObject.transform.position = ranPos.position;
        // transform.position = Vector3.MoveTowards(transform.position, ranPos, movementSpeed * Time.deltaTime); 
        // get movement speed from rb? Or make it a variable I can control.
    }

    public void defend()
    {
        anim.SetTrigger("defend");
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
