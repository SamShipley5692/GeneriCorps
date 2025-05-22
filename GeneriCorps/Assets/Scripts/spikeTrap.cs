using System.Collections;
using UnityEngine;

public class spikeTrap : MonoBehaviour
{
    [SerializeField] GameObject spikePrefab;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float speed;

    bool movingToB = true;
    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInRange = false;
        spikePrefab.transform.position = pointA.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            shoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void shoot()
    {
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        if (movingToB)
        {
            spikePrefab.transform.position = Vector3.MoveTowards(spikePrefab.transform.position, pointB.position, speed); // * Time.deltaTime
            if (spikePrefab.transform.position == pointB.position)
            {
                movingToB = false;
            }
        }
        yield return new WaitForSeconds(2f);
        if (!movingToB)
        {
            spikePrefab.transform.position = Vector3.MoveTowards(spikePrefab.transform.position, pointA.position, speed); // * Time.deltaTime
            if (spikePrefab.transform.position == pointA.position)
            {
                movingToB = true;
            }
        }
        yield return new WaitForSeconds(2f);
    }

}


