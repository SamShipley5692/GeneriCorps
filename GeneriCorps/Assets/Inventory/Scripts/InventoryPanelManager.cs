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
        [SerializeField] private Button dropButton;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private List<weaponStats> allWeapons;
        [SerializeField] private jumpItems jumpPickups;
        [SerializeField] private speedItems speedPickups;
        [SerializeField] private healthItems healthPickups;
        [SerializeField] private InvincibleItems invinciblePickups;

        private bool isPanelVisible;
        private Dictionary<GameObject, ItemType> inventoryItemMap = new Dictionary<GameObject, ItemType>();
        private Dictionary<string, GameObject> previewItemObjects;

        public static InventoryPanelManager instance { get; private set; }

        private ItemType activeItemTypeTab;
        

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

        public void SetPanelVisibility(bool isVisible)
        {
            isPanelVisible = isVisible;

            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            canvasGroup.alpha = isVisible ? 1 : 0;
            canvasGroup.interactable = isVisible;
            canvasGroup.blocksRaycasts = isVisible;
        }

        public void TogglePanelVisbility()
        {
            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            if (canvasGroup.alpha == 1)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public itemButtonSettings CreateInventoryButton(weaponStats weapon, inventorySlot slot)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            itemButtonSettings _itemButtonSettings = itemButton.GetComponent<itemButtonSettings>();
            _itemButtonSettings.Init(weapon, 1);
            inventoryItemMap.Add(itemButton, weapon.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(weapon.name, slot));

            if (weapon.itemType != activeItemTypeTab)
            {
                itemButton.gameObject.SetActive(false);
            }
            return _itemButtonSettings;
        }

        public itemButtonSettings CreateInventoryButton(jumpItems pickup, inventorySlot slot)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            itemButtonSettings _itemButtonSettings = itemButton.GetComponent<itemButtonSettings>();
            _itemButtonSettings.Init(pickup, 1);
            inventoryItemMap.Add(itemButton, pickup.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(pickup.name, slot));

            if (pickup.itemType != activeItemTypeTab)
            {
                itemButton.gameObject.SetActive(false);
            }
            return _itemButtonSettings;
        }

        public itemButtonSettings CreateInventoryButton(speedItems pickup, inventorySlot slot)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            itemButtonSettings _itemButtonSettings = itemButton.GetComponent<itemButtonSettings>();
            _itemButtonSettings.Init(pickup, 1);
            inventoryItemMap.Add(itemButton, pickup.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(pickup.name, slot));

            if (pickup.itemType != activeItemTypeTab)
            {
                itemButton.gameObject.SetActive(false);
            }
            return _itemButtonSettings;
        }

        public itemButtonSettings CreateInventoryButton(healthItems pickup, inventorySlot slot)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            itemButtonSettings _itemButtonSettings = itemButton.GetComponent<itemButtonSettings>();
            _itemButtonSettings.Init(pickup, 1);
            inventoryItemMap.Add(itemButton, pickup.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(pickup.name, slot));

            if (pickup.itemType != activeItemTypeTab)
            {
                itemButton.gameObject.SetActive(false);
            }
            return _itemButtonSettings;
        }

        public itemButtonSettings CreateInventoryButton(InvincibleItems pickup, inventorySlot slot)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            itemButtonSettings _itemButtonSettings = itemButton.GetComponent<itemButtonSettings>();
            _itemButtonSettings.Init(pickup, 1);
            inventoryItemMap.Add(itemButton, pickup.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(pickup.name, slot));

            if(pickup.itemType != activeItemTypeTab)
            {
                itemButton.gameObject.SetActive(false);
            }
            return _itemButtonSettings;
        }

        public void DestroyInventoryButton(GameObject inventoryButton)
        {
            inventoryItemMap.Remove(inventoryButton);
            Destroy(inventoryButton);
        }

        public void FilterItemsByType(ItemType type)
        {
            activeItemTypeTab = type;

            foreach (var kvp in inventoryItemMap)
            {
                GameObject itemButton = kvp.Key;
                ItemType itemType = kvp.Value;

                itemButton.gameObject.SetActive(itemType == type);
            }
        }

        public void ShowItem(string itemName, inventorySlot slot) 
        { 
            dropButton.onClick.RemoveAllListeners();
            dropButton.onClick.AddListener(() => inventorySystem.RemoveItemFromSlot(slot, 1));
            foreach(var obj in previewItemObjects.Values)
            {
                obj.SetActive(false);
            }
            if(previewItemObjects.TryGetValue(itemName, out GameObject itemToShow))
            {
                itemToShow.SetActive(true);
            }
        }

        public bool IsPanelVisible()
        {
            return isPanelVisible;
        }
    }
}


