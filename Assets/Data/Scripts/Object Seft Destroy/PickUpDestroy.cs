using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDestroy : DestroyByTime
{
    protected override void DestroyObject()
    {
        PickUpSpawn.Instance.DeSpawn(transform.parent);
    }
}
