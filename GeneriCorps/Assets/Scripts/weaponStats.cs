using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Stats")]
public class weaponStats : ScriptableObject
{
    public GameObject model;
    [Range(1, 10)] public int shootDamage;
    [Range(0.1f, 2)] public float shootRate;
    [Range(5, 1000)] public int shootDistance;
    public int ammoCurrent;
    [Range(1, 150)] public int ammoMax;

    public ParticleSystem hitEffect;
    public AudioClip[] shootSound;
    [Range(0, 1)] public float shootSoundVol;
}
