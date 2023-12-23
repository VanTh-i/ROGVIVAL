using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject audioPanelUI;

    private void Start()
    {
        audioPanelUI.SetActive(false);
        SoundManager.Instance.musicSource.Stop();
        SoundManager.Instance.PlayMusic("Title Theme");
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
