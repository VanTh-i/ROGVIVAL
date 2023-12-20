using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawn : ObjectPooling
{
    private static PickUpSpawn instance;
    public static PickUpSpawn Instance { get => instance; }
    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
}
