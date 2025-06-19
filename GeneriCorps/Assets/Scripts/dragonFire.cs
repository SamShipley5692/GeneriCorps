using UnityEngine;

public class dragonFire : MonoBehaviour
{
    [SerializeField][Range(1, 5)] int damageAmount;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();

            dmg.takeDamage(damageAmount);
        }
    }
}
