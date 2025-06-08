using UnityEngine;
using Holistic3D.Inventory;

public class inventoryManager : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public weaponStats weapon;
    public jumpItems jumpItems;
    public speedItems speedItems;
    public healthItems healthItems;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            inventorySystem.AddItem(jumpItems, 1);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            inventorySystem.AddItem(weapon, 1);
        }
    }

}
