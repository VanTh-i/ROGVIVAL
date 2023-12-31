using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : PickUp, ICollectible
{
    public int experienceGranted;
    public void Collect()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.IncreaseExperience(experienceGranted);
        SoundManager.Instance.PlaySFX("Pick Up");
        //Destroy(gameObject);
    }
}
