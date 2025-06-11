using UnityEngine;

[CreateAssetMenu(fileName = "MagicWand", menuName = "Scriptable Objects/MagicWand")]
public class MagicWand : ScriptableObject
{
    public string magicwand;

    public int numberOfPrefabsToCreate;

    public Vector3[] spawnPoints;
}
