using UnityEngine;

public class pickup : MonoBehaviour
{
    [SerializeField] weaponStats weapon;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pickupReceiver = other.GetComponent<IPickup>();

        if (pickupReceiver !=null)
        {
            pickupReceiver.getWeaponStats(weapon);
            Destroy(gameObject);
        }
    }
}
