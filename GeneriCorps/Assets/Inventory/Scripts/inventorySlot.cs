using UnityEngine;

namespace Holistic3D.Inventory
{
    [System.Serializable]
    public class inventorySlot
    {
        public speedItems _speedItems;
        public weaponStats _weaponStats;
        public jumpItems _jumpItems;
        public healthItems _healthItems;
        public InvincibleItems _invincibleItems;
        
        public int quantity;
        

        public inventorySlot(weaponStats weapon, int q)
        {
            _weaponStats = weapon;
            quantity = q;
        }

        public inventorySlot(healthItems pickup, int q)
        {
            _healthItems = pickup;
            quantity = q;
        }

        public inventorySlot(speedItems pickup, int q)
        {
            _speedItems = pickup;
            quantity = q;
        }

        public inventorySlot(jumpItems pickup, int q)
        {
           _jumpItems = pickup;
            quantity = q;
        }

        public inventorySlot(InvincibleItems pickup, int q)
        {
            _invincibleItems = pickup;
            quantity = q;
        }

        public bool IsEmpty()
        {
            return _speedItems == null && _weaponStats == null && _jumpItems == null && _healthItems == null && _invincibleItems == null || quantity <= 0;
        }

        public void ClearSlot()
        {
            _speedItems = null;
            _weaponStats = null;
            _jumpItems = null;
            _healthItems = null;
            _invincibleItems = null;
            quantity = 0;
        }

        public void SetItem(speedItems newItem, int newQuantity)
        {
            _speedItems = newItem;
            quantity = newQuantity;
        }

        public void SetItem(weaponStats newItem, int newQuantity)
        {
            _weaponStats = newItem;
            quantity = newQuantity;
        }

        public void SetItem(jumpItems newItem, int newQuantity)
        {
            _jumpItems = newItem;
            quantity= newQuantity;
        }

        public void SetItem(healthItems newItem, int newQuantity)
        {
            _healthItems = newItem;
            quantity = newQuantity;
        }

        public void SetItem(InvincibleItems newItem, int newQuantity)
        {
            _invincibleItems = newItem;
            quantity = newQuantity;
        }
    }
}

