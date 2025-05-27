using UnityEngine;

[CreateAssetMenu]
public class healthItems : ScriptableObject
{
    public GameObject itemModel;
    [Range(1, 1000)] public int healthAmount;
    public AudioClip[] pickupSound;
    [Range(0, 1)] public float pickupSoundVol;
}
