using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirect : ThaiBehaviour
{
    protected Vector2 moveDir;
    protected SpriteRenderer playerSprite;
    protected float lastHorizontalVector;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        playerSprite = GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        PlayerDirection();
    }

    private void PlayerDirection()
    {
        moveDir = InputManager.Instance.MoveDir;
        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
        }

        if (lastHorizontalVector < 0)
        {
            playerSprite.flipX = false;
        }
        else
        {
            playerSprite.flipX = true;
        }
    }
}
