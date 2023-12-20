using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyBible : WeaponController
{
    private Transform player;
    public GameObject holyBibleProjectile;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        player = GetComponentInParent<Transform>();
        if (player == null)
        {
            Debug.LogError("Can not find player");
        }
    }
    protected override void Attack()
    {
        base.Attack();
        StartCoroutine(HolyBibleAttack());
    }
    private IEnumerator HolyBibleAttack()
    {
        holyBibleProjectile.gameObject.SetActive(true);

        yield return new WaitForSeconds(4f);

        holyBibleProjectile.gameObject.SetActive(false);

    }
}
