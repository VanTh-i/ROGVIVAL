using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponController
{
    public Transform slashPoint, slashPoint2;
    protected float lastHorizontalVector;
    protected Vector3 swordDirection;

    private void FixedUpdate()
    {
        //swordDirection = InputManager.Instance.MoveDir;
        swordDirection = InputManager.Instance.MobileMoveDir;
        if (swordDirection.x != 0)
        {
            lastHorizontalVector = swordDirection.x;
        }

        if (lastHorizontalVector < 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -180);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
        }
    }
    protected override void Attack()
    {
        base.Attack();
        if (CurrProjectile == 1)
        {
            slashPoint2.gameObject.SetActive(false);
            Vector3 spawnPos = slashPoint.transform.position;
            Quaternion rot = slashPoint.transform.rotation;
            Transform bullet = BulletSpawn.Instance.Spawn(spawnPos, rot, 4);
            bullet.gameObject.SetActive(true);
        }
        else if (CurrProjectile == 2)
        {
            slashPoint2.gameObject.SetActive(true);
            StartCoroutine(SlashSpawn());
        }

    }
    private IEnumerator SlashSpawn()
    {
        Vector3 spawnPos = slashPoint.transform.position;
        Quaternion rot = slashPoint.transform.rotation;
        Transform bullet = BulletSpawn.Instance.Spawn(spawnPos, rot, 4);
        bullet.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Vector3 reverseSpawnPos = slashPoint2.transform.position;
        Quaternion reverserot = slashPoint2.transform.rotation;
        Transform bullet2 = BulletSpawn.Instance.Spawn(reverseSpawnPos, reverserot, 4);
        bullet2.gameObject.SetActive(true);
    }
}
