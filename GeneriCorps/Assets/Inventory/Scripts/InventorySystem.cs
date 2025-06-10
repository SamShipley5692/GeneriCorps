using UnityEngine;
using System.Collections.Generic;


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

        public void RemoveItem(weaponStats weapon, int quantity)
        {
            inventorySlot slot = slots.Find(s  => s._weaponStats == weapon);
            if (slot != null)
            {
                if(slot.quantity >= quantity)
                {
                    slot.quantity -= quantity;

                    if(slot.quantity <= 0)
                    {
                        slot.ClearSlot();
                        slots.Remove(slot);
                    }
                }
            }
        }

        public void RemoveItem(jumpItems pickup, int quantity)
        {
            inventorySlot slot = slots.Find(s => s._jumpItems == pickup);
            if (slot != null)
            {
                if (slot.quantity >= quantity)
                {
                    slot.quantity -= quantity;

                    if (slot.quantity <= 0)
                    {
                        slot.ClearSlot();
                        slots.Remove(slot);
                    }
                }
            }
        }

        public void RemoveItem(speedItems pickup, int quantity)
        {
            inventorySlot slot = slots.Find(s => s._speedItems == pickup);
            if (slot != null)
            {
                if (slot.quantity >= quantity)
                {
                    slot.quantity -= quantity;

                    if (slot.quantity <= 0)
                    {
                        slot.ClearSlot();
                        slots.Remove(slot);
                    }
                }
            }
        }

        public void RemoveItem(healthItems pickup, int quantity)
        {
            inventorySlot slot = slots.Find(s => s._healthItems == pickup);
            if (slot != null)
            {
                if (slot.quantity >= quantity)
                {
                    slot.quantity -= quantity;

                    if (slot.quantity <= 0)
                    {
                        slot.ClearSlot();
                        slots.Remove(slot);
                    }
                }
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
