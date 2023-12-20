using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStats : EnemyStats
{
    private EnemySpawnSystem enemySpawn;

    protected override void OnEnable()
    {
        base.OnEnable();
        enemySpawn = FindObjectOfType<EnemySpawnSystem>();
    }
    protected override void Kill()
    {
        base.Kill();
        EnemySpawn.Instance.DeSpawn(transform.parent);
        enemySpawn.OnEnemyKilled();
        SpawnExplosion();
    }
    protected virtual void SpawnExplosion()
    {
        Transform explosion = FxSpawn.Instance.Spawn(transform.position, transform.rotation, 0);
        explosion.gameObject.SetActive(true);
    }
}
