using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Scriptable Objects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] AnimationClip attackAnimation;
    [SerializeField] AnimationClip reloadAnimation;
    [SerializeField] float timeBetweenAnimationCycles = .1f;
    [SerializeField] float maxAttackRange = 2f;
    [SerializeField] float damage = 10f;

    public AnimationClip GetAttackAnimation()
    {
        return attackAnimation;
    }
}
