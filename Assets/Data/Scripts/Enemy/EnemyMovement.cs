using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : ThaiBehaviour
{
    protected EnemyStats enemyStats;
    protected Transform player;
    protected Transform enemyDir;

    private Vector2 knockbackVelocity;
    private float knockbackDuration;

    protected override void LoadComponents()
    {
        enemyStats = transform.parent.GetChild(1).GetComponent<EnemyStats>();

        enemyDir = transform.parent.GetComponent<Transform>();

        player = FindPlayer.GetPlayer();
        if (player == null)
        {
            Debug.LogError("Can not find player");
        }
    }

    private void Update()
    {
        MovingForward();
    }
    private void FixedUpdate()
    {
        LookPlayer();
    }

    protected virtual void MovingForward()
    {
        transform.LookAt(player);

        if (Vector3.Distance(transform.parent.position, player.position) >= enemyStats.currentAttackRange)
        {
            if (knockbackDuration > 0)
            {
                transform.parent.position += (Vector3)knockbackVelocity * Time.deltaTime;
                knockbackDuration -= Time.deltaTime;
            }
            else
            {
                //transform.parent.position += transform.forward * enemyStats.currentSpeed * Time.deltaTime;
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, player.position, enemyStats.currentSpeed * Time.deltaTime);
            }
        }
        else
        {
            Action();
        }
    }
    protected virtual void Action()
    {
        // for override
    }

    protected virtual void LookPlayer()
    {
        Vector3 diff = player.position - transform.parent.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        Vector3 aimDir = transform.parent.localScale;
        if (rot_z > 90 || rot_z < -90)
        {
            //aimDir.x = transform.parent.localScale.x * (-1f);
            if (aimDir.x != -Mathf.Abs(transform.parent.localScale.x))
            {
                aimDir.x = -Mathf.Abs(transform.parent.localScale.x);
            }

        }
        else
        {
            if (aimDir.x != Mathf.Abs(transform.parent.localScale.x))
            {
                aimDir.x = Mathf.Abs(transform.parent.localScale.x);
            }
        }
        enemyDir.localScale = aimDir;

    }

    public void Knockback(Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0) return;
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }

}
