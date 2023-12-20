using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronHeart : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerStats.CurrentMaxHP += (int)passiveItemData.Multipler;
        playerStats.UpdateHealthBar();
    }
}
