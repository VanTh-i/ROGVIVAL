using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSpecial : WeaponController
{
    protected Animator animator;
    protected BoxCollider2D playerCol;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        animator = GetComponent<Animator>();
    }
    protected override void Attack()
    {
        if (!InputManager.Instance.OnRightClick)
        {
            return;
        }
        base.Attack();
        animator.SetTrigger("Special");
        SpawnSlashFX();
        StartCoroutine(WarriorInvincible());

    }

    private IEnumerator WarriorInvincible()
    {
        playerCol = GetComponentInParent<BoxCollider2D>();
        if (playerCol == null)
        {
            Debug.LogError("Can not get");
        }

        playerCol.enabled = false;
        yield return new WaitForSeconds(2f);
        playerCol.enabled = true;
    }

    protected virtual void SpawnSlashFX()
    {
        Transform explosion = FxSpawn.Instance.Spawn(transform.position, transform.rotation, 3);
        explosion.gameObject.SetActive(true);
    }

}
