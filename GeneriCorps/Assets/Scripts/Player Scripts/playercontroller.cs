using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Holistic3D.Inventory;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playercontroller : MonoBehaviour, IDamage, IPickup, IOpen, IMovement, IAction
{

    [SerializeField] SoundModulator modulator;
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] AudioSource aud;

    // Status Effects
    [SerializeField] StatusEffectPoison poisonEffect;
    [SerializeField] StatusEffectOnFire fireEffect;
    [SerializeField] StatusEffectFreeze freezeEffect;

    [SerializeField] private gameManager gameManager;



    // World 
    [SerializeField] int gravity;

    // Player
    Vector3 moveDir;
    Vector3 playerVel;
    private Rigidbody2D rb;
    public float jump;
    public float Speed = 10;
    private float moveInput;
    public GameObject player;
    bool grounded;
    private float mobileInput;
    private int ammo = 10;
    private int Hp = 100;
    public Text HP;
    public Text ammoammount;
    public GameObject bulletEmitter;
    public GameObject bulletprefab;
    private int grapesdead = 0;
    public Image winning;
    public GameObject grape1;
    public GameObject grape2;
    private bool g1; 
    private bool g2;
    private bool shielded;
    [SerializeField]
    private GameObject shield;


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

    // Status effect controls
    public bool canMove = true;
    public bool canAct = true;

    public int health = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shielded = false;
        grounded = true;
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
        modulator.PlayOneShotModulated(audSteps[Random.Range(0, audSteps.Length)], audStepVol);
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
            modulator.PlayOneShotModulated(audJump[Random.Range(0, audJump.Length)], audJumpVol);
        }
    }

    void Shoot()
    {
        shootTimer = 0;
        if (aud != null && weaponInv[weaponInvPos].shootSound.Length > 0)
            modulator.PlayOneShotModulated(weaponInv[weaponInvPos].shootSound[Random.Range(0, weaponInv[weaponInvPos].shootSound.Length)], weaponInv[weaponInvPos].shootSoundVol);
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
        modulator.PlayOneShotModulated(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
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

    IEnumerator flashHealthScreen()
    {
        gameManager.instance.playerHealthScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerHealthScreen.SetActive(false);
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

        StartCoroutine(flashHealthScreen());
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

    public void getInvincibleStats(InvincibleItems item)
    {
        StartCoroutine(applyInvincibilityBuff(item.buffDuration));
    }

    private IEnumerator applyInvincibilityBuff(float duration)
    {
        controller.detectCollisions = false;

        yield return new WaitForSeconds(duration);

        controller.detectCollisions = true;
    }

    //Added for status effect control
    public void ApplyFireEffect()
    {
        if (fireEffect != null)
            fireEffect.Ignite();
    }

    public void ApplyPoisonEffect()
    {
        if (poisonEffect != null)
            poisonEffect.ApplyPoison();
    }

    public void ApplyFreezeEffect()
    {
        if (freezeEffect != null)
            freezeEffect.ApplyFreeze();
    }

    public void DisableMovement() => canMove = false;
    public void EnableMovement() => canMove = true;

    public void DisableActions() => canAct = false;
    public void EnableActions() => canAct = true;

    public weaponStats GetEquippedWeapon()
    {
        if (weaponInv.Count == 0)
        {
            return null;
        }
        return weaponInv[weaponInvPos];
    }

    public void RemoveEquippedWeapon()
    {
        if(weaponInv.Count == 0)
        {
            return;
        }

        weaponInv.RemoveAt(weaponInvPos);

        if(weaponInv.Count == 0)
        {
            gunModel.GetComponent<MeshFilter>().sharedMesh = null;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = null;
            return;
        }

        weaponInvPos = Mathf.Clamp(weaponInvPos, 0, weaponInv.Count - 1);
        changeWeapon();
    }
}
//trying to restore checkpoint but having issues with gamemanager will check on this later on
//public void RestoreToCheckpoint()
//{
    //hp = HPOrig;
    //health = HPOrig;
    //updatePlayerUI();
//}




