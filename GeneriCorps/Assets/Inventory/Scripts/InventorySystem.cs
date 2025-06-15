using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;


namespace Holistic3D.Inventory 
{
    public class InventorySystem : MonoBehaviour
    {
        public List<inventorySlot> slots = new List<inventorySlot>();
        public int maxSlots = 20;
        public playerInventorySystem playerInventorySystem;

        public int AddItem(weaponStats weapon, int q)
        {
            if(q <= 0)
            {
                return 0;
            }

            int remainingItems = q;

            if(weapon.isStackable)
            {
                foreach (inventorySlot slot in slots)
                {
                    if(slot._weaponStats == weapon)
                    {
                        int spaceInStack = weapon.maxStackSize - slot.Quantity;
                        if (spaceInStack > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.Quantity += itemsToAdd;
                            remainingItems -= itemsToAdd;

                            if(remainingItems <= 0)
                            {
                                return 0;
                            }
                        }
                    }
                }
            }

            while(remainingItems > 0 && slots.Count < maxSlots)
            {
                int itemsToAdd = Mathf.Min(remainingItems, weapon.isStackable ? weapon.maxStackSize : 1);
                slots.Add(new inventorySlot(weapon, itemsToAdd));

                remainingItems -= itemsToAdd;
            }
            return remainingItems;
        }

        public int AddItem(jumpItems pickup, int q)
        {
            if (q <= 0)
            {
                return 0;
            }

            int remainingItems = q;

            if (pickup.isStackable)
            {
                foreach (inventorySlot slot in slots)
                {
                    if (slot._jumpItems == pickup)
                    {
                        int spaceInStack = pickup.maxStackSize - slot.Quantity;
                        if (spaceInStack > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.Quantity += itemsToAdd;
                            remainingItems -= itemsToAdd;

                            if (remainingItems <= 0)
                            {
                                return 0;
                            }
                        }
                    }
                }
            }

            while (remainingItems > 0 && slots.Count < maxSlots)
            {
                int itemsToAdd = Mathf.Min(remainingItems, pickup.isStackable ? pickup.maxStackSize : 1);
                slots.Add(new inventorySlot(pickup, itemsToAdd));

                remainingItems -= itemsToAdd;
            }
            return remainingItems;
        }
        public int AddItem(healthItems pickup, int q)
        {
            if (q <= 0)
            {
                return 0;
            }

            int remainingItems = q;

            if (pickup.isStackable)
            {
                foreach (inventorySlot slot in slots)
                {
                    if (slot._healthItems == pickup)
                    {
                        int spaceInStack = pickup.maxStackSize - slot.Quantity;
                        if (spaceInStack > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.Quantity += itemsToAdd;
                            remainingItems -= itemsToAdd;

                            if (remainingItems <= 0)
                            {
                                return 0;
                            }
                        }
                    }
                }
            }

            while (remainingItems > 0 && slots.Count < maxSlots)
            {
                int itemsToAdd = Mathf.Min(remainingItems, pickup.isStackable ? pickup.maxStackSize : 1);
                slots.Add(new inventorySlot(pickup, itemsToAdd));

                remainingItems -= itemsToAdd;
            }
            return remainingItems;
        }
        public int AddItem(speedItems pickup, int q)
        {
            if (q <= 0)
            {
                return 0;
            }

            int remainingItems = q;

            if (pickup.isStackable)
            {
                foreach (inventorySlot slot in slots)
                {
                    if (slot._speedItems == pickup)
                    {
                        int spaceInStack = pickup.maxStackSize - slot.Quantity;
                        if (spaceInStack > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.Quantity += itemsToAdd;
                            remainingItems -= itemsToAdd;

                            if (remainingItems <= 0)
                            {
                                return 0;
                            }
                        }
                    }
                }
            }

            while (remainingItems > 0 && slots.Count < maxSlots)
            {
                int itemsToAdd = Mathf.Min(remainingItems, pickup.isStackable ? pickup.maxStackSize : 1);
                slots.Add(new inventorySlot(pickup, itemsToAdd));

                remainingItems -= itemsToAdd;
            }
            return remainingItems;
        }

        public int AddItem(InvincibleItems pickup, int q)
        {
            if (q <= 0)
            {
                return 0;
            }

            int remainingItems = q;

            if (pickup.isStackable)
            {
                foreach (inventorySlot slot in slots)
                {
                    if (slot._invincibleItems == pickup)
                    {
                        int spaceInStack = pickup.maxStackSize - slot.Quantity;
                        if (spaceInStack > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.Quantity += itemsToAdd;
                            remainingItems -= itemsToAdd;

                            if (remainingItems <= 0)
                            {
                                return 0;
                            }
                        }
                    }
                }
            }

            while (remainingItems > 0 && slots.Count < maxSlots)
            {
                int itemsToAdd = Mathf.Min(remainingItems, pickup.isStackable ? pickup.maxStackSize : 1);
                slots.Add(new inventorySlot(pickup, itemsToAdd));

                remainingItems -= itemsToAdd;
            }
            return remainingItems;
        }

        public int RemoveItem(weaponStats weapon, int quantity, bool removePartial = true)
        {
            int remainingItems = quantity;
            List<inventorySlot> slotsWithItem = slots.Where(s => s._weaponStats == weapon).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.Quantity);
            if(remainingItems > totalAvailableItems && !removePartial)
            {
                return quantity;
            }

