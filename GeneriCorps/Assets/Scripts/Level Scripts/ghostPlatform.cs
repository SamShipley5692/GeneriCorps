using System.Collections;
using UnityEngine;

public class ghostPlatform : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] float disappearTime = 3;
    Animator myAnim;

    [SerializeField] bool canRest;
    [SerializeField] float resetTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        myAnim = GetComponent<Animator>();
        myAnim.SetFloat("Disappear Time", 1/disappearTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == playerTag)
        {
            myAnim.SetBool("Trigger", true);
        }
    }

    public void TriggerReset()
    {
        if(canRest)
        {
            StartCoroutine(Reset());
        }
    }
    
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetTime);
        myAnim.SetBool("Trigger", false);
    }
}
 