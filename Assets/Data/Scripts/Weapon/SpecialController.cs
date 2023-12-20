using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialController : ThaiBehaviour
{
    [Header("Special Stats")]
    public WeaponScriptableObject specialStats;
    public int currDamage;
    public float currHitDelay;
    public float hitDelay;
    [HideInInspector] public float currPierce;
    [HideInInspector] public float currRange;

    public int CurrDamage
    {
        get => currDamage;
        set
        {
            currDamage = value;
            // if (GameManager.Instance != null)
            // {
            //     GameManager.Instance.specialDamageDisplay.text = "" + currDamage.ToString("F2");
            // }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        CurrDamage = specialStats.Damage;
        hitDelay = specialStats.HitDelay;
        currPierce = specialStats.Pierce;
        currRange = specialStats.Range;
    }
    protected virtual void Start()
    {
        currHitDelay = 0f;
        //GameManager.Instance.specialDamageDisplay.text = "" + currDamage.ToString("F2");

    }
    protected virtual void Update()
    {
        CanSpecial();
    }

    protected virtual void CanSpecial()
    {
        currHitDelay -= Time.deltaTime;
        if (currHitDelay <= 0f)
        {
            currHitDelay = 0f;
            if (GameManager.Instance.currentState == GameManager.GameState.Paused ||
             GameManager.Instance.currentState == GameManager.GameState.GameOver)
            {
                return;
            }
            Special();
        }
    }

    protected virtual void Special()
    {
        currHitDelay = hitDelay;
    }
}
