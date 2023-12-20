using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropImpact : ThaiBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Prop"))
        {
            ImpactWithBullet();

            if (other.gameObject.TryGetComponent(out BreakableProps breakableProps))
            {
                breakableProps.TakeDamage(1);
            }
        }
    }
    protected virtual void ImpactWithBullet()
    {
        BulletSpawn.Instance.DeSpawn(transform.parent);
        SpawnExplosion();
    }
    protected virtual void SpawnExplosion()
    {
        Transform explosion = FxSpawn.Instance.Spawn(transform.position, transform.rotation, 1);
        explosion.gameObject.SetActive(true);
    }
}
