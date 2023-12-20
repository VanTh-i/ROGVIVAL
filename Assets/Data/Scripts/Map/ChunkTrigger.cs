using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : ThaiBehaviour
{
    MapController map;
    public GameObject targetMap;
    protected override void LoadComponents()
    {
        map = FindObjectOfType<MapController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            map.currentChunk = targetMap;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (map.currentChunk == targetMap)
            {
                map.currentChunk = null;
            }
        }
    }
}
