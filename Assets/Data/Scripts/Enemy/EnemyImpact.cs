using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpact : ThaiBehaviour
{
    // protected WeaponController weaponStats;
    // protected override void LoadComponents()
    // {
    //     base.LoadComponents();
    //     weaponStats = FindObjectOfType<WeaponController>();
    // }
    // protected void OnEnable()
    // {
    //     weaponStats = FindObjectOfType<WeaponController>();
    //     GetCurrentDamage();
    //     Debug.Log(GetCurrentDamage());
    // }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        //
    }
    protected virtual void SpawnExplosion()
    {
        // Transform explosion = FxSpawn.Instance.Spawn(transform.position, transform.rotation, 1);
        // explosion.gameObject.SetActive(true);
    }
}
