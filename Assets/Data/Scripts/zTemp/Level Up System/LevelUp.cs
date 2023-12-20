using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    protected PlayerStats playerStats;
    protected WeaponController weaponStats;
    protected SpecialController specialStats;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        weaponStats = FindObjectOfType<WeaponController>();
        specialStats = FindObjectOfType<SpecialController>();
    }
    public void LevelUpSpeed()
    {
        playerStats.CurrentSpeed *= 1 + 5 / 100f;
    }

    public void LevelUpAttack()
    {
        weaponStats.CurrDamage += 10;
    }
    public void LevelUpAttackSpeed()
    {
        if (weaponStats.hitDelay <= 0.5f)
        {
            return;
        }
        weaponStats.hitDelay *= 1 - 10 / 100f;
    }

    public void LevelUpSpecial()
    {
        specialStats.CurrDamage += 10;
    }
    public void LevelUpSpecialSpeed()
    {
        if (specialStats.hitDelay <= 2f)
        {
            return;
        }
        specialStats.hitDelay *= 1 - 10 / 100f;
    }
}
