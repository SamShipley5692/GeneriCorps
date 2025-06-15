using UnityEngine;

public class Mimic : MonoBehaviour

{
    [SerializeField] GameObject closedChest;
    [SerializeField] GameObject mimicModel;
    [SerializeField] Animator anim;
    [SerializeField] float attackDamage = 15f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] Transform player;
    [SerializeField] float moveSpeed = 3f;

    private bool isAwake = false;

    void Start()
    {
        mimicModel.SetActive(false);
        closedChest.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isAwake && other.CompareTag("Player"))
        {
            AwakeMimic();
        }
    }

    void Update()
    {
        if (!isAwake) return;

        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            AttackPlayer();
        }
    }

    void AwakeMimic()
    {
        isAwake = true;
        closedChest.SetActive(false);
        mimicModel.SetActive(true);
        if (anim != null) anim.SetTrigger("Awake");
    }

    void AttackPlayer()
    {
        IDamage dmg = player.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.takeDamage(Mathf.RoundToInt(attackDamage));
        }
    }
}