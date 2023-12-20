using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerCollector : ThaiBehaviour
{
    private PlayerStats playerStats;
    private CircleCollider2D circle;
    public float pullForce;

    protected override void LoadComponents()
    {
        circle = GetComponent<CircleCollider2D>();
        playerStats = GetComponentInParent<PlayerStats>();
        LoadCollider();
    }

    private void LoadCollider()
    {
        circle.isTrigger = true;
    }

    private void Update()
    {
        if (!(circle.radius == playerStats.CurrentMagnet))
        {
            circle.radius = playerStats.CurrentMagnet;
            Debug.Log("set radius = CurrentMagnet");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out ICollectible collectible))
        {
            //keo vat the lai player
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirect = (transform.position - other.transform.position).normalized;
            rb.AddForce(forceDirect * pullForce);

            StartCoroutine(CollectItem(collectible));
        }
    }
    private IEnumerator CollectItem(ICollectible collectible)
    {
        yield return new WaitForSeconds(0.2f);
        collectible.Collect();
    }
}
