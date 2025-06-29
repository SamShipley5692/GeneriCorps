using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class dragonBoss : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] Collider jawCol;
    [SerializeField] Collider hitCollider;
    [SerializeField] GameObject dragonFire;

    // added nav mesh agent and head pos for movement
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;

    // dissolve shader parameters
    [SerializeField][Range(0.001f, 1)] float dissolveRate;
    [SerializeField][Range(0.001f, 2)] float refreshRate;

    // health and death delay
    [SerializeField][Range(0, 200)] int HP;
    [SerializeField][Range(0.1f, 10)] float enemyDestroyTime;

    // movement parameters
    [SerializeField][Range(1, 50)] int faceTargetSpeed;
    [SerializeField][Range(1, 180)] int FOV;
    [SerializeField][Range(1, 30)] int animTransSpeed;

    // attack rate parameters
    [SerializeField][Range(0.1f, 10)] float attackRate;
    float attackTimer;
    bool coHasPlayed;

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

    Vector3 playerDir;

    float angleToPlayer;

    bool playerInRange;

    Material[] skinnedMaterials;

    enum attackType { Bite, Jump, Fire }
    attackType currentAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager.instance.updateGameGoal(1);
        anim.SetBool("isSleeping", true);
        hitCollider.enabled = false;
        coHasPlayed = false;

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
        attackTimer += Time.deltaTime;

        if (playerInRange && coHasPlayed && canSeePlayer())
        {
            setAnimPara();
        }

    }

    void setAnimPara()
    {
        if (anim.GetBool("isResting"))
        {
            anim.SetFloat("speed", 0);
        }

        else
        {
            float agentSpeedCur = agent.velocity.normalized.magnitude;
            float animSpeedCur = anim.GetFloat("speed");

            anim.SetFloat("speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTransSpeed));
        }
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
                if (!anim.GetBool("isResting"))
                {
                    agent.SetDestination(gameManager.instance.player.transform.position);
                }

                if (attackTimer >= attackRate)
                {
                    attack();
                    rest();
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }

                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            StartCoroutine(wakeUp());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            disableFlame();
            anim.StopPlayback();

            StartCoroutine(deathSequence());
            Destroy(gameObject, enemyDestroyTime);
            gameManager.instance.updateGameGoal(-1);
        }

        else
        {
            if (effectAudio != null && audHurt.Length > 0)
            {
                effectAudio.PlayOneShot(audHurt[UnityEngine.Random.Range(0, audHurt.Length)], audHurtVol);
            }
            anim.SetTrigger("getHit");
        }
        
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
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

    IEnumerator deathSequence()
    {
        anim.SetTrigger("die");

        if (effectAudio != null && audDeath.Length > 0)
        {
            effectAudio.PlayOneShot(audDeath[UnityEngine.Random.Range(0, audDeath.Length)], audDeathVol);
        }
        yield return new WaitForSeconds(2.133f);

        //start dissolve effect 
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

    void attack()
    {
        anim.SetBool("isResting", false);
        currentAttack = (attackType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(attackType)).Length);

        if (HP > 0)
        {
            switch (currentAttack)
            {
                case attackType.Bite:
                    anim.SetTrigger("bite");

                    if (effectAudio != null && audAttack.Length > 0)
                    {
                        effectAudio.PlayOneShot(audAttack[UnityEngine.Random.Range(0, audAttack.Length)], audAttackVol);
                    }
                    break;

                case attackType.Jump:
                    anim.SetTrigger("jump");

                    if (effectAudio != null && audAttack.Length > 0)
                    {
                        effectAudio.PlayOneShot(audAttack[UnityEngine.Random.Range(0, audAttack.Length)], audAttackVol);
                    }
                    break;

                case attackType.Fire:
                    anim.SetTrigger("fire");

                    if (effectAudio != null && audFlame.Length > 0)
                    {
                        effectAudio.PlayOneShot(audFlame[UnityEngine.Random.Range(0, audFlame.Length)], audFlameVol);
                    }
                    StartCoroutine(disableFlameDelay());
                    break;
            }
        }
        attackTimer = 0;
    }

    void rest()
    {
        // rest animations for damage from player
        anim.SetFloat("speed", 0);
        anim.SetBool("isResting", true);
        StartCoroutine(restDelay());
    }

    IEnumerator restDelay()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("isResting", false);
    }

    IEnumerator wakeUp()
    {
        // wake up dragon and roar
        anim.SetBool("isResting", true);
        anim.SetFloat("speed", 0);
        anim.SetBool("isSleeping", false);
        anim.SetBool("isRoaring", true);

        if (effectAudio && audRoar.Length > 0)
        {
            effectAudio.PlayOneShot(audRoar[UnityEngine.Random.Range(0, audRoar.Length)], audRoarVol);
        }

        yield return new WaitForSeconds(3.333f); // changed from 3 second to 1 second
        anim.SetBool("isRoaring", false);

        coHasPlayed = true;

        StartCoroutine(disableFlameDelay());
    }

    void disableDamage()
    {
        hitCollider.enabled = false;
    }

    void enableDamage()
    {
        hitCollider.enabled = true;
    }

    IEnumerator disableFlameDelay()
    {
        yield return new WaitForSeconds(3f);
        disableFlame();
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