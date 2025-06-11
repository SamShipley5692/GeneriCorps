using UnityEngine;

[CreateAssetMenu(fileName = "Staff", menuName = "Scriptable Objects/Staff", order =1)]
public class Staff : ScriptableObject
{
    public string staff;


    public int numberOfstaffToCreate;
    

    public Vector3[] spawnPoints;

}
