using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// Script controls the sound manager. Provides a function to use in other scripts to call the manager to play audio clips
    /// Functions to change the volume of the sound are included
    /// </summary>

    public static SoundManager instance { get; private set; }
    private AudioSource source;
    private AudioSource musicSource;
    
    private void Awake()
    {
        instance = this;  
        source = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        //Ensures that there is only one SoundManager in a scene. Prevents multiple of the same audio sources playing
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

    //Function to call Sound Manager to play audio clips in other scripts
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
