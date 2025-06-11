using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Holistic3D.Inventory;
public class playercontroller : MonoBehaviour, IDamage, IPickup, IOpen
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] AudioSource aud;

    // World 
    [SerializeField] int gravity;

    // Player
    Vector3 moveDir;
    Vector3 playerVel;
    private Rigidbody2D rb;
    public float jump;
    public float Speed = 10;
    private float moveInput;

    bool isPlayingStep;
    bool isSprinting;
    int jumpCount;
    int HPOrig;
    int jumpForceOrig;
    int speedOrig;

    [SerializeField] int hp;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;

    [SerializeField] int jumpMax;
    [SerializeField] int jumpForce;

    // Weapon
    [SerializeField] GameObject gunModel;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;

    //Audio
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0,1)] [SerializeField] float audHurtVol;
    [SerializeField] AudioClip[]  audSteps;
    [Range(0, 1)][SerializeField] float audStepVol;


    float shootTimer;

    int weaponInvPos;
    [SerializeField] List<weaponStats> weaponInv = new List<weaponStats>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpForceOrig = jumpForce;
        speedOrig = speed;
        HPOrig = hp;
        updatePlayerUI();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        
        if(!gameManager.instance.isPaused)
            movement();

        sprint();

        if (Input.GetButtonDown("Jump"))
        {

            //rb.AddForce(new Vector2(rb.Velocity.x, jump));
        }
    }

    void movement()
    {
        shootTimer += Time.deltaTime;

        if (controller.isGrounded)
        {
            if(moveDir.normalized.magnitude > 0.3f && !isPlayingStep)
                StartCoroutine(PlayStep());
            
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);

        //transform.position += moveDir * speed * Time.deltaTime;

        controller.Move(moveDir * speed * Time.deltaTime);

        Jump();

        controller.Move(playerVel * Time.deltaTime);
        
        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && weaponInv.Count > 0 && shootTimer > shootRate)
        {
            Shoot();
        }

        selectWeapon();
    }
    IEnumerator PlayStep()
    {
        isPlayingStep = true;
        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepVol);
        if (isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        isPlayingStep = false;
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpForce;
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
        }
    }

    void Shoot()
    {
        shootTimer = 0;

        aud.PlayOneShot(weaponInv[weaponInvPos].shootSound[Random.Range(0, weaponInv[weaponInvPos].shootSound.Length)], weaponInv[weaponInvPos].shootSoundVol);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            //Debug.Log(hit.transform.name);

            Instantiate(weaponInv[weaponInvPos].hitEffect, hit.point, Quaternion.identity);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null) 
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    public void takeDamage(int amount) 
    {
        aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
        hp -= amount;
        updatePlayerUI();
        StartCoroutine(flashDamageScreen());

        // check for death
        if (hp <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)hp / HPOrig;
    }

    IEnumerator flashDamageScreen()
    {
        gameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageScreen.SetActive(false);
    }

    public void getWeaponStats(weaponStats weapon)
    {
        weaponInv.Add(weapon);
        weaponInvPos = weaponInv.Count - 1;
        changeWeapon();
    }

    void selectWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && weaponInvPos < weaponInv.Count - 1)
        {
            weaponInvPos++;
            changeWeapon();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && weaponInvPos > 0)
        {
            weaponInvPos--;
            changeWeapon();
        }
    }
    void changeWeapon()
    {
        shootDamage = weaponInv[weaponInvPos].shootDamage;
        shootDist = weaponInv[weaponInvPos].shootDistance;
        shootRate = weaponInv[weaponInvPos].shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = weaponInv[weaponInvPos].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = weaponInv[weaponInvPos].model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void getHealthItemStats(healthItems item) // added this method - Sam
    {
        int health = item.healthAmount;
        hp += health;
        if (hp > HPOrig)
        {
            hp = HPOrig;
        }
        updatePlayerUI();
    }

    public void getSpeedItemStats(speedItems item) // Cade 
    {
       StartCoroutine(applySpeedBuff(item.speedAmount, item.buffDuration));

        
    }

    private IEnumerator applySpeedBuff(int bonus, float duration) // Cade
    {
        speed += bonus;
        yield return new WaitForSeconds(duration);
        speed = speedOrig;
    }

    public void getJumpItemStats(jumpItems item) // Cade
    {
       StartCoroutine(applyJumpBuff(item.jumpForceAmount, item.buffDuration));

    }

    private IEnumerator applyJumpBuff(int bonus, float duration) // Cade
    {
        jumpForce += bonus;
        yield return new WaitForSeconds(duration);
        jumpForce = jumpForceOrig;
    }

        // updated for status effect poison
        public int health = 100;

        public void TakeDamage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                
                Debug.Log("Character has died.");
            }
        }
    }
