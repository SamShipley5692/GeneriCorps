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
        [SerializeField] private Transform weaponHolder;

        [SerializeField] private playercontroller playerController;

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
                if (this.HasItem(weaponDrop))
                {
                    DropItem(weaponDrop, 1);
                }

                if (this.HasItem(jumpItemDrop))
                {
                    DropItem(jumpItemDrop, 1);
                }

                if (this.HasItem(speedItemDrop))
                {
                    DropItem(speedItemDrop, 1);
                }

                if (this.HasItem(healthItemDrop))
                {
                    DropItem(healthItemDrop, 1);
                }

                if (this.HasItem(invincibleItemsDrop))
                {
                    DropItem(invincibleItemsDrop, 1);
                }
            }
            else if(inventoryAction.WasPressedThisFrame())
            {
                OpenCloseInventory();
            }

        }

        public void OpenCloseInventory()
        {

            if(gameManager.instance != null && gameManager.instance.isPaused)
            {
                return;
            }

            bool newState = !inventoryPanelManager.IsPanelVisible();
            inventoryPanelManager.SetPanelVisibility(newState);
            if (newState)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else 
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

            if(numberDropped > 0)
            {
                Instantiate(weapon.model, GetDropPosition(), Quaternion.identity);

                weaponStats currentlyEquipped = playerController.GetEquippedWeapon();

                if(currentlyEquipped != null && currentlyEquipped == weapon)
                {
                    playerController.RemoveEquippedWeapon();
                }
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

        public bool HasItem(weaponStats weapon)
        {
            foreach (inventorySlot slot in inventorySystem.slots)
            {
                if (slot._weaponStats == weapon && slot.Quantity > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasItem(jumpItems item)
        {
            foreach (inventorySlot slot in inventorySystem.slots)
            {
                if (slot._jumpItems == item && slot.Quantity > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasItem(speedItems item)
        {
            foreach (inventorySlot slot in inventorySystem.slots)
            {
                if (slot._speedItems == item && slot.Quantity > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasItem(healthItems item)
        {
            foreach (inventorySlot slot in inventorySystem.slots)
            {
                if (slot._healthItems == item && slot.Quantity > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasItem(InvincibleItems item)
        {
            foreach (inventorySlot slot in inventorySystem.slots)
            {
                if (slot._invincibleItems == item && slot.Quantity > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
