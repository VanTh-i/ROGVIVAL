using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraDmgSender : PassiveItem
{
    protected int auraDamage;

    protected override void ApplyModifier()
    {
        auraDamage += (int)passiveItemData.Multipler;
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent(out EnemyStats enemyStats))
            {
                //enemyStats.TakeDamage(auraDamage);
            }
        }
    }

}
