using UnityEngine;
using UnityEngine.Audio;

public class LoadPlayerPrefs : MonoBehaviour
{
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Awake()
    {
        //Load resolution settings
        Screen.SetResolution(PlayerPrefs.GetInt("resolutionWidht", 1920), PlayerPrefs.GetInt("resolutionHeight", 1080), bool.Parse(PlayerPrefs.GetString("fullScreen", "False")) );

        //Load general music volume settings
        audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume", 0));

        //Load sound volume options
        audioMixer.SetFloat("playerVolume", PlayerPrefs.GetFloat("soundVolume", 0));
        audioMixer.SetFloat("voiceVolume", PlayerPrefs.GetFloat("soundVolume", 0));
        audioMixer.SetFloat("effectVolume", PlayerPrefs.GetFloat("soundVolume", 0));

        //Destroy this script when the setting are loaded
        Destroy(this);
    }

}
