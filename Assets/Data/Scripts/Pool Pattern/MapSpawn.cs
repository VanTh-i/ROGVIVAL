using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawn : ObjectPooling
{
    private static MapSpawn instance;
    public static MapSpawn Instance { get => instance; }

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
}
