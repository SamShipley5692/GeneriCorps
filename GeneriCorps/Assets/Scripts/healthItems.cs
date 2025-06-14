using UnityEngine;

namespace Holistic3D.Inventory
{
    [CreateAssetMenu]
    public class healthItems : ScriptableObject
    {
        public GameObject itemModel;
        [Range(1, 1000)] public int healthAmount;
        public AudioClip[] pickupSound;
        [Range(0, 1)] public float pickupSoundVol;

        //Tenia
        public ItemType itemType;
        public Sprite icon;
        public bool isStackable;
        public int maxStackSize;
    }
}
