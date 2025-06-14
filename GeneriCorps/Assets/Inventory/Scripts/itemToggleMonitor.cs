using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Holistic3D.Inventory
{
    public class itemToggleMonitor : MonoBehaviour
    {
        public ToggleGroup toggleGroup;

        private Toggle lastSelectedToggle;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
            {
                toggle.onValueChanged.AddListener(isOn =>
                {
                    if(isOn)
                    {
                        HandleToggleChanged(toggle);
                    }
                });
                if (toggle.isOn) InventoryPanelManager.instance.FilterItemsByType(toggle.GetComponent<ToggleItemType>().toggleItemType);
            }
        }

        private void HandleToggleChanged(Toggle selectToggle)
        {
            if(selectToggle != lastSelectedToggle)
            {
                lastSelectedToggle = selectToggle;
                ItemType selectedType = selectToggle.GetComponent<ToggleItemType>().toggleItemType;
                InventoryPanelManager.instance.FilterItemsByType(selectedType);
            }
        }
    }
}


