using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Holistic3D.Inventory 
{
    public class itemButtonSettings : MonoBehaviour
    {
        [SerializeField] private Image spriteImage;
        [SerializeField] private TMPro.TextMeshProUGUI numberInSlot;
        [SerializeField] private ItemType _itemType;

        public void Init(weaponStats weapon, int itemCount)
        {
            spriteImage.sprite = weapon.icon;
            numberInSlot.text = itemCount.ToString();
            _itemType = weapon.itemType;
        }

        public void Init(jumpItems pickup, int itemCount)
        {
            spriteImage.sprite = pickup.icon;
            numberInSlot.text = itemCount.ToString();
            _itemType = pickup.itemType;
        }

        public void Init(speedItems pickup, int itemCount)
        {
            spriteImage.sprite = pickup.icon;
            numberInSlot.text = itemCount.ToString();
            _itemType = pickup.itemType;
        }

        public void Init(healthItems pickup, int itemCount)
        {
            spriteImage.sprite = pickup.icon;
            numberInSlot.text = itemCount.ToString();
            _itemType = pickup.itemType;
        }

        public void Init(InvincibleItems pickup, int itemCount)
        {
            spriteImage.sprite = pickup.icon;
            numberInSlot.text = itemCount.ToString();
            _itemType = pickup.itemType;
        }
    }
}


