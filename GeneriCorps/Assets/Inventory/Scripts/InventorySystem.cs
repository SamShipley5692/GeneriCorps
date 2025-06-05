using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Holistic3D.Inventory 
{
    public class InventorySystem : MonoBehaviour
    {
        public List<inventorySlot> slots = new List<inventorySlot>();
        public int maxSlots = 20;





        public int AddItem(weaponStats weapon, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                return 0;
            }

            int remainingItems = newQuantity;

            if (weapon.isStackable)
            {
                foreach (inventorySlot slot in slots)
                {
                    if (slot.weaponStats = weapon)
                    {
                        int spaceInSlot = weapon.maxStackSize - slot.quantity;
                        if(spaceInSlot > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInSlot);
                            slot.quantity += itemsToAdd;
                            remainingItems -= itemsToAdd;

                            if(remainingItems <= 0)
                            {
                                return 0;
                            }
                        }
                    }
            }   }

            while (remainingItems > 0 && slots.Count < maxSlots)
            {
                int itemsToAdd = Mathf.Min(remainingItems, weapon.isStackable ? weapon.maxStackSize : 1);
                slots.Add(new inventorySlot(weapon, itemsToAdd));
                remainingItems -= itemsToAdd;
            }

            return remainingItems;
        }

        public int AddItem(speedItems pickup, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                return 0;
            }

            int remainingItems = newQuantity;

            if (pickup.isStackable)
            {
                foreach (inventorySlot slot in slots)
                {
                    if (slot.speedItems = pickup)
                    {
                        int spaceInSlot = pickup.maxStackSize - slot.quantity;
                        if (spaceInSlot > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInSlot);
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

        public int AddItem(jumpItems pickup, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                return 0;
            }

            int remainingItems = newQuantity;

            if (pickup.isStackable)
            {
                foreach (inventorySlot slot in slots)
                {
                    if (slot.jumpItems = pickup)
                    {
                        int spaceInSlot = pickup.maxStackSize - slot.quantity;
                        if (spaceInSlot > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInSlot);
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

        public int AddItem(healthItems pickup, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                return 0;
            }

            int remainingItems = newQuantity;

            if (pickup.isStackable)
            {
                foreach (inventorySlot slot in slots)
                {
                    if (slot.healthItems = pickup)
                    {
                        int spaceInSlot = pickup.maxStackSize - slot.quantity;
                        if (spaceInSlot > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInSlot);
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

        public void RemoveItem(weaponStats weapon, int q)
        {
           inventorySlot slot = slots.Find(s => s.weaponStats =  weapon);
            if(slot != null)
            {
                if(slot.quantity >= q)
                {
                    slot.quantity -= q;

                    if(slot.quantity <= 0)
                    {
                        slot.ClearSlot();
                        slots.Remove(slot);
                    }
                }
            }
        }

        public void RemoveItem(speedItems pickup, int q)
        {
            inventorySlot slot = slots.Find(s => s.speedItems = pickup);
            if (slot != null)
            {
                if (slot.quantity >= q)
                {
                    slot.quantity -= q;

                    if (slot.quantity <= 0)
                    {
                        slot.ClearSlot();
                        slots.Remove(slot);
                    }
                }
            }
        }

        public void RemoveItem(healthItems pickup, int q) 
        {
            inventorySlot slot = slots.Find(s => s.healthItems = pickup);
            if (slot != null)
            {
                if (slot.quantity >= q)
                {
                    slot.quantity -= q;

                    if (slot.quantity <= 0)
                    {
                        slot.ClearSlot();
                        slots.Remove(slot);
                    }
                }
            }
        }

        public void RemoveItem(jumpItems pickup, int q)
        {
            inventorySlot slot = slots.Find(s => s.jumpItems = pickup);
            if (slot != null)
            {
                if (slot.quantity >= q)
                {
                    slot.quantity -= q;

                    if (slot.quantity <= 0)
                    {
                        slot.ClearSlot();
                        slots.Remove(slot);
                    }
                }
            }
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
