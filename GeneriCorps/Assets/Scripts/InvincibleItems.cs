

using System.ComponentModel;
using UnityEngine;

namespace Holistic3D.Inventory
{
    [CreateAssetMenu]
    public class InvincibleItems : ScriptableObject
    {
        public GameObject itemModel;
        public AudioClip[] pickupSound;
        [Range(0, 1)] public float pickupSoundVol;

        [Range(1f, 10f)] public float buffDuration;

        // Cade

        //Tenia
        public ItemType itemType;
        public bool isStackable;
        public int maxStackSize;
    }
}