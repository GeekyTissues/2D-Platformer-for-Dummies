using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;
    private AudioSource musicSource;

    private void Awake()
    {
        instance = this;  
        source = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _change)
    {
        //Get initial value of volume and change it
        float currentVolume = PlayerPrefs.GetFloat("soundVolume");
        currentVolume += _change;

        //Check if we reached the maximum or minimum value
        if(currentVolume > 1)
        {
            currentVolume = 0;
        }
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }

        //Assign final value
        source.volume = currentVolume;

        //Save final value to player prefs
        PlayerPrefs.SetFloat("soundVolume", currentVolume);
    }
    
    public void ChangeMusicVolume(float _change)
    {
        //Get initial value of volume and change it
        float currentVolume = PlayerPrefs.GetFloat("musicVolume");
        currentVolume += _change;

        //Check if we reached the maximum or minimum value
        if (currentVolume > 1)
        {
            currentVolume = 0;
        }
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }

        //Assign final value
        musicSource.volume = currentVolume;

        //Save final value to player prefs
        PlayerPrefs.SetFloat("musicVolume", currentVolume);
    }
}
