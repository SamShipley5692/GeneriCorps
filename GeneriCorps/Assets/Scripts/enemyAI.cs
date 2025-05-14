using UnityEngine;
using System.Collections;
using UnityEngine.AI;
//using UnityEditor.Build.Content;


public class enemyAI : MonoBehaviour //,//IDamage
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

    //Gio or Cade will do the OnTrigger collider code 
    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {

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
        model.material.color = colorOrig;
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
        fireCooldown = 0f;
        //projectile code cade or gio
    }
}

