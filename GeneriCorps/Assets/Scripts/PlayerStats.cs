using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    int maxHealth;
    float currentHealth;
    string playerName;
    Vector3 strengthStaminaMana;
    public GameObject playerShield;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        maxHealth = 100;
        currentHealth = maxHealth;
        playerName = "Rob";
        Debug.Log("Player's Name is" + playerName);
        strengthStaminaMana = new Vector3(8.8f, 5.6f, 100f);




    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentHealth = currentHealth - 10;
            Debug.Log("Current Health = " + currentHealth);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            strengthStaminaMana = new Vector3(strengthStaminaMana.x, strengthStaminaMana.y, strengthStaminaMana.z - 10f);
            Debug.Log("Current Mana = " + strengthStaminaMana.z);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerShield.SetActive(true);
            strengthStaminaMana = new Vector3(strengthStaminaMana.x, strengthStaminaMana.y, strengthStaminaMana.z - 0.1f);
        }
        else
        {
            playerShield.SetActive(false);
        }
    }
}
