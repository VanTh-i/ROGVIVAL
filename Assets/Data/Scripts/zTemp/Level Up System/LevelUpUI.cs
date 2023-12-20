using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public Button[] levelUpAbilityBtn;
    private void OnEnable()
    {
        if (levelUpAbilityBtn != null && levelUpAbilityBtn.Length > 0)
        {
            int random = Random.Range(0, levelUpAbilityBtn.Length);
            levelUpAbilityBtn[random].gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (levelUpAbilityBtn != null)
        {
            foreach (Button button in levelUpAbilityBtn)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
