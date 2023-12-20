using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : ThaiBehaviour
{

    [System.Serializable]
    public class Drop
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }
    public List<Drop> drops;

    protected virtual void OnDisable()
    {

    }
    protected virtual void OnDestroy()
    {

    }
}
