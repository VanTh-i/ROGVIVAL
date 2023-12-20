using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : WeaponController
{
    public Transform shootPoint;

    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Update()
    {
        base.Update();
        GetShootPoint();
    }

    protected override void Attack()
    {
        base.Attack();
        for (int i = 0; i < CurrProjectile; i++)
        {
            Vector3 spawnPos = shootPoint.transform.position;
            float offsetMinDist = 0.5f;
            Vector3 offset = new Vector3(Random.Range(-offsetMinDist, offsetMinDist), Random.Range(-offsetMinDist, offsetMinDist), 0f);
            spawnPos += offset;

            Quaternion rot = shootPoint.transform.rotation;

            Transform bullet = BulletSpawn.Instance.Spawn(spawnPos, rot, 3);
            bullet.gameObject.SetActive(true);
        }

    }

    private void GetShootPoint()
    {
        Vector3 moveDir = InputManager.Instance.MoveDir;

        if (moveDir.x > 0 && moveDir.y == 0) //right
        {
            shootPoint.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveDir.x < 0 && moveDir.y == 0) // left
        {
            shootPoint.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (moveDir.x == 0 && moveDir.y > 0) // up
        {
            shootPoint.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (moveDir.x == 0 && moveDir.y < 0) // down
        {
            shootPoint.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (moveDir.x > 0 && moveDir.y > 0) // right up
        {
            shootPoint.rotation = Quaternion.Euler(0, 0, 45);
        }
        else if (moveDir.x > 0 && moveDir.y < 0) // right down
        {
            shootPoint.rotation = Quaternion.Euler(0, 0, -45);
        }
        else if (moveDir.x < 0 && moveDir.y > 0) // left up
        {
            shootPoint.rotation = Quaternion.Euler(0, 0, 135);
        }
        else if (moveDir.x < 0 && moveDir.y < 0) // left down
        {
            shootPoint.rotation = Quaternion.Euler(0, 0, -135);
        }
    }
}
