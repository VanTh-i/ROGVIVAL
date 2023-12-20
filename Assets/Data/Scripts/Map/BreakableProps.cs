using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public PropScriptableObject propStats;
    private int currentDurability;
    private void Awake()
    {
        currentDurability = propStats.Durability;
    }
    public void TakeDamage(int dmg)
    {
        currentDurability -= dmg;

        if (currentDurability <= 0)
        {
            Kill();
        }
    }
    private void Kill()
    {
        Destroy(gameObject);
    }
}
