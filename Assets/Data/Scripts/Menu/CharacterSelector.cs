using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private static CharacterSelector instance;
    public static CharacterSelector Instance { get => instance; }

    public CharacterScriptableObjects characterData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static CharacterScriptableObjects GetData()
    {
        return instance.characterData;
    }
    public void SelectCharacter(CharacterScriptableObjects character)
    {
        characterData = character;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
