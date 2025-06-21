using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WeaponIK : MonoBehaviour
{
    public Transform targetTransform;
    public Transform aimTransform;
    public Transform bone;

    public int iterations = 10;
    [Range(0,1)]
    public float weight = 1.0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = targetTransform.position;
        for(int i = 0; i < iterations; i++)
        {
        AimAtTarget(bone, targetPosition,weight);

        }
    }
    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRoation = Quaternion.Slerp(Quaternion.identity,aimTowards,weight);
        bone.rotation = aimTowards * bone.rotation;
    }
}
