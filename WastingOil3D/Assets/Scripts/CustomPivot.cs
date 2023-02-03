using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPivot : MonoBehaviour
{
    public float gismoSize = 0.75f;
    public Color gizmoColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gismoSize);
    }
}
