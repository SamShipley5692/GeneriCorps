using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public class enemyAI : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;


    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] Transform shootRate;

    Color colorOrig;

    Vector3 playerDirection;

    float shootTimer;

    bool playerInRange;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        shootTimer = 0f;
        // waiting on gamemanager script from Theo
        //if (gamemanager.instance != null)
        //{
            //gamemanager.instance.updateGameGoal(1);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        //waiting on gamemanagerscript
    }
}
