using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizzardMagic : WeaponController
{
    protected Animator animator;
    // public int magicDamage;
    // public float magicDelay;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        animator = GetComponentInParent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        // magicDamage = currDamage;
        // magicDelay = hitDelay;
    }

    protected override void Update()
    {
        CanAttack();
    }
    protected override void Attack()
    {
        if (!InputManager.Instance.OnLeftClick)
        {
            return;
        }
        base.Attack();
        animator.SetTrigger("Attack");
    }
}
