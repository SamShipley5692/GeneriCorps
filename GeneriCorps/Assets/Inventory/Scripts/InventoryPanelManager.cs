using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Holistic3D.Inventory
{
    public class InventoryPanelManager : MonoBehaviour
    {
        [SerializeField] private GameObject itemButtonPrefab;
        [SerializeField] private GameObject itemContainer;
        [SerializeField] private List<weaponStats> allWeapons;
        [SerializeField] private jumpItems jumpPickups;
        [SerializeField] private speedItems speedPickups;
        [SerializeField] private healthItems healthPickups;
        [SerializeField] private InvincibleItems invinciblePickups;


        private Dictionary<GameObject, ItemType> inventoryItemMap = new Dictionary<GameObject, ItemType>();
        private Dictionary<string, GameObject> previewItemObjects;

        public static InventoryPanelManager instance { get; private set; }

        

        private void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            previewItemObjects = new Dictionary<string, GameObject>();

            foreach (var item in allWeapons)
            {
                GameObject obj = GameObject.Find("Preview " + item.model.name);
                if (obj != null)
                {
                    previewItemObjects[item.name] = obj;
                    obj.SetActive(false);
                }
            }
        }

        void Start()
        {
            /*for (int i = 0; i < allWeapons.Count; i++)
            {
                GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
                weaponStats thisWeapon = Random.Range(0,2) == 1 ? allWeapons[0] : allWeapons[1];
                itemButton.GetComponent<itemButtonSettings>().Init(thisWeapon, Random.Range(1, 10));
                inventoryItemMap.Add(itemButton, thisWeapon.itemType);

                itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(thisWeapon.name));
            }

            for (int i = 0; i < 10; i++)
            {
                GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
                jumpItems thisItem = jumpPickups;
                itemButton.GetComponent<itemButtonSettings>().Init(thisItem, Random.Range(1, 10));
                inventoryItemMap.Add(itemButton, thisItem.itemType);

                itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(thisItem.name));

            }

            for (int i = 0; i < 10; i++)
            {
                GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
                speedItems thisItem = speedPickups;
                itemButton.GetComponent<itemButtonSettings>().Init(thisItem, Random.Range(1, 10));
                inventoryItemMap.Add(itemButton, thisItem.itemType);

                itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(thisItem.name));
            }

            for (int i = 0; i < 10; i++)
            {
                GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
                healthItems thisItem = healthPickups;
                itemButton.GetComponent<itemButtonSettings>().Init(thisItem, Random.Range(1, 10));
                inventoryItemMap.Add(itemButton, thisItem.itemType);

                itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(thisItem.name));
            }

            {
                GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
                InvincibleItems thisItem = invinciblePickups;
                itemButton.GetComponent<itemButtonSettings>().Init(thisItem, Random.Range(1, 10));
                inventoryItemMap.Add(itemButton, thisItem.itemType);

                itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(thisItem.name));
            }*/

        }

        public itemButtonSettings CreateInventoryButton(weaponStats weapon)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            itemButtonSettings _itemButtonSettings = itemButton.GetComponent<itemButtonSettings>();
            _itemButtonSettings.Init(weapon, 1);
            inventoryItemMap.Add(itemButton, weapon.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(weapon.name));
            return _itemButtonSettings;
        }

        public itemButtonSettings CreateInventoryButton(jumpItems pickup)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            itemButtonSettings _itemButtonSettings = itemButton.GetComponent<itemButtonSettings>();
            _itemButtonSettings.Init(pickup, 1);
            inventoryItemMap.Add(itemButton, pickup.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(pickup.name));
            return _itemButtonSettings;
        }

        public itemButtonSettings CreateInventoryButton(speedItems pickup)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            itemButtonSettings _itemButtonSettings = itemButton.GetComponent<itemButtonSettings>();
            _itemButtonSettings.Init(pickup, 1);
            inventoryItemMap.Add(itemButton, pickup.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(pickup.name));
            return _itemButtonSettings;
        }

        public itemButtonSettings CreateInventoryButton(healthItems pickup)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            itemButtonSettings _itemButtonSettings = itemButton.GetComponent<itemButtonSettings>();
            _itemButtonSettings.Init(pickup, 1);
            inventoryItemMap.Add(itemButton, pickup.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(pickup.name));
            return _itemButtonSettings;
        }

        public itemButtonSettings CreateInventoryButton(InvincibleItems pickup)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            itemButtonSettings _itemButtonSettings = itemButton.GetComponent<itemButtonSettings>();
            _itemButtonSettings.Init(pickup, 1);
            inventoryItemMap.Add(itemButton, pickup.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(pickup.name));
            return _itemButtonSettings;
        }

        public void FilterItemsByType(ItemType type)
        {
            foreach(var kvp in inventoryItemMap)
            {
                GameObject itemButton = kvp.Key;
                ItemType itemType = kvp.Value;

                itemButton.gameObject.SetActive(itemType == type);
            }
        }

        public void ShowItem(string itemName) 
        { 
            foreach(var obj in previewItemObjects.Values)
            {
                obj.SetActive(false);
            }
            if(previewItemObjects.TryGetValue(itemName, out GameObject itemToShow))
            {
                itemToShow.SetActive(true);
            }
        }
    }
}


