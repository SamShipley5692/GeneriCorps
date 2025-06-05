using UnityEngine;
using Holistic3D.Inventory;

public class inventoryManager : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public weaponStats weapon;
    public jumpItems _jumpItems;
    public speedItems _speedItems;
    public healthItems _healthItems;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            inventorySystem.AddItem(weapon, 1);
            
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            inventorySystem.AddItem(_jumpItems, 1);

        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            inventorySystem.RemoveItem(weapon,1);
        }
    }
}
