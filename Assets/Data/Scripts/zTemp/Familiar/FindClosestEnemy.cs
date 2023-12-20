using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestEnemy : ThaiBehaviour
{
    private Transform shootPoint;
    public Animator animator;

    private float timer;
    private float currTime = 5f;
    [HideInInspector] public EnemyStats closestEnemy;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        shootPoint = GameObject.Find("Familiar Direction").GetComponent<Transform>();
        // animator = GameObject.Find("Familiar").GetComponentInChildren<Animator>();
    }
    void Update()
    {
        FindEnemy();
    }

    private void FindEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        closestEnemy = null;
        EnemyStats[] allEnemies = FindObjectsOfType<EnemyStats>();

        foreach (EnemyStats currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }

        if (closestEnemy != null)
        {
            Debug.DrawLine(this.transform.position, closestEnemy.transform.position);
            Vector3 direction = (closestEnemy.transform.position - shootPoint.position).normalized;
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            shootPoint.rotation = Quaternion.Euler(0f, 0f, rot_z);

            if (Vector3.Distance(transform.parent.position, closestEnemy.transform.position) <= 10)
            {
                if (Time.time - timer >= currTime)
                {
                    timer = Time.time;
                    CanAttack();
                }
            }
            else return;
        }
        else
        {
            return;
        }

    }

    private void CanAttack()
    {
        animator.SetTrigger("Attack");
    }
}
