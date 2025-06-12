using UnityEngine;
using Holistic3D.Inventory;
public class pickup : MonoBehaviour
{
    [SerializeField] weaponStats weapon;
    [SerializeField] healthItems itemPickup; // added this
    [SerializeField] speedItems speedPickup; // Cade
    [SerializeField] jumpItems jumpPickup; // Cade
    [SerializeField] InvincibleItems invinciblePickup;

    public int quantity = 1; //Tenia

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
                playerInventorySystem playerInventory = other.GetComponent<playerInventorySystem>();//Tenia
                if (playerInventory != null)
                {
                    quantity = playerInventory.PickupItem(weapon, quantity);
                    if(quantity <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
                Destroy(gameObject);
            }

            if (itemPickup != null)
            {
                pickupReceiver.getHealthItemStats(itemPickup);
                playerInventorySystem playerInventory = other.GetComponent<playerInventorySystem>();//Tenia
                if (playerInventory != null)
                {
                    quantity = playerInventory.PickupItem(itemPickup, quantity);
                    if (quantity <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
                Destroy(gameObject);
            }

            if (speedPickup != null)
            {
                pickupReceiver.getSpeedItemStats(speedPickup); // Cade
                var clips = speedPickup.pickupSound;
                var clip = clips[Random.Range(0, clips.Length)];
                AudioSource.PlayClipAtPoint(clip, transform.position, speedPickup.pickupSoundVol);
                playerInventorySystem playerInventory = other.GetComponent<playerInventorySystem>();//Tenia
                if (playerInventory != null)
                {
                    quantity = playerInventory.PickupItem(speedPickup, quantity);
                    if (quantity <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
                Destroy(gameObject);
            }

            if (jumpPickup != null)
            {
                pickupReceiver.getJumpItemStats(jumpPickup); // Cade
                playerInventorySystem playerInventory = other.GetComponent<playerInventorySystem>(); //Tenia
                if (playerInventory != null)
                {
                    quantity = playerInventory.PickupItem(jumpPickup, quantity);
                    if (quantity <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
                Destroy(gameObject);
            }
        }

        if (invinciblePickup != null)
        {
            pickupReceiver.getInvincibleStats(invinciblePickup);
            playerInventorySystem playerInventory = other.GetComponent<playerInventorySystem>();
            if (playerInventory != null)
            {
                quantity = playerInventory.PickupItem(jumpPickup, quantity);
                if (quantity <= 0)
                {
                    Destroy(gameObject);
                }
            }
            Destroy(gameObject);
        }

    }
}
