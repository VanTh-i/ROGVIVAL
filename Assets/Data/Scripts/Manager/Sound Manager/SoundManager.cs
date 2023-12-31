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
    private Dictionary<string, Sound> musicDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> sfxDictionary = new Dictionary<string, Sound>();

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

        foreach (Sound music in musicSounds)
        {
            musicDictionary.Add(music.name, music);
        }

        foreach (Sound sfx in sfxSounds)
        {
            sfxDictionary.Add(sfx.name, sfx);
        }

    }

    public void PlayMusic(string name)
    {
        // Sound music = musicSounds.Find(sound => sound.name == name);
        // if (music == null)
        // {
        //     Debug.LogWarning("Không tìm thấy âm thanh có tên " + name);
        // }
        // else
        // {
        //     musicSource.clip = music.clip;
        //     musicSource.Play();
        // }
        if (musicDictionary.ContainsKey(name))
        {
            Sound music = musicDictionary[name];
            musicSource.clip = music.clip;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy âm thanh có tên " + name);
        }
    }

    public void PlaySFX(string name)
    {
        if (sfxDictionary.ContainsKey(name))
        {
            Sound sfx = sfxDictionary[name];
            SFXSource.PlayOneShot(sfx.clip);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy âm thanh có tên " + name);
        }
    }

}
