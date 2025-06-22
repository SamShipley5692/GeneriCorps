using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff", menuName = "Scriptable Objects/Staff", order =1)]
public class Staff : ScriptableObject
{
    public string staff;


    public int numberOfstaffToCreate;
    

    public Vector3[] spawnPoints;
    AudioSource m_shootingSound;


    void Start() => m_shootingSound = GetCompenent<AudioSource>();

    private T GetCompenent<T>()
    {
        throw new NotImplementedException();
    }
}
