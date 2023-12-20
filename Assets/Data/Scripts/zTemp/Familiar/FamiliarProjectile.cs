using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarProjectile : PassiveItem
{
    private Vector3 direction = Vector3.right;
    private int pierce = 1;
    protected int familiarDamage;

    protected override void ApplyModifier()
    {
        familiarDamage += (int)passiveItemData.Multipler;
    }
    private void Update()
    {
        MovingForward();
    }
    protected virtual void MovingForward()
    {
        transform.parent.Translate(direction * 15 * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            DestroyObject();

            if (other.gameObject.TryGetComponent(out EnemyStats enemyStats))
            {
                //enemyStats.TakeDamage(familiarDamage);
            }
        }
    }
    protected virtual void DestroyObject()
    {
        pierce--;
        if (pierce <= 0)
        {
            BulletSpawn.Instance.DeSpawn(transform.parent);
            pierce = 1;
        }
        SpawnExplosion();
    }

    private void SpawnExplosion()
    {
        Transform explosion = FxSpawn.Instance.Spawn(transform.position, transform.rotation, 1);
        explosion.gameObject.SetActive(true);
    }

    private void OnDisable() //tao ra vu no sau khi dan va cham
    {

    }
}
