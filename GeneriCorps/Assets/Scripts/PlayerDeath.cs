using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public GameObject player;
    public Transform respawnPoint;
    public Animator playerAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        //player.transform.position = respawnPoint.position;
    }

    public void Die()
    {
        if (!playerAnimator.enabled)
        {
            playerAnimator.enabled = true;
        }
        playerAnimator.SetTrigger("death");
    }






}
