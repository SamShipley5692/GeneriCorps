using UnityEngine;

namespace Holistic3D.Inventory 
{
    public class playerInventorySystem : MonoBehaviour
    {
        public InventorySystem inventorySystem;

        public int PickupItem(weaponStats weapon, int q)
        {
            Debug.Log("Picked up" + weapon.name + " " + q);
            return 0;
        }

        public int PickupItem(healthItems pickup, int q)
        {
            Debug.Log("Picked up" + pickup.name + " " + q);
            return 0;
        }

        public int PickupItem(speedItems pickup, int q)
        {
            Debug.Log("Picked up" + pickup.name + " " + q);
            return 0;
        }

        public int PickupItem(jumpItems pickup, int q)
        {
            Debug.Log("Picked up" + pickup.name + " " + q);
            return 0;
        }
    }
}
