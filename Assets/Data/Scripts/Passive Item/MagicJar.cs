using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicJar : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerStats.CurrentMagnet += MagnetMultipler();
    }
    private float MagnetMultipler()
    {
        return playerStats.characterStats.Magnet * (passiveItemData.Multipler / 100f);
    }
}
