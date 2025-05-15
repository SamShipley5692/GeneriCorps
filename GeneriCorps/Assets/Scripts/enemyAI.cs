using UnityEngine;
using System.Collections;
using UnityEngine.AI;



public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent navAgent;


    [SerializeField] int HP;
    [SerializeField] int rotationSpeed;

    [SerializeField] float shootRate;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform shootPos;


    Color colorOrig;

    Vector3 lookDirection;

    float shootTimer;

    bool isPlayerNearby;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.updateGameGoal(1);

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

        //Instantiate(projectile, shootPos.position, transform.rotation);

    }


}

