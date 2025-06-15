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
        private int quantity;

        private itemButtonSettings itemButton;

        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value < 0 ? 0 : value;
                if(itemButton != null)
                {
                    itemButton.UpdateQuanityDisplay(quantity);
                }
            }
        }

        public inventorySlot(weaponStats weapon, int q)
        {
            _weaponStats = weapon;
            itemButton = InventoryPanelManager.instance.CreateInventoryButton(weapon, this);
            Quantity = q;
        }

        public inventorySlot(healthItems pickup, int q)
        {
            _healthItems = pickup;
            itemButton = InventoryPanelManager.instance.CreateInventoryButton(pickup, this);
            Quantity = q;
        }

        public inventorySlot(speedItems pickup, int q)
        {
            _speedItems = pickup;
            itemButton = InventoryPanelManager.instance.CreateInventoryButton(pickup, this);
            Quantity = q;
        }

        public inventorySlot(jumpItems pickup, int q)
        {
           _jumpItems = pickup;
            itemButton = InventoryPanelManager.instance.CreateInventoryButton(pickup, this);
            Quantity = q;
        }

        public inventorySlot(InvincibleItems pickup, int q)
        {
            _invincibleItems = pickup;
            itemButton = InventoryPanelManager.instance.CreateInventoryButton(pickup, this);
            Quantity = q;
        }

        public bool IsEmpty()
        {
            return _speedItems == null && _weaponStats == null && _jumpItems == null && _healthItems == null && _invincibleItems == null || Quantity <= 0;
        }

        public void ClearSlot()
        {
            _speedItems = null;
            _weaponStats = null;
            _jumpItems = null;
            _healthItems = null;
            _invincibleItems = null;
            Quantity = 0;
            if(itemButton != null)
            {
               InventoryPanelManager.instance.DestroyInventoryButton(itemButton.gameObject);
            }
        }

        public void SetItem(speedItems newItem, int newQuantity)
        {
            _speedItems = newItem;
            Quantity = newQuantity;
        }

        public void SetItem(weaponStats newItem, int newQuantity)
        {
            _weaponStats = newItem;
            Quantity = newQuantity;
        }

        public void SetItem(jumpItems newItem, int newQuantity)
        {
            _jumpItems = newItem;
            Quantity = newQuantity;
        }

        public void SetItem(healthItems newItem, int newQuantity)
        {
            _healthItems = newItem;
            Quantity = newQuantity;
        }

        public void SetItem(InvincibleItems newItem, int newQuantity)
        {
            _invincibleItems = newItem;
            Quantity = newQuantity;
        }
    }
}

