using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObject/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField] private new string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField] private string description;
    public string Description { get => description; private set => description = value; }

    [SerializeField] private Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }

    [SerializeField] private int damage;
    public int Damage { get => damage; private set => damage = value; }

    [SerializeField] private int projectile;
    public int Projectile { get => projectile; private set => projectile = value; }

    [SerializeField] private float hitDelay; //delay don danh
    public float HitDelay { get => hitDelay; private set => hitDelay = value; }

    [SerializeField] private int pierce; // dam xuyen ke thu
    public int Pierce { get => pierce; private set => pierce = value; }

    [SerializeField] private float range;
    public float Range { get => range; private set => range = value; }

    [SerializeField] private int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField] private GameObject nextLevelPrefab;
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }
}
