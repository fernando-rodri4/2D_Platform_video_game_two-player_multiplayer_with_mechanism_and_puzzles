using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;

    [SerializeField] RectTransform sliderMusic, sliderSound;

    /// <summary>
    /// 
    /// </summary>
    Vector2 music = new Vector2(1, 1);
    Vector2 sound = new Vector2(1, 1);

    /// <summary>
    /// 
    /// </summary>
    bool fullScreen = true;

    /// <summary>
    /// 
    /// </summary>
    int resolution = 1;

    Resolution[] resolutions;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options =  new List<string>();
        string option;
        int currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolumeMusic(float volume)
    {
        music = sliderMusic.anchorMax;

        audioMixer.SetFloat("musicVolume", volume);
    }
    public void SetVolumeSounds(float volume)
    {
        sound = sliderSound.anchorMax;

        audioMixer.SetFloat("playerVolume", volume);
        audioMixer.SetFloat("voiceVolume", volume);
        audioMixer.SetFloat("effectVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        fullScreen = isFullscreen;

        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        this.resolution = resolutionIndex;
        
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    void Update()
    {
        if (sliderMusic.anchorMax != music)
        {
            sliderMusic.anchorMax = music;
            sliderSound.anchorMax = sound;
        }
    }
}
