using UnityEngine;

[CreateAssetMenu]
public class jumpItems : ScriptableObject
{
    public GameObject itemModel;
    [Range(1, 100)] public int jumpForceAmount;

    public AudioClip[] pickupSound;
    [Range(0, 1)] public float pickupSoundVol;

    [Range(1f, 10f)] public float buffDuration;

    // Cade
}
