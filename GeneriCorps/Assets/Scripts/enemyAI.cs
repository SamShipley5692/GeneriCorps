using UnityEngine;
using System.Collections;
using UnityEngine.AI;



public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent navAgent;


    [SerializeField] int HP;
    [SerializeField] int rotationSpeed;

    [SerializeField] float fireDelay;

    [SerializeField] GameObject projectile;


    Color colorOrig;

    Vector3 lookDirection;

    float fireCooldown;

    bool isPlayerNearby;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        fireCooldown = 0f;
        // waiting on gamemanager script from Theo
        //if (gamemanager.instance != null)
        //{
        //gamemanager.instance.updateGameGoal(1);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        fireCooldown += Time.deltaTime;
        //if (!isPlayerNearby || gamemanager.instance == null || gamemanager.instance.player == null)
        return;

        //Vector3 targetPos = gamemanager.instance.player.transform.position;
        //lookDirection = targetPos - transform.position;

        //navAgent.SetDestination(targetPos);

        //if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        //{
        //turnToFace();
        //}

        //if (fireCooldown >= fireDelay)
        {
            //fire();
        }
    }

    //Gio: Collider code done. Awaiting gameManager
    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.CompareTag("Player"))
            {
                isPlayerNearby = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {   //OnTriggerExit is declared but never used warning, will check on it later today
        //void OnTriggerExit(Collider other)  
        {
            if (other.CompareTag("Player"))
            {
                isPlayerNearby = false;
            }
        }
    }

    public void takeDamage(int damage)
    {
        //HP -= damage;

        //if (gamemanager.instance != null && gamemanager.instance.player != null)
        {
            //navAgent.SetDestination(gamemanager.instance.player.transform.position);
        }

        //if (HP <= 0)
        {
            //if (gamemanager.instance != null)
            {
                //gamemanager.instance.updateGameGoal(-1);
            }

            //Destroy(gameObject);
        }
        //else
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

    void fire()
    {
        //Fire cooldown for ranged enemy. Awaiting GameManger
        { /* if (fireCooldown <= 0f)
        {
            fireCooldown = 1f / fireRate;
            if (projectilePrefab != null && firePoint != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = firePoint.forward * projectileSpeed;
                }
            }
            else
            {
                Debug.LogError("Projectile Prefab or Fire is not assigned");
            }
        } */
        }
    }
}

