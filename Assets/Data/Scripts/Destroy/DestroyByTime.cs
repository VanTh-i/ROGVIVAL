using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : Destroy
{
    public float delay = 1f;
    protected float timer = 0f;

    private void OnEnable()
    {
        ResetTime();
    }
    protected virtual void ResetTime()
    {
        timer = 0f;
    }
    protected override bool CanDestroy()
    {
        timer += Time.fixedDeltaTime;
        if (timer > delay)
        {
            return true;
        }
        return false;
    }
}
