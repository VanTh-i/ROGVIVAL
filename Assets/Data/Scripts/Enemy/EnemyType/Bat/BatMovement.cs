using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : EnemyMovement
{
    private float collisionCooldown = 1f;
    private float lastCollisionTime = 0f;

    protected override void Action()
    {
        base.Action();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time - lastCollisionTime >= collisionCooldown)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("get hit");
                PlayerStats player = other.gameObject.GetComponent<PlayerStats>();
                player.TakeDamage(enemyStats.currentDamage);
                lastCollisionTime = Time.time;
            }
        }
    }
}
