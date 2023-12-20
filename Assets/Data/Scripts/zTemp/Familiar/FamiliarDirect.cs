using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarDirect : MonoBehaviour
{
    protected Vector3 targetPosition;

    private void FixedUpdate()
    {
        FamiliarDirection();
    }

    private void FamiliarDirection()
    {
        targetPosition = GameObject.Find("Familiar Direction").GetComponent<Transform>().transform.position;

        Vector3 diff = targetPosition;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        Vector3 aimDir = Vector3.one;
        if (rot_z > 90 || rot_z < -90)
        {
            aimDir.x = 1f;
        }
        else
        {
            aimDir.x = -1f;
        }
        transform.localScale = aimDir;
    }
}
