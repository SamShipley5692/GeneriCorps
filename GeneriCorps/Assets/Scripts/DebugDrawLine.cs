using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugDrawLine : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 50);
    }
}
