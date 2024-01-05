using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PickUp, ICollectible
{
    public void Collect()
    {
        Debug.Log("get coin");
        GameManager.Instance.Coin += 1;
        SoundManager.Instance.PlaySFX("Pick Up");
    }
}
