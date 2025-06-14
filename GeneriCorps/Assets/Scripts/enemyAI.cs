using UnityEngine;
using System.Collections;
using UnityEngine.AI;



public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent navAgent;
    
    [SerializeField] Animator anim;
    [SerializeField][Range(1, 30)] int roamDist;
    [SerializeField][Range(1, 5)] int roamPauseTime;
    [SerializeField][Range(1, 90)] int FOV;
    [SerializeField][Range(1, 50)] float faceTargetSpeed;
    [SerializeField][Range(0.1f, 2)] float attackRate;
    [SerializeField][Range(0.1f, 5)] int enemyDestroyTime;
    [SerializeField] int animTransSpeed;



    // enemy HP 
    [SerializeField] int HP;
    int HPOriginal;
    [SerializeField] int rotationSpeed;

    [SerializeField] float shootRate;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootPos;

    public bool canSeePlayer;
    private bool isInQueue = false;

    Color colorOrig;

    Vector3 lookDirection;

    float shootTimer;

    bool isPlayerNearby;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.updateGameGoal(1);

        HPOriginal = HP;
        updateEnemyHP();

    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (!isPlayerNearby || gameManager.instance == null || gameManager.instance.player == null)
            return;

        Vector3 targetPos = gameManager.instance.player.transform.position;
        lookDirection = targetPos - transform.position;

        navAgent.SetDestination(targetPos);

        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            turnToFace();
        }

        if (shootTimer >= shootRate)
        {
            shoot();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Orc: Player ENTERED trigger. isPlayerNearby = " + isPlayerNearby);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Orc: Player EXITED trigger. isPlayerNearby = " + isPlayerNearby);
        }

    }

    public void takeDamage(int damage)
    {
        HP -= damage;
        updateEnemyHP();

        if (gameManager.instance != null && gameManager.instance.player != null)
        {
            navAgent.SetDestination(gameManager.instance.player.transform.position);
        }

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(hitFlash());
        }
    }

    IEnumerator hitFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        model.material.color = colorOrig;
    }


    void turnToFace()
    {
        Vector3 flat = new Vector3(lookDirection.x, 0f, lookDirection.z);
        Quaternion faceRot = Quaternion.LookRotation(flat);
        transform.rotation = Quaternion.Lerp(transform.rotation, faceRot, Time.deltaTime * rotationSpeed);
    }

    void shoot()
    {
        Debug.Log("Orc: SHOOT function CALLED!");
        shootTimer = 0;

        Instantiate(projectile, shootPos.position, transform.rotation);

    }


    public void updateEnemyHP()
    {
        gameManager.instance.enemyHPBar.fillAmount = (float)HP / HPOriginal;
    }

    void setAnimParameter() 
    {
        anim.SetFloat("Speed", navAgent.velocity.normalized.magnitude);
    }

    //adding For PoliteAiQueue 
    public void EnableAttack()
    {
        canSeePlayer = true;
        shootTimer = shootRate; 
    }

    public void WaitInQueue()
    {
        canSeePlayer = false;
        navAgent.SetDestination(transform.position); 
    }
}
