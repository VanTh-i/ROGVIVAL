using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObject/Enemy")]

public class EnemyScriptableObject : ScriptableObject
{
    [SerializeField] private int damage;
    public int Damage { get => damage; private set => damage = value; }

    [SerializeField] private int maxHp;
    public int MaxHp { get => maxHp; private set => maxHp = value; }

    [SerializeField] private float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField] private int attackRange;
    public int AttackRange { get => attackRange; private set => attackRange = value; }
}
