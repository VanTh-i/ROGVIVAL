using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : EnemyImpact
{
    protected Vector3 direction = Vector3.right;
    private Bow bow;
    private int pierce;

    private void OnEnable()
    {
        if (bow == null)
        {
            bow = FindObjectOfType<Bow>();
        }
        pierce = bow.weaponStats.Pierce;
    }

    private void Update()
    {
        MovingForward();
    }
    protected virtual void MovingForward()
    {
        transform.parent.Translate(direction * 10 * Time.deltaTime);
    }


    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.gameObject.CompareTag("Enemy"))
        {
            DestroyObject();
            if (other.gameObject.TryGetComponent(out EnemyStats enemyStats))
            {
                enemyStats.TakeDamage(bow.GetCurrentDamage(), transform.parent.position);
            }

        }
    }
    protected virtual void DestroyObject()
    {
        pierce--;
        if (pierce <= 0)
        {
            BulletSpawn.Instance.DeSpawn(transform.parent);
            pierce = bow.weaponStats.Pierce;
        }
        SpawnExplosion();
    }

}
