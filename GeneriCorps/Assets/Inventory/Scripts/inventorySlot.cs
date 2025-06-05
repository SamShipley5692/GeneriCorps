using UnityEngine;

namespace Holistic3D.Inventory
{
    [System.Serializable]
    public class inventorySlot
    {
        public speedItems speedItems;
        public weaponStats weaponStats;
        public jumpItems jumpItems;
        public healthItems healthItems;
        
        public int quantity;
        

        public inventorySlot(weaponStats weapon, int q)
        {
            weaponStats = weapon;
            quantity = q;
        }

        public inventorySlot(healthItems pickup, int q)
        {
            healthItems = pickup;
            quantity = q;
        }

        public inventorySlot(speedItems pickup, int q)
        {
            speedItems = pickup;
            quantity = q;
        }

        public inventorySlot(jumpItems pickup, int q)
        {
           jumpItems = pickup;
            quantity = q;
        }
        public bool IsEmpty()
        {
            return speedItems == null && weaponStats == null && jumpItems == null && healthItems == null || quantity <= 0;
        }

        public void ClearSlot()
        {
            speedItems = null;
            weaponStats = null;
            jumpItems = null;
            healthItems = null;
            quantity = 0;
        }

        public void SetItem(speedItems newItem, int newQuantity)
        {
            speedItems = newItem;
            quantity = newQuantity;
        }

        public void SetItem(weaponStats newItem, int newQuantity)
        {
            weaponStats = newItem;
            quantity = newQuantity;
        }

        public void SetItem(jumpItems newItem, int newQuantity)
        {
            jumpItems = newItem;
            quantity= newQuantity;
        }

        public void SetItem(healthItems newItem, int newQuantity)
        {
            healthItems = newItem;
            quantity = newQuantity;
        }
    }
}

