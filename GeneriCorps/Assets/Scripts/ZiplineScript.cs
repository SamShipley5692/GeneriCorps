using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class ZiplineScript : MonoBehaviour
{
    public float go = 100f;
    public float range = 3f;

    public GameObject zipline;
    public bool isMoving = false;

    public Camera fpsCam;

    public void Update()
    {
        if(Input.GetKeyDown("f"))
        {
            Shoot();
        }
    }

    void Shoot ()
    {
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                StartCoroutine(ZiplineGo());
            }
        }
    }

    IEnumerator ZiplineGo()
    {
        isMoving = true;
        zipline.GetComponent<Animator>().Play("Zipline");
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(10f);
        zipline.GetComponent<Animator>().Play("New State");
        isMoving = false;
    }
}
