using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : DestroyByTime
{
    protected override void DestroyObject()
    {
        BulletSpawn.Instance.DeSpawn(transform.parent);
    }
}
