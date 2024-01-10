using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTreasure : MonoBehaviour
{
    private void OnDisable()
    {
        GameManager.Instance.Coin += Random.Range(10, 30);
    }
}
