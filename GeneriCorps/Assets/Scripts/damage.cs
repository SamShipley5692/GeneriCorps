using UnityEngine;
using System.Collections;

public class damage : MonoBehaviour
{
    enum damageType { moving, homing, stationary, DOT }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int damageRate;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    bool isDamaging;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == damageType.moving || type == damageType.homing)
        {
            Destroy(gameObject, destroyTime);

            if (type == damageType.moving)
            {
                rb.linearVelocity = transform.forward * speed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == damageType.homing)
        {
            // waiting on Theo to make the gameManager script for this line of code to work
            // need Tymel J to add the player and playerController before Theo can make the gameManager script
            //rb.linearVelocity = (gameManager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        //IDamage dmg = other.GetComponent<IDamage>();

        //if (dmg != null && (type == damageType.moving || type == damageType.stationary || type == damageType.homing))
        //{
        //    dmg.takeDamage(damageAmount);
        //}

        if (type == damageType.moving || type == damageType.homing)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;
        // need to make IDamage script before this code will work
        //IDamage dmg = other.GetComponent<IDamage>();

        //if (dmg != null && type == damageType.DOT)
        //{
        //    if (!isDamaging)
        //        StartCoroutine(damageOther(dmg));
        //}
    }

    //IEnumerator damageOther(IDamage d)
    //{
    //    isDamaging = true;
    //    d.takeDamage(damageAmount);
    //    yield return new WaitForSeconds(damageRate);
    //    isDamaging = false;
    //}

}
