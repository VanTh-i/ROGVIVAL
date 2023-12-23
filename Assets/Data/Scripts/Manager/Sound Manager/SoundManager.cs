using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get => instance; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }
    public List<Sound> musicSounds, sfxSounds;

    public AudioSource musicSource, SFXSource;


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

    public void PlayMusic(string name)
    {
        Sound music = musicSounds.Find(sound => sound.name == name);
        if (music == null)
        {
            Debug.LogWarning("Không tìm thấy âm thanh có tên " + name);
        }
        else
        {
            musicSource.clip = music.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sfx = sfxSounds.Find(sound => sound.name == name);
        if (sfx == null)
        {
            Debug.LogWarning("Không tìm thấy âm thanh có tên " + name);
        }
        else
        {
            SFXSource.PlayOneShot(sfx.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
    }

}
