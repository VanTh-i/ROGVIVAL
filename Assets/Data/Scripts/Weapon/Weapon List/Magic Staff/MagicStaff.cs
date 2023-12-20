using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStaff : WeaponController
{
    public Transform shootPoint;
    [HideInInspector] public EnemyStats closestEnemy;

    protected override void Attack()
    {
        base.Attack();
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

            if (Vector3.Distance(transform.parent.position, closestEnemy.transform.position) <= currRange)
            {
                for (int i = 0; i < CurrProjectile; i++)
                {
                    Vector3 spawnPos = shootPoint.transform.position;
                    float offsetMinDist = 0.5f;
                    Vector3 offset = new Vector3(Random.Range(-offsetMinDist, offsetMinDist), Random.Range(-offsetMinDist, offsetMinDist), 0f);
                    spawnPos += offset;
                    Quaternion rot = shootPoint.transform.rotation;
                    Transform bullet = BulletSpawn.Instance.Spawn(spawnPos, rot, 2);
                    bullet.gameObject.SetActive(true);
                }
            }
            else return;
        }
        else
        {
            return;
        }

    }
}
