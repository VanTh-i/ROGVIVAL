using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer
{
    public static Transform GetPlayer()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            return playerObj.GetComponent<Transform>();
        }
        return null;
    }
}
