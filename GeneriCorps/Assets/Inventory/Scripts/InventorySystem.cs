using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace Holistic3D.Inventory 
{
    public class InventorySystem : MonoBehaviour
    {
        public List<inventorySlot> slots = new List<inventorySlot>();
        public int maxSlots = 20;

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
                        int spaceInStack = weapon.maxStackSize - slot.quantity;
                        if (spaceInStack > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.quantity += itemsToAdd;
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
                        int spaceInStack = pickup.maxStackSize - slot.quantity;
                        if (spaceInStack > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.quantity += itemsToAdd;
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
                        int spaceInStack = pickup.maxStackSize - slot.quantity;
                        if (spaceInStack > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.quantity += itemsToAdd;
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
                        int spaceInStack = pickup.maxStackSize - slot.quantity;
                        if (spaceInStack > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.quantity += itemsToAdd;
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
                        int spaceInStack = pickup.maxStackSize - slot.quantity;
                        if (spaceInStack > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.quantity += itemsToAdd;
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

            int totalAvailableItems = slotsWithItem.Sum(s => s.quantity);
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

                if(slot.quantity < remainingItems)
                {
                    remainingItems -= slot.quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else
                {
                    slot.quantity -= remainingItems;
                    remainingItems = 0;
                }
            }
            return remainingItems;
        }

        public int RemoveItem(jumpItems pickup, int quantity, bool removePartial = true)
        {
            int remainingitems = quantity;
            List<inventorySlot> slotsWithItem = slots.Where(s => s._jumpItems == pickup).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.quantity);
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

                if (slot.quantity < remainingitems)
                {
                    remainingitems -= slot.quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else
                {
                    slot.quantity -= remainingitems;
                    remainingitems = 0;
                }
            }
            return remainingitems;
        }

        public int RemoveItem(speedItems pickup, int quantity, bool removePartial = true)
        {
            int remainingitems = quantity;
            List<inventorySlot> slotsWithItem = slots.Where(s => s._speedItems == pickup).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.quantity);
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

                if (slot.quantity < remainingitems)
                {
                    remainingitems -= slot.quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else
                {
                    slot.quantity -= remainingitems;
                    remainingitems = 0;
                }
            }
            return remainingitems;
        }

        public int RemoveItem(healthItems pickup, int quantity, bool removePartial = true)
        {
            int remainingitems = quantity;
            List<inventorySlot> slotsWithItem = slots.Where(s => s._healthItems == pickup).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.quantity);
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

                if (slot.quantity < remainingitems)
                {
                    remainingitems -= slot.quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else
                {
                    slot.quantity -= remainingitems;
                    remainingitems = 0;
                }
            }
            return remainingitems;
        }

        public int RemoveItem(InvincibleItems pickup, int quantity, bool removePartial = true)
        {
            int remainingitems = quantity;
            List<inventorySlot> slotsWithItem = slots.Where(s => s._invincibleItems == pickup).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.quantity);
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

                if (slot.quantity < remainingitems)
                {
                    remainingitems -= slot.quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else
                {
                    slot.quantity -= remainingitems;
                    remainingitems = 0;
                }
            }
            return remainingitems;
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
