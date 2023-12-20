using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : DropRateManager
{
    protected override void OnDisable()
    {
        float random = Random.Range(0f, 100f);
        List<Drop> possibleDrops = new List<Drop>();

        foreach (Drop drop in drops)
        {
            if (random <= drop.dropRate)
            {
                possibleDrops.Add(drop);
            }
        }
        if (possibleDrops.Count > 0)
        {
            Drop drop = possibleDrops[Random.Range(0, possibleDrops.Count)];
            if (!gameObject.scene.isLoaded) return;
            // GameObject item = Instantiate(drop.itemPrefab, transform.position, Quaternion.identity);
            // item.transform.parent = poolHolder;
            Transform item = PickUpSpawn.Instance.Spawn(drop.itemPrefab.transform, transform.position, Quaternion.identity);
            item.gameObject.SetActive(true);
        }
    }
}
