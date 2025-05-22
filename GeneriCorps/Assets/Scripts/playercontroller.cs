using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playercontroller : MonoBehaviour, IDamage, IPickup
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    // World 
    [SerializeField] int gravity;

    // Player
    Vector3 moveDir;
    Vector3 playerVel;

    bool isSprinting;
    int jumpCount;
    int HPOrig;

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

    float shootTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = hp;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        
        if(!gameManager.instance.isPaused)
            movement();

        sprint();
    }

    void movement()
    {
        shootTimer += Time.deltaTime;

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);

        //transform.position += moveDir * speed * Time.deltaTime;

        controller.Move(moveDir * speed * Time.deltaTime);

        Jump();

        controller.Move(playerVel * Time.deltaTime);
        
        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && shootTimer > shootRate)
        {
            Shoot();
        }
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
        }
    }

    void Shoot()
    {
        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.transform.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null) 
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    public void takeDamage(int amount) 
    {
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
        throw new System.NotImplementedException();
    }
}
