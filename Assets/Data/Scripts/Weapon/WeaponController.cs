using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : ThaiBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponStats;
    private int currDamage;
    private int currProjectile;
    public float currHitDelay;
    public float hitDelay;
    [HideInInspector] public float currPierce;
    [HideInInspector] public float currRange;

    // public int CurrDamage
    // {
    //     get => currDamage;
    //     set
    //     {
    //         currDamage = value;
    //     }
    // }
    public int CurrDamage { get; set; }
    public int CurrProjectile { get => currProjectile; set => currProjectile = value; }

    protected override void Awake()
    {
        base.Awake();
        CurrDamage = weaponStats.Damage;
        currProjectile = weaponStats.Projectile;
        hitDelay = weaponStats.HitDelay;
        currPierce = weaponStats.Pierce;
        currRange = weaponStats.Range;
    }
    protected virtual void Start()
    {
        currHitDelay = 0f;

        //GameManager.Instance.attackDamageDisplay.text = "" + currDamage.ToString("F2");

    }
    protected virtual void Update()
    {
        CanAttack();
    }

    protected virtual void CanAttack()
    {
        currHitDelay -= Time.deltaTime;
        if (currHitDelay <= 0f)
        {
            currHitDelay = 0f;
            if (GameManager.Instance.currentState == GameManager.GameState.Paused ||
             GameManager.Instance.currentState == GameManager.GameState.GameOver)
            {
                return;
            }
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currHitDelay = hitDelay;
    }

    public int GetCurrentDamage()
    {
        return CurrDamage + FindObjectOfType<PlayerStats>().CurrentStrength;
    }

}
