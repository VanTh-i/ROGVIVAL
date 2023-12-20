using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMagicBehaviour : EnemyImpact
{
    private WizzardSpecial wizzardSpecial;
    private Vector3 direction = Vector3.right;
    private int pierce;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        wizzardSpecial = GameObject.Find("Player").GetComponentInChildren<WizzardSpecial>();
    }
    private void Start()
    {
        pierce = wizzardSpecial.specialStats.Pierce;
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
                //enemyStats.TakeDamage(wizzardSpecial.CurrDamage);
            }
        }
    }
    protected virtual void DestroyObject()
    {
        pierce--;
        if (pierce <= 0)
        {
            BulletSpawn.Instance.DeSpawn(transform.parent);
            pierce = wizzardSpecial.specialStats.Pierce;
        }
        SpawnExplosion();
    }
    private void OnDisable() //tao ra vu no sau khi dan va cham
    {

    }
}
