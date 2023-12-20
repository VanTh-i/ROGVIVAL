using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerStats.CurrentStrength = (int)passiveItemData.Multipler;
    }

}
