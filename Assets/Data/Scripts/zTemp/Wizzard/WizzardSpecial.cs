using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizzardSpecial : SpecialController
{
    protected Animator animator;
    // public int specialDamage;
    // public float specialDelay;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        animator = GetComponentInParent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        // specialDamage = currDamage;
        // specialDelay = hitDelay;
    }

    protected override void Update()
    {
        CanSpecial();
    }
    protected override void Special()
    {
        if (!InputManager.Instance.OnRightClick)
        {
            return;
        }
        base.Special();
        animator.SetTrigger("Special");
    }
}
