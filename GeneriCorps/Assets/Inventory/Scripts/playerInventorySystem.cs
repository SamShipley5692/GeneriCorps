using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Holistic3D.Inventory 
{
    public class playerInventorySystem : MonoBehaviour
    {
        public InventorySystem inventorySystem;
        public InputAction dropAction;
        public weaponStats weaponDrop;
        public jumpItems jumpItemDrop;
        public healthItems healthItemDrop;
        public speedItems speedItemDrop;
        public InvincibleItems invincibleItemsDrop;

        private void Start()
        {
            dropAction = InputSystem.actions.FindAction("Drop");
        }

        private void Update()
        {
            if (dropAction.WasPressedThisFrame())
            {
                DropItem(weaponDrop, 1);
                DropItem(jumpItemDrop, 1);
                DropItem(healthItemDrop, 1);
                DropItem(speedItemDrop, 1);
                DropItem(invincibleItemsDrop, 1);
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

        public int PickupItem(InvincibleItems pickup, int q)
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
            int couldntBeDropped = inventorySystem.RemoveItem(weapon, quantity);
            int numberDropped = quantity - couldntBeDropped;

            if(numberDropped > 0)
            {
                Instantiate(weapon.model, GetDropPosition(), Quaternion.identity);
            }
            
        }

      public void DropItem(jumpItems pickup, int quantity)
        {
            int couldntBeDropped = inventorySystem.RemoveItem(pickup, quantity);
            int numberDropped = quantity - couldntBeDropped;

            if (numberDropped > 0)
            {
                Instantiate(pickup.itemModel, GetDropPosition(), Quaternion.identity);
            }
        }

        public void DropItem(speedItems pickup, int quantity)
        {
            int couldntBeDropped = inventorySystem.RemoveItem(pickup, quantity);
            int numberDropped = quantity - couldntBeDropped;

            if (numberDropped > 0)
            {
                Instantiate(pickup.itemModel, GetDropPosition(), Quaternion.identity);
            }
        }

        public void DropItem(healthItems pickup, int quantity)
        {
            int couldntBeDropped = inventorySystem.RemoveItem(pickup, quantity);
            int numberDropped = quantity - couldntBeDropped;

            if (numberDropped > 0)
            {
                Instantiate(pickup.itemModel, GetDropPosition(), Quaternion.identity);
            }
        }

        public void DropItem(InvincibleItems pickup, int quantity)
        {
            int couldntBeDropped = inventorySystem.RemoveItem(pickup, quantity);
            int numberDropped = quantity - couldntBeDropped;

            if (numberDropped > 0)
            {
                Instantiate(pickup.itemModel, GetDropPosition(), Quaternion.identity);
            }
        }

        private Vector3 GetDropPosition()
        {
            Vector3 playerPosition = transform.position;
            Vector3 forwardDirection = transform.forward;
            
            float dropDistance = 4f;
            return playerPosition + forwardDirection * dropDistance;
        }
    }
}
