using UnityEngine;

public class pickup : MonoBehaviour
{
    [SerializeField] weaponStats weapon;
    [SerializeField] healthItems itemPickup; // added this
    [SerializeField] speedItems speedPickup;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pickupReceiver = other.GetComponent<IPickup>();

        //if (pickupReceiver !=null)
        //{
        //    pickupReceiver.getWeaponStats(weapon);
        //    Destroy(gameObject);
        //}

        // added this below and commented out above - Sam

        if (pickupReceiver != null)
        {
            if (weapon != null)
            {
                pickupReceiver.getWeaponStats(weapon);
                Destroy(gameObject);
            }

            if (itemPickup != null)
            {
                pickupReceiver.getHealthItemStats(itemPickup);
                Destroy(gameObject);
            }

            if (speedPickup != null)
            {
                pickupReceiver.getSpeedItemStats(speedPickup);
                Destroy(gameObject);
            }
        }
        
    }
}