            foreach(var slot in slotsWithItem)
            {
                if(remainingItems <= 0)
                {
                    break;
                }

                if(slot.Quantity < remainingItems)
                {
                    remainingItems -= slot.Quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else
                {
                    slot.Quantity -= remainingItems;
                    remainingItems = 0;
                }
            }
            return remainingItems;
        }

        public int RemoveItem(jumpItems pickup, int quantity, bool removePartial = true)
        {
            int remainingitems = quantity;
            List<inventorySlot> slotsWithItem = slots.Where(s => s._jumpItems == pickup).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.Quantity);
            if (remainingitems > totalAvailableItems && !removePartial)
            {
                return quantity;
            }

            foreach (var slot in slotsWithItem)
            {
                if (remainingitems <= 0)
                {
                    break;
                }

                if (slot.Quantity < remainingitems)
                {
                    remainingitems -= slot.Quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else
                {
                    slot.Quantity -= remainingitems;
                    remainingitems = 0;
                }
            }
            return remainingitems;
        }

        public int RemoveItem(speedItems pickup, int quantity, bool removePartial = true)
        {
            int remainingitems = quantity;
            List<inventorySlot> slotsWithItem = slots.Where(s => s._speedItems == pickup).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.Quantity);
            if (remainingitems > totalAvailableItems && !removePartial)
            {
                return quantity;
            }

            foreach (var slot in slotsWithItem)
            {
                if (remainingitems <= 0)
                {
                    break;
                }

                if (slot.Quantity < remainingitems)
                {
                    remainingitems -= slot.Quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else
                {
                    slot.Quantity -= remainingitems;
                    remainingitems = 0;
                }
            }
            return remainingitems;
        }

        public int RemoveItem(healthItems pickup, int quantity, bool removePartial = true)
        {
            int remainingitems = quantity;
            List<inventorySlot> slotsWithItem = slots.Where(s => s._healthItems == pickup).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.Quantity);
            if (remainingitems > totalAvailableItems && !removePartial)
            {
                return quantity;
            }

            foreach (var slot in slotsWithItem)
            {
                if (remainingitems <= 0)
                {
                    break;
                }

                if (slot.Quantity < remainingitems)
                {
                    remainingitems -= slot.Quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else
                {
                    slot.Quantity -= remainingitems;
                    remainingitems = 0;
                }
            }
            return remainingitems;
        }

        public int RemoveItem(InvincibleItems pickup, int quantity, bool removePartial = true)
        {
            int remainingitems = quantity;
            List<inventorySlot> slotsWithItem = slots.Where(s => s._invincibleItems == pickup).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.Quantity);
            if (remainingitems > totalAvailableItems && !removePartial)
            {
                return quantity;
            }

            foreach (var slot in slotsWithItem)
            {
                if (remainingitems <= 0)
                {
                    break;
                }

                if (slot.Quantity < remainingitems)
                {
                    remainingitems -= slot.Quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else
                {
                    slot.Quantity -= remainingitems;
                    remainingitems = 0;
                }
            }
            return remainingitems;
        }

        public int RemoveItemFromSlot(inventorySlot slot, int quantity)
        {
            if(slot.Quantity >= quantity)
            {
                slot.Quantity -= quantity;
                if (slot.Quantity == 0)
                {
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                DropItem(slot._weaponStats, quantity);
                return 0;
            }
            else
            {
                int remainingQuantity = quantity - slot.Quantity;
                DropItem(slot._weaponStats, slot.Quantity);
                slot.ClearSlot();
                slots.Remove(slot);
                return remainingQuantity;
            }
        }

        public void DropItem(weaponStats weapon, int numberDropped)
        {
            if(numberDropped > 0)
            {
                if(weapon != null)
                Instantiate(weapon, playerInventorySystem.GetDropPosition(), Quaternion.identity).GetComponent<pickup>().quantity = numberDropped;
            }
        }

        public void DropItem(jumpItems pickup, int numberDropped)
        {
            if (numberDropped > 0)
            {
                
                Instantiate(pickup, playerInventorySystem.GetDropPosition(), Quaternion.identity).GetComponent<pickup>().quantity = numberDropped;
            }
        }

        public void DropItem(speedItems pickup, int numberDropped)
        {
            if (numberDropped > 0)
            {
                
                Instantiate(pickup, playerInventorySystem.GetDropPosition(), Quaternion.identity).GetComponent<pickup>().quantity = numberDropped;
            }
        }

        public void DropItem(healthItems pickup, int numberDropped)
        {
            if (numberDropped > 0)
            {
               
                Instantiate(pickup, playerInventorySystem.GetDropPosition(), Quaternion.identity).GetComponent<pickup>().quantity = numberDropped;
            }
        }

        public void DropItem(InvincibleItems pickup, int numberDropped)
        {
            if (numberDropped > 0)
            {
                
                Instantiate(pickup, playerInventorySystem.GetDropPosition(), Quaternion.identity).GetComponent<pickup>().quantity = numberDropped;
            }
        }


        public void RemoveItemsFromSlot(int slotNumber)
        {
            slots.RemoveAt(slotNumber);
        }

        public bool IsFull()
        {
            return slots.Count >= maxSlots;
        }

        public void ClearInventory()
        {
            slots.Clear();
        }
    }

}
