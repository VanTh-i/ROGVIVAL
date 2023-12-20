using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandom : MonoBehaviour
{
    [SerializeField] protected List<GameObject> propSpawnPoint;
    [SerializeField] protected List<GameObject> propPrefabs;

    private void Start()
    {
        SpawnProps();
    }
    protected virtual void SpawnProps()
    {
        foreach (GameObject sp in propSpawnPoint)
        {
            int random = Random.Range(0, propPrefabs.Count);
            GameObject propPrefab = propPrefabs[random];
            Quaternion rotation = propPrefab.transform.rotation;
            GameObject prop = Instantiate(propPrefab, sp.transform.position, rotation);
            prop.transform.parent = sp.transform;
        }
    }
}
