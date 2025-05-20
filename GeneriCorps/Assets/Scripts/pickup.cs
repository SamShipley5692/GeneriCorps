using UnityEngine;

public class pickup : MonoBehaviour
{
    [SerializeField] private weaponStats weaponData;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pickupReceiver = other.GetComponent<IPickup>();
        if (pickupReceiver !=null)
        {
            pickupReceiver.getWeaponStats(weaponData);
            Destroy(gameObject);
        }
    }
}
