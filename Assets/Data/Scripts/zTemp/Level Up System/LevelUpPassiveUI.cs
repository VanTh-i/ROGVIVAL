using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPassiveUI : MonoBehaviour
{
    public Button[] levelUpPassiveBtn;
    public bool hasSpawnAura = false;
    public bool hasSpawnFamiliar = false;
    private void Start()
    {
        if (!hasSpawnAura && !hasSpawnFamiliar)
        {
            int random = Random.Range(0, levelUpPassiveBtn.Length - 2);
            levelUpPassiveBtn[random].gameObject.SetActive(true);
            Debug.Log("aura and Familiar");
            if (random == 0)
            {
                hasSpawnAura = true;
            }
            else if (random == 1)
            {
                hasSpawnFamiliar = true;
            }
        }

    }

    private void OnEnable()
    {
        if (hasSpawnAura && !hasSpawnFamiliar)
        {
            int[] value = { 1, 2 };
            int random = value[Random.Range(0, value.Length)];
            levelUpPassiveBtn[random].gameObject.SetActive(true);
            Debug.Log(random);
            Debug.Log("aura");
            if (random == 1)
            {
                hasSpawnFamiliar = true;
            }
        }

        else if (hasSpawnFamiliar && !hasSpawnAura)
        {
            int[] value = { 0, 3 };
            int random = value[Random.Range(0, value.Length)];
            levelUpPassiveBtn[random].gameObject.SetActive(true);
            Debug.Log(random);
            Debug.Log("Familiar");
            if (random == 0)
            {
                hasSpawnAura = true;
            }
        }

        else if (hasSpawnFamiliar && hasSpawnAura)
        {
            int random = Random.Range(levelUpPassiveBtn.Length - 2, levelUpPassiveBtn.Length);
            levelUpPassiveBtn[random].gameObject.SetActive(true);
            Debug.Log("ca 2");
        }
    }

    private void OnDisable()
    {
        if (levelUpPassiveBtn != null)
        {
            foreach (Button button in levelUpPassiveBtn)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
