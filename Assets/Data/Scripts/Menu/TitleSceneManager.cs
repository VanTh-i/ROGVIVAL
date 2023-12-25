using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject audioPanelUI;
    public VolumeSettings volumeSett;

    private void Start()
    {
        audioPanelUI.SetActive(false);
        SoundManager.Instance.PlayMusic("Title Theme");
        if (PlayerPrefs.GetInt("MusicMuted") == 1)
        {
            volumeSett.gameMixer.SetFloat("Music", Mathf.Log10(-80f) * 20);
            volumeSett.MusicBtnStatus();
        }
    }
    public void Setting()
    {
        audioPanelUI.SetActive(true);
        SoundManager.Instance.PlaySFX("Button");
    }

    public void CloseSetting()
    {
        audioPanelUI.SetActive(false);
        SoundManager.Instance.PlaySFX("Button");
    }

}
