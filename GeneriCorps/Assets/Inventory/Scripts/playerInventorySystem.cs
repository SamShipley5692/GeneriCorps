using UnityEngine;
using UnityEngine.InputSystem;

namespace Holistic3D.Inventory 
{
    public class playerInventorySystem : MonoBehaviour
    {
        public InventorySystem inventorySystem;
        public InputAction dropAction;
        public weaponStats weaponDrop; //for testing

        private void Start()
        {
            dropAction = InputSystem.actions.FindAction("Drop");
        }

        private void Update()
        {
            if (dropAction.WasPressedThisFrame())
            {
                DropItem(weaponDrop, 1);
            }
        }

        public int PickupItem(weaponStats weapon, int q)
        {
            if (!inventorySystem.IsFull() || weapon.isStackable)
            {
                return inventorySystem.AddItem(weapon, q);
            }
            return q;
        }

        public int PickupItem(healthItems pickup, int q)
        {
            if(!inventorySystem.IsFull() || pickup.isStackable)
            {
                return inventorySystem.AddItem(pickup, q);
            }
            return q;
        }

        public int PickupItem(speedItems pickup, int q)
        {
            if (!inventorySystem.IsFull() || pickup.isStackable)
            {
                return inventorySystem.AddItem(pickup, q);
            }
            return q;
        }

        public int PickupItem(jumpItems pickup, int q)
        {
            if (!inventorySystem.IsFull() || pickup.isStackable)
            {
                return inventorySystem.AddItem(pickup, q);
            }
            return q;
        }

        public void DropItemsFromSlot(int slotNumber)
        {
            inventorySystem.RemoveItemsFromSlot(slotNumber);
        }

        public void DropItem(weaponStats weapon, int quantity)
        {
            inventorySystem.RemoveItem(weapon, quantity);
        }
    }
}
