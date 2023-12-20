using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenMedal : WeaponController
{
    private float damageICD = 0.2f;
    private float lastDamageICD = 0f;
    private Collider2D box;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        box = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();
        transform.localScale += new Vector3(currRange, currRange, currRange);
        StartCoroutine(DamageCooldown());
    }

    private IEnumerator DamageCooldown()
    {
        while (true)
        {
            box.enabled = true;
            yield return new WaitForSeconds(hitDelay / 2);
            box.enabled = false;
            yield return new WaitForSeconds(hitDelay / 2);
        }

    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time - lastDamageICD >= damageICD)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if (other.gameObject.TryGetComponent(out EnemyStats enemyStats))
                {
                    enemyStats.TakeDamage(GetCurrentDamage(), transform.parent.position);
                    lastDamageICD = Time.time;
                }
            }
        }
    }
}
