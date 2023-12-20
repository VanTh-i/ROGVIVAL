using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerStats.CurrentSpeed += MagnetMultipler();
    }
    private float MagnetMultipler()
    {
        return playerStats.characterStats.Speed * (passiveItemData.Multipler / 100f);
    }
}
