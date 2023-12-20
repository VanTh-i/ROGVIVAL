using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : ObjectPooling
{
    private static BulletSpawn instance;
    public static BulletSpawn Instance { get => instance; }

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

}
