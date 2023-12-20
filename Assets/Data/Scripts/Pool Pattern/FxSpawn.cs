using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxSpawn : ObjectPooling
{
    private static FxSpawn instance;
    public static FxSpawn Instance { get => instance; }

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
}
