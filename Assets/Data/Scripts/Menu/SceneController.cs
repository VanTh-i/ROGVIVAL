using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
        SoundManager.Instance.PlaySFX("Button");
    }

    public void TitleMenu()
    {
        SceneManager.LoadScene(0);
        SoundManager.Instance.PlaySFX("Button");
        Time.timeScale = 1f;
        SoundManager.Instance.musicSource.Stop();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
