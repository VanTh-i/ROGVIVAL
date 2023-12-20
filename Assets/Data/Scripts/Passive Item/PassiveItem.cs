using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveItem : ThaiBehaviour
{
    public PassiveItemScriptableObject passiveItemData;
    protected PlayerStats playerStats;

    protected virtual void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }
    protected abstract void ApplyModifier();
}
