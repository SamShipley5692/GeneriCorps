using UnityEngine;
using UnityEngine.UI;

namespace Holistic3D.Inventory
{
    public class InventoryPanelManager : MonoBehaviour
    {
        [SerializeField] private GameObject itemButtonPrefab;
        [SerializeField] private GameObject itemContainer;

        public jumpItems tempItem;

        
        void Start()
        {
            for(int i = 0; i < 20; i++)
            {
                GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
                itemButton.GetComponent<itemButtonSettings>().Init(tempItem, Random.Range(1, 11));
            }           
        }

    }
}


