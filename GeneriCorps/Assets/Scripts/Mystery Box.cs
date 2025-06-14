using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public class MysteryBox : MonoBehaviour, IDamage
{

    [SerializeField] private GameObject[] possibleDrops;
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;
    [SerializeField][Range(1, 50)] int HP;
    [SerializeField] Animator anim;
    [SerializeField] Renderer Model;
    [SerializeField][Range(0.1f, 5)] float enemyDestroyTime;

    Color colorOrig;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());
        if (HP <= 0)
        {
            anim.SetTrigger("destroy");
            Destroy(gameObject, enemyDestroyTime);
          
           
        }
     
    }

    private void OnDestroy()
    {
        DropRandomItem();
    }

    private void DropRandomItem()
    {
        if (possibleDrops == null || possibleDrops.Length == 0) return;

        int idx = Random.Range(0, possibleDrops.Length);
        GameObject drop = possibleDrops[idx];
        if (drop != null)
            Instantiate(drop, transform.position + spawnOffset, Quaternion.identity);
 
    }

    IEnumerator flashRed()
    {
        Model.material.color = Color.red;
        yield return null;
        Model.material.color = colorOrig;
    }
}
