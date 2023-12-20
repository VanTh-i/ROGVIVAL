using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyromancerProjectile : EnemyImpact
{
    private Vector3 direction = Vector3.right;
    private PyromancerLamp pyromancerLamp;

    private void OnEnable()
    {
        if (pyromancerLamp == null)
        {
            pyromancerLamp = FindObjectOfType<PyromancerLamp>();
        }
    }

    private void Update()
    {
        MovingForward();
    }
    protected virtual void MovingForward()
    {
        transform.parent.Translate(direction * 10 * Time.deltaTime);
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.gameObject.CompareTag("Enemy"))
        {

            if (other.gameObject.TryGetComponent(out EnemyStats enemyStats))
            {
                enemyStats.TakeDamage(pyromancerLamp.GetCurrentDamage(), transform.parent.position);
            }
        }
    }
}
