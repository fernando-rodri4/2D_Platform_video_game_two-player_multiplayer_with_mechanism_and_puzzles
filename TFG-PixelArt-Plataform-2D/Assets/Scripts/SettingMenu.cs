using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;   //Audios selected
    public Dropdown resolutionDropdown; //List the resolution's options
    public Toggle fullResolution;   //Enable/Disable fullScreen
    public Slider music, sounds;    //Sliders used for the volumen

    List <Resolution> resolutionsList = new List<Resolution>();

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        int currentResolutionIndex = -1;
        
        for(int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            //////////////Create instance of Resolution struct
            int wid, hei;

            wid = int.Parse(resolutionDropdown.options[i].text.Substring(0, resolutionDropdown.options[i].text.IndexOf("x") - 1));
            
            try
            {
                hei = int.Parse(resolutionDropdown.options[i].text.Substring(resolutionDropdown.options[i].text.IndexOf("x") + 2, 4));
            }
            catch
            {
                hei = int.Parse(resolutionDropdown.options[i].text.Substring(resolutionDropdown.options[i].text.IndexOf("x") + 2, 3));
            }

            Resolution resolution = new Resolution
            {
                width = wid,
                
                height = hei

            };
            ///////////////////List the resolution list
            
            resolutionsList.Add(resolution);

            if (resolutionsList[i].width == PlayerPrefs.GetInt("resolutionWidht", 1920) &&
               resolutionsList[i].height == PlayerPrefs.GetInt("resolutionHeight", 1080) && currentResolutionIndex == -1)
            {
                currentResolutionIndex = i;
            }
        }

        if (currentResolutionIndex == -1)
        {
            currentResolutionIndex = 0;
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullResolution.isOn = bool.Parse(PlayerPrefs.GetString("fullScreen", "false"));
        
        music.value = PlayerPrefs.GetFloat("musicVolume", 0);

        sounds.value = PlayerPrefs.GetFloat("soundVolume", 0);
    }

    /// <summary>
    /// Set the volume of the music for the game
    /// </summary>
    public void SetVolumeMusic(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);

        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    /// <summary>
    /// Set the volume of sounds for the game
    /// </summary>
    public void SetVolumeSounds(float volume)
    {
        audioMixer.SetFloat("playerVolume", volume);
        audioMixer.SetFloat("voiceVolume", volume);
        audioMixer.SetFloat("effectVolume", volume);

        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    /// <summary>
    /// Set the fullscreen for the game
    /// </summary>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        PlayerPrefs.SetString("fullScreen", isFullscreen.ToString());
    }

    /// <summary>
    /// Set the resolution of the game
    /// </summary>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutionsList[resolutionIndex];

        GameManager.Instance.currentHeight = resolution.height;
        GameManager.Instance.currentWidth = resolution.width;

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt("resolutionWidht", resolution.width);
        PlayerPrefs.SetInt("resolutionHeight", resolution.height);
    }
}
