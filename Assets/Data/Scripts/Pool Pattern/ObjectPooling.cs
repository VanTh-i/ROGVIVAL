using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPooling : ThaiBehaviour
{
    [SerializeField] protected List<Transform> prefabs;
    [SerializeField] public List<Transform> poolObjs;
    [HideInInspector] public Transform poolHolder;

    protected override void LoadComponents()
    {
        LoadPrefabs();
        LoadPoolHolder();
    }

    protected virtual void LoadPrefabs()
    {
        if (prefabs.Count > 0)
        {
            return;
        }

        Transform prefabObj = transform.Find("Prefabs");
        foreach (Transform prefab in prefabObj)
        {
            prefabs.Add(prefab);
        }

        HidePrefabs();
    }

    protected virtual void HidePrefabs()
    {
        foreach (Transform prefab in prefabs)
        {
            prefab.gameObject.SetActive(false);
        }
    }

    protected virtual void LoadPoolHolder()
    {
        if (poolHolder != null)
        {
            return;
        }
        poolHolder = transform.Find("Pool");
    }
    public virtual void DeSpawn(Transform obj)
    {
        poolObjs.Add(obj);
        obj.gameObject.SetActive(false);
    }

    public virtual Transform Spawn(Vector3 spawnPos, Quaternion rotation, int index)//spawn 1 obj nhat dinh
    {
        Transform prefab = this.prefabs[index];
        Transform obj = GetObjectFromPool(prefab);
        obj.SetPositionAndRotation(spawnPos, rotation);
        obj.parent = poolHolder;
        return obj;
    }

    public virtual Transform Spawn(Transform enemyPrefab, Vector3 spawnPos, Quaternion rotation)//spawn enemy
    {
        Transform prefab = enemyPrefab;
        Transform obj = GetObjectFromPool(prefab);
        obj.SetPositionAndRotation(spawnPos, rotation);
        obj.parent = poolHolder;
        return obj;
    }

    public virtual Transform Spawn(Vector3 spawnPos, Quaternion rotation)//spawn random obj
    {
        Transform prefab = this.prefabs[RandomPrefab()];
        Transform obj = GetObjectFromPool(prefab);
        obj.SetPositionAndRotation(spawnPos, rotation);
        obj.parent = poolHolder;
        return obj;
    }

    public virtual int RandomPrefab()
    {
        int rand = Random.Range(0, prefabs.Count);
        return rand;
    }

    protected virtual Transform GetObjectFromPool(Transform prefab)
    {
        foreach (Transform poolObj in poolObjs)
        {
            if (poolObj.name == prefab.name)
            {
                poolObjs.Remove(poolObj);
                return poolObj;
            }
        }

        Transform newPrefab = Instantiate(prefab);
        newPrefab.name = prefab.name;
        return newPrefab;
    }
}
