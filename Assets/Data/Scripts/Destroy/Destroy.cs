using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destroy : ThaiBehaviour
{

    protected virtual void FixedUpdate()
    {
        Destroying();
    }

    protected virtual void Destroying()
    {
        if (!CanDestroy())
        {
            return;
        }
        DestroyObject();
    }

    protected virtual void DestroyObject()
    {
        Destroy(transform.parent.gameObject);
    }

    protected abstract bool CanDestroy();
}
