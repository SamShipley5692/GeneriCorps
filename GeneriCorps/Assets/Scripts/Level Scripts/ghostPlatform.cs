using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class ghostPlatform : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] float disappearTime = 3;
    Animator myAnim;

    [SerializeField] bool canRest;
    [SerializeField] float resetTime;

    BoxCollider[] _boxes;
    Collider _playerCol;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        myAnim = GetComponent<Animator>();

        _boxes = GetComponentsInChildren<BoxCollider>();

        var player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null)
            Debug.Log($"[GhostPlatform] could not find any GameObject tagged '{playerTag}'");
        else
            _playerCol = player.GetComponent<Collider>()
        ?? player.GetComponent<CharacterController>() as Collider;

        if (_playerCol == null)
            Debug.LogError($"[GhostPlatform] PLayer has no Collider or CharacterController!");


         
    }
    private void Start()
    {
        
        myAnim.SetFloat("Disappear Time", 1/disappearTime);

    
    }

    void OnTriggerEnter(Collider other)
    {
        if(other == _playerCol)
        {
            myAnim.SetBool("Trigger", true);

            Invoke(nameof(IgnorePlayer), disappearTime);

            if (canRest)
                StartCoroutine(Reset());
        }
    }

    void IgnorePlayer()
    {
        Debug.Log($"[{name}] Ignoring collisions with player at t={Time.time:F2}");

        foreach (var bc in _boxes)
            Physics.IgnoreCollision(bc, _playerCol, true);
      

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

        Debug.Log($"[{name}] Restoring collisions with player at t={Time.time:F2}");
        foreach (var bc in _boxes)
            Physics.IgnoreCollision(bc, _playerCol, false);

    }
}
 