using System.ComponentModel;
using UnityEngine;

namespace Holistic3D.Inventory
{
    [CreateAssetMenu]
    public class speedItems : ScriptableObject
    {
        public GameObject itemModel;
        [Range(1, 100)] public int speedAmount;
        public AudioClip[] pickupSound;
        [Range(0, 1)] public float pickupSoundVol;

        [Range(1f, 10f)] public float buffDuration;

        // Cade

        //Tenia
        public ItemType itemType;
        public Sprite icon;
        public bool isStackable;
        public int maxStackSize;
    }
}
