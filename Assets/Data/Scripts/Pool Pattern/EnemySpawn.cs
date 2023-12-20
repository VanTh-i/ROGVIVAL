using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : ObjectPooling
{
    private static EnemySpawn instance;
    public static EnemySpawn Instance { get => instance; }

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
}
