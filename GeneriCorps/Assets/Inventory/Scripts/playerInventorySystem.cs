using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Holistic3D.Inventory 
{
    public class playerInventorySystem : MonoBehaviour
    {
        public InventorySystem inventorySystem;
        public InputAction dropAction;
        public InputAction inventoryAction;
        public weaponStats weaponDrop;
        public jumpItems jumpItemDrop;
        public healthItems healthItemDrop;
        public speedItems speedItemDrop;
        public InvincibleItems invincibleItemsDrop;
        [SerializeField] InventoryPanelManager inventoryPanelManager;

        private void Start()
        {
            dropAction = InputSystem.actions.FindAction("Drop");
            inventoryAction = InputSystem.actions.FindAction("Inventory");
            inventoryPanelManager.SetPanelVisibility(false);
            inventorySystem.playerInventorySystem = this;
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
            else if(inventoryAction.WasPressedThisFrame())
            {
                OpenCloseInventory();
            }

        }

        public void OpenCloseInventory()
        {
            inventoryPanelManager.TogglePanelVisbility();
            if (inventoryPanelManager.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if(!inventoryPanelManager.gameObject.activeSelf)
            {
                Cursor.lockState= CursorLockMode.Locked;
                Cursor.visible = false;
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

        public Vector3 GetDropPosition()
        {
            Vector3 playerPosition = transform.position;
            Vector3 forwardDirection = transform.forward;
            
            float dropDistance = 4f;
            return playerPosition + forwardDirection * dropDistance;
        }
    }
}
