using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;

    public Slider music, sounds;

    List <Resolution> resolutionsList = new List<Resolution>();

    void Start()
    {
        int currentResolutionIndex = -1;
        
        for(int i = 0; i < resolutionDropdown.options.Count && currentResolutionIndex == -1; i++)
        {
            string currentResolution = Screen.currentResolution.width + " x " + Screen.currentResolution.height;

            Resolution resolution = new Resolution
            {
                width = Screen.currentResolution.width,
                height = Screen.currentResolution.height
            };

            resolutionsList.Add(resolution);

            if (resolutionDropdown.options[i].text == currentResolution)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        float vol = 0f;

        audioMixer.GetFloat("musicVolume", out vol);
        music.value = vol;

         audioMixer.GetFloat("playerVolume", out vol);
        sounds.value = vol;
    }

    public void SetVolumeMusic(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }
    public void SetVolumeSounds(float volume)
    {
        audioMixer.SetFloat("playerVolume", volume);
        audioMixer.SetFloat("voiceVolume", volume);
        audioMixer.SetFloat("effectVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {        
        Resolution resolution = resolutionsList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
