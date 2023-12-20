using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : ThaiBehaviour
{
    protected Rigidbody2D rb;
    private BoxCollider2D box2d;
    [HideInInspector] public Vector2 moveDir;

    protected PlayerStats playerStats;


    protected override void LoadComponents()
    {
        playerStats = GetComponent<PlayerStats>();
        LoadCollider();
        LoadRigidbody();
    }

    protected virtual void LoadRigidbody()
    {
        if (rb != null) return;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    protected virtual void LoadCollider()
    {
        if (box2d != null) return;
        box2d = GetComponent<BoxCollider2D>();
        //this.box2d.isTrigger = true;
    }

    private void FixedUpdate()
    {
        Moving();
    }

    protected virtual void Moving()
    {
        moveDir = InputManager.Instance.MoveDir;

        //moving
        rb.velocity = new Vector2(moveDir.x * playerStats.CurrentSpeed, moveDir.y * playerStats.CurrentSpeed);
    }
}
