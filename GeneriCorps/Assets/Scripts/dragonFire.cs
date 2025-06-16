using UnityEngine;

public class dragonFire : MonoBehaviour
{
    [SerializeField][Range(1, 10)] int damageAmount;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other;
            player.GetComponent<PlayerHealth>().TakeDamage(damageAmount); 
        }
    }
}
