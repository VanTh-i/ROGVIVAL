using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropScriptableObject", menuName = "ScriptableObject/Prop")]
public class PropScriptableObject : ScriptableObject
{
    [SerializeField] private int durability;
    public int Durability { get => durability; private set => durability = value; }
}
