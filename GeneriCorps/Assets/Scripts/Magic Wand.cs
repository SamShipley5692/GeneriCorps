using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MagicWand", menuName = "Scriptable Objects/MagicWand")]
public class MagicWand : ScriptableObject
{
    public string magicwand;

    public int numberOfPrefabsToCreate;

    public Vector3[] spawnPoints;

     AudioSource m_shootingSound;

    void Start() => m_shootingSound = GetCompenent<AudioSource>();

    private T GetCompenent<T>()
    {
        throw new NotImplementedException();
    }

    
}
