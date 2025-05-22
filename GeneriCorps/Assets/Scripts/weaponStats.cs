using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Stats")]
public class weaponStats : ScriptableObject
{
    [SerializeField] private string weaponName;
    [SerializeField] private int damage;
    [SerializeField] private float range;

    public string WeaponName => weaponName;
    public int Damage => damage;
    public float Range => range;
}
