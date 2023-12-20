using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyBibleProjectile : ThaiBehaviour
{
    protected float rotationSpeed = 500f;
    private Transform player;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        player = FindPlayer.GetPlayer();
        if (player == null)
        {
            Debug.LogError("Can not find player");
        }
    }

    void Update()
    {
        transform.RotateAround(player.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

}
