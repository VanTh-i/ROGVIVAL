using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStaffProjectile : EnemyImpact
{
    private Vector3 direction = Vector3.right;
    private MagicStaff magicStaff;
    private int pierce;

    // protected override void LoadComponents()
    // {
    //     base.LoadComponents();
    //     magicStaff = FindObjectOfType<MagicStaff>();
    // }
    // private void Start()
    // {
    //     pierce = magicStaff.weaponStats.Pierce;
    // }
    private void OnEnable()
    {
        if (magicStaff == null)
        {
            magicStaff = FindObjectOfType<MagicStaff>();
        }
        pierce = magicStaff.weaponStats.Pierce;
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
                enemyStats.TakeDamage(magicStaff.GetCurrentDamage(), transform.parent.position);
            }
        }
    }
    protected virtual void DestroyObject()
    {
        pierce--;
        if (pierce <= 0)
        {
            BulletSpawn.Instance.DeSpawn(transform.parent);
            pierce = magicStaff.weaponStats.Pierce;
        }
        SpawnExplosion();
    }
}
