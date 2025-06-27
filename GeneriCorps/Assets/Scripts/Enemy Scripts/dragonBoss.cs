using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class dragonBoss : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] Animator anim;
    [SerializeField] Collider jawCol;
    [SerializeField] GameObject dragonFire;

    [SerializeField][Range(0.001f, 1)] float dissolveRate;
    [SerializeField][Range(0.001f, 2)] float refreshRate;
    [SerializeField][Range(1, 200)] int HP;
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


    int maxHP;
    int percentHP;

    public bool isInvulnerable;

    Material[] skinnedMaterials;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager.instance.updateGameGoal(1);
        maxHP = HP;
        anim.SetBool("isSleeping", true);
        isInvulnerable = true;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attackRoutine();
        }
    }

    public void takeDamage(int amount)
    {
        if (!isInvulnerable)
        {
            HP -= amount;

            if (HP <= 0)
            {
                anim.SetBool("isRoaring", false);
                anim.SetBool("isBiting", false);
                anim.SetBool("isJumping", false);
                anim.SetBool("isFiring", false);
                anim.SetBool("isResting", false);
                disableFlame();

                StartCoroutine(deathSequence());
                Destroy(gameObject, enemyDestroyTime);
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
            effectAudio.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
        }
        yield return new WaitForSeconds(2.133f);

        // start dissolve effect
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

    void attackRoutine()
    {
        StartCoroutine(AttackCycle());
    }

    IEnumerator AttackCycle()
    {
        // wake up dragon and roar
        anim.SetBool("isSleeping", false);
        //yield return new WaitForSeconds(1f);
        anim.SetBool("isRoaring", true);
        if (effectAudio && audRoar.Length > 0)
        {
            effectAudio.PlayOneShot(audRoar[Random.Range(0, audRoar.Length)], audRoarVol);
        }
        yield return new WaitForSeconds(3.333f);
        anim.SetBool("isRoaring", false);
        StartCoroutine(disableFlameDelay());

        //start the repeating attack cycle
        while (HP > 0)
        {
            // bite attack
            anim.SetBool("isBiting", true);
            isInvulnerable = true;

            if (effectAudio != null && audAttack.Length > 0)
            {
                effectAudio.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
            }
            yield return new WaitForSeconds(1.167f);
            anim.SetBool("isBiting", false);
            isInvulnerable = false;

            // rest animations for damage from player
            anim.SetBool("isResting", true);
            yield return new WaitForSeconds(2f);
            anim.SetBool("isResting", false);

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

            //rest animations for damage from player
            anim.SetBool("isResting", true);
            yield return new WaitForSeconds(2f);
            anim.SetBool("isResting", false);

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
            StartCoroutine(disableFlameDelay());

            // rest animations for damage from player
            anim.SetBool("isResting", true);
            yield return new WaitForSeconds(2f);
            anim.SetBool("isResting", false);

            // if health below 50% summon baby dragons
            if (percentHP <= 50 && percentHP > 0)
            {
                anim.SetBool("isRoaring", true);

                // put code to summon baby dragons here

                if (effectAudio && audRoar.Length > 0)
                {
                    effectAudio.PlayOneShot(audRoar[Random.Range(0, audRoar.Length)], audRoarVol);
                }
                yield return new WaitForSeconds(3.333f);
                anim.SetBool("isRoaring", false);
                StartCoroutine(disableFlameDelay());
            }

        }
    }

    void summonDragons()
    {
        
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
