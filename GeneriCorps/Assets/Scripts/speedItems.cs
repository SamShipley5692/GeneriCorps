using UnityEngine;

[CreateAssetMenu]
public class speedItems : ScriptableObject
{
    public GameObject itemModel;
    [Range(1, 100)] public int speedAmount;
    public AudioClip[] pickupSound;
    [Range(0, 1)] public float pickupSoundVol;

    [Range(1f, 10f)] public float buffDuration;
   
  // Cade
}
