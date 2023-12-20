using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttack : WeaponController
{
    protected Animator animator;
    protected Transform attackPoint;
    public LayerMask enemyLayer;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        animator = GetComponent<Animator>();
        attackPoint = GameObject.FindGameObjectWithTag("ShootPoint").transform;
    }

    protected override void Attack()
    {
        if (!InputManager.Instance.OnLeftClick)
        {
            return;
        }
        base.Attack();
        animator.SetTrigger("Attack");
        SpawnSlashFX();

    }

    public void DealingDamage() // call trong animation
    {
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, currRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemys)
        {
            float distance = Vector2.Distance(attackPoint.position, enemy.transform.position);

            if (distance <= currRange)
            {
                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    //enemyStats.TakeDamage(CurrDamage);
                }
                BreakableProps breakableProps = enemy.GetComponent<BreakableProps>();
                if (breakableProps != null)
                {
                    breakableProps.TakeDamage(1);
                }
            }
        }
    }

    protected virtual void SpawnSlashFX()
    {
        Transform explosion = FxSpawn.Instance.Spawn(transform.position, transform.rotation, 2);
        explosion.gameObject.SetActive(true);
        explosion.transform.localScale = new Vector3(currRange * 2, currRange * 2, currRange * 2);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, currRange);
    }
}
