using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodLustPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerStats.CurrentRecovery = (int)passiveItemData.Multipler;
    }
}
